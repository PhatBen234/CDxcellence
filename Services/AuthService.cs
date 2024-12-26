using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
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
    }
}
