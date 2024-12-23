using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Unilever.CDExcellent.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<User>> GetAllUsersAsync()  // Return type should be List<User>
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return null; // Or throw an exception
            }

            existingUser.FullName = user.FullName ?? existingUser.FullName;
            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.PasswordHash = user.PasswordHash ?? existingUser.PasswordHash;
            existingUser.Role = user.Role ?? existingUser.Role;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
