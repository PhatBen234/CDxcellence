using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);
    }
}
