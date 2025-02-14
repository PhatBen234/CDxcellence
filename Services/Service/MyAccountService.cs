using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Models.Entities;
using Unilever.CDExcellent.API.Services.IService;
using BCrypt.Net; // Thêm thư viện mã hóa mật khẩu

namespace Unilever.CDExcellent.API.Services.Service
{
    public class MyAccountService : IMyAccountService
    {
        private readonly AppDbContext _context;

        public MyAccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MyAccountDto> GetMyAccountAsync(int userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Comments)  // Include danh sách comments
                .Include(u => u.UserTasks) // Include danh sách tasks
                .Select(u => new MyAccountDto
                {
                    Id = u.Id,
                    FullName = u.FullName, // Sửa Name -> FullName
                    StartDate = u.CreatedAt,
                    Rating = 4.5, // Placeholder
                    LatestComments = u.Comments
                        .OrderByDescending(c => c.CreatedAt)
                        .Take(5)
                        .Select(c => new CommentDto
                        {
                            Id = c.Id,
                            Content = c.Content,
                            CreatedAt = c.CreatedAt,
                            UserId = c.UserId,
                            UserName = u.FullName // Sửa Name -> FullName
                        }).ToList(),
                    DoneTasks = u.UserTasks
                        .Where(t => t.Status == "Done")
                        .Select(t => new UserTaskDto
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Description = t.Description,
                            StartDate = t.StartDate,
                            EndDate = t.EndDate
                        }).ToList(),
                    NotDoneTasks = u.UserTasks
                        .Where(t => t.Status != "Done")
                        .Select(t => new UserTaskDto
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Description = t.Description,
                            StartDate = t.StartDate,
                            EndDate = t.EndDate
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return user ?? throw new Exception("User not found");
        }

        public async Task<bool> UpdateUserInfoAsync(int userId, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Kiểm tra nếu email đã tồn tại
            if (await _context.Users.AnyAsync(u => u.Email == updateUserDto.Email && u.Id != userId))
            {
                throw new Exception("Email already in use");
            }

            user.FullName = updateUserDto.FullName; // Sửa Name -> FullName
            user.Email = updateUserDto.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Kiểm tra mật khẩu cũ (Hash)
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, user.Password))
                throw new Exception("Old password is incorrect");

            // Hash mật khẩu mới trước khi lưu
            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
