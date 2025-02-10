using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models.Entities;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private bool HasPermission()
    {
        var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        return userRole == "Admin" || userRole == "Owner";
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        if (!HasPermission())
            throw new UnauthorizedAccessException("You must be an Admin or Owner to access this resource.");

        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> SearchUsersAsync(string query)
    {
        var usersQuery = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            usersQuery = usersQuery.Where(u => u.FullName.Contains(query) || u.Email.Contains(query));
        }

        return await usersQuery.AsNoTracking().ToListAsync();
    }

    public async Task<User> CreateUserAsync(User newUser)
    {
        var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

        if (!HasPermission() || (newUser.Role == "Admin" && userRole != "Admin"))
        {
            throw new UnauthorizedAccessException("Only Admin can create another Admin user.");
        }

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
        newUser.Role = string.IsNullOrEmpty(newUser.Role) ? "User" : newUser.Role;

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    public async Task<User> UpdateUserAsync(int id, User user)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null) return null;

        if (user.Role != existingUser.Role && !HasPermission())
            throw new UnauthorizedAccessException("Only Admin or Owner can change user roles.");

        existingUser.FullName = user.FullName;
        existingUser.Email = user.Email;

        if (!string.IsNullOrEmpty(user.Password))
        {
            existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        }

        existingUser.Role = user.Role;

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        if (!HasPermission())
            throw new UnauthorizedAccessException("You must be an Admin or Owner to delete users.");

        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ImportUsersFromExcelAsync(IFormFile file)
    {
        if (!HasPermission())
            throw new UnauthorizedAccessException("You must be an Admin or Owner to import users.");

        try
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var users = new List<User>();

                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var user = new User
                        {
                            FullName = worksheet.Cells[row, 1].Text,
                            Email = worksheet.Cells[row, 2].Text,
                            Password = BCrypt.Net.BCrypt.HashPassword("defaultpassword"),
                            Role = worksheet.Cells[row, 3].Text
                        };

                        users.Add(user);
                    }
                }

                _context.Users.AddRange(users);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing users from Excel: {ex.Message}");
            return false;
        }
    }
}
