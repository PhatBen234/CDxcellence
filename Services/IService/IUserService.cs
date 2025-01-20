using Unilever.CDExcellent.API.Models.Entities;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User newUser);
    Task<IEnumerable<User>> SearchUsersAsync(string query);
    Task<User> UpdateUserAsync(int id, User user);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> ImportUsersFromExcelAsync(IFormFile file);
}
