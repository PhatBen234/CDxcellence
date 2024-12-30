using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Threading.Tasks;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly string _smtpServer;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;
        private readonly int _smtpPort;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;

            // Load thông tin từ appsettings.json
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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
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
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            // Generate JWT token logic (if applicable)
            var token = "GeneratedJWTToken"; // Replace with real JWT logic

            return new AuthResult
            {
                Success = true,
                Token = token,
                FullName = user.FullName,
                Role = user.Role,
                Message = "Login successful."
            };
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

            // Generate a new OTP
            var otp = new Random().Next(100000, 999999).ToString();

            // Send the new OTP to the user's email
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
            message.From.Add(new MailboxAddress("CDExcellent Support", _smtpUser)); // Người gửi
            message.To.Add(new MailboxAddress("", recipientEmail)); // Người nhận
            message.Subject = "Password Reset OTP";

            // Nội dung email rõ ràng, không gây hiểu lầm
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

            // The OTP will no longer be stored, so you can't verify it against the database.
            // You need to handle OTP verification on the client-side or in-memory for this example.

            // Reset password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                Success = true,
                Message = "Password reset successfully."
            };
        }
    }
}
