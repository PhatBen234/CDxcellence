using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);

        // Add method for login authentication
        Task<string> AuthenticateAsync(LoginRequest request);
    }
}
