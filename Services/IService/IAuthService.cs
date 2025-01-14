using Unilever.CDExcellent.API.Models;
using Unilever.CDExcellent.API.Models.Dto;

namespace Unilever.CDExcellent.API.Services.IService
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);
        Task<AuthResult> GenerateOtpForPasswordResetAsync(string email);
        Task<AuthResult> ResetPasswordAsync(string email, string otp, string newPassword);
    }
}
