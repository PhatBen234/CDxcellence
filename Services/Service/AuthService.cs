using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models;
using MailKit.Net.Smtp;
using Unilever.CDExcellent.API.Services.IService;
using Unilever.CDExcellent.API.Models.Dto;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly string _smtpServer;
    private readonly string _smtpUser;
    private readonly string _smtpPassword;
    private readonly int _smtpPort;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;

        var smtpSettings = configuration.GetSection("SmtpSettings");
        _smtpServer = smtpSettings["Server"];
        _smtpUser = smtpSettings["User"];
        _smtpPassword = smtpSettings["Password"];
        _smtpPort = int.Parse(smtpSettings["Port"]);
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return new AuthResult
            {
                Success = false,
                Message = "Email already exists."
            };
        }

        var newUser = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "User"
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return new AuthResult
        {
            Success = true,
            Message = "User registered successfully."
        };
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return new AuthResult
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        var token = GenerateJwtToken(user);

        return new AuthResult
        {
            Success = true,
            Token = token,
            FullName = user.FullName,
            Role = user.Role,
            Message = "Login successful."
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.NameId, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(1);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResult> GenerateOtpForPasswordResetAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return new AuthResult
            {
                Success = false,
                Message = "Email not found."
            };
        }

        var otp = new Random().Next(100000, 999999).ToString();

        await SendOtpEmailAsync(user.Email, otp);

        return new AuthResult
        {
            Success = true,
            Message = "New OTP sent to email."
        };
    }

    private async Task SendOtpEmailAsync(string recipientEmail, string otp)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("CDExcellent Support", _smtpUser));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = "Password Reset OTP";

        message.Body = new TextPart("plain")
        {
            Text = $"Dear User,\n\nYour OTP for password reset is: {otp}.\nThis OTP is valid for 10 minutes.\n\nBest regards,\nCDExcellent Support Team"
        };

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }

    public async Task<AuthResult> ResetPasswordAsync(string email, string otp, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return new AuthResult
            {
                Success = false,
                Message = "Email not found."
            };
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _context.SaveChangesAsync();

        return new AuthResult
        {
            Success = true,
            Message = "Password reset successfully."
        };
    }
}
