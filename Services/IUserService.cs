using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<List<User>> GetAllUsersAsync();  // Return type should be List<User>
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
