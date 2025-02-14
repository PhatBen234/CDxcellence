using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Services.IService;
using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Models.Entities;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IEmailService emailService, IConfiguration configuration)
    {
        _context = context;
        _emailService = emailService;
        _configuration = configuration;
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
            Role = request.Role
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
        var expires = DateTime.UtcNow.AddDays(1);

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

        _context.OtpCodes.RemoveRange(_context.OtpCodes.Where(o => o.Email == email));
        await _context.SaveChangesAsync();

        var otpEntry = new OtpCode
        {
            Email = email,
            Code = otp,
            ExpiryTime = DateTime.UtcNow.AddMinutes(10)
        };

        await _context.OtpCodes.AddAsync(otpEntry);
        await _context.SaveChangesAsync();

        var subject = "Password Reset OTP";
        var body = $"Dear {user.FullName},\n\nYour OTP for password reset is: {otp}.\nThis OTP is valid for 10 minutes.\n\nBest regards,\nCDExcellent Support Team";

        await _emailService.SendEmailAsync(user.Email, subject, body);

        return new AuthResult
        {
            Success = true,
            Message = "OTP sent to email."
        };
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

        var storedOtp = await _context.OtpCodes.FirstOrDefaultAsync(o => o.Email == email && o.Code == otp);
        if (storedOtp == null || storedOtp.ExpiryTime < DateTime.UtcNow)
        {
            return new AuthResult
            {
                Success = false,
                Message = "Invalid or expired OTP."
            };
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _context.SaveChangesAsync();

        _context.OtpCodes.Remove(storedOtp);
        await _context.SaveChangesAsync();

        return new AuthResult
        {
            Success = true,
            Message = "Password reset successfully."
        };
    }
}
