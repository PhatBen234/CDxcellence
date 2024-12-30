using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);

        // Generate OTP for password reset
        Task<AuthResult> GenerateOtpForPasswordResetAsync(string email);

        // Reset password with OTP
        Task<AuthResult> ResetPasswordAsync(string email, string otp, string newPassword);
    }
}
