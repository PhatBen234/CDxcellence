using Unilever.CDExcellent.API.Data;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Unilever.CDExcellent.API.Services.IService;
using Unilever.CDExcellent.API.Models.Entities;

namespace Unilever.CDExcellent.API.Services.Service
{
    public class AreaService : IAreaService
    {
        private readonly AppDbContext _context;

        public AreaService(AppDbContext context)
        {
            _context = context;
        }

        // Create new area
        public async Task<Area> CreateAreaAsync(Area area)
        {
            _context.Areas.Add(area);
            await _context.SaveChangesAsync();
            return area;
        }

        // Get list of all areas
        public async Task<IEnumerable<Area>> GetAllAreasAsync()
        {
            return await _context.Areas.ToListAsync();
        }

        // Get area by ID
        public async Task<Area> GetAreaByIdAsync(int id)
        {
            return await _context.Areas
                .Include(a => a.Distributors)
                .Include(a => a.AreaUsers)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // Update area by ID
        public async Task<Area> UpdateAreaAsync(int id, Area area)
        {
            var existingArea = await _context.Areas.FindAsync(id);
            if (existingArea == null) return null;

            // Validation: Area Name is required
            if (string.IsNullOrWhiteSpace(area.Name))
                throw new ArgumentException("Area Name is required.");

            // Update fields
            existingArea.Code = area.Code;
            existingArea.Name = area.Name;
            existingArea.City = area.City;
            existingArea.Status = area.Status;
            existingArea.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingArea;
        }


        // Delete area by ID
        public async Task<bool> DeleteAreaAsync(int id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null) return false;

            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAreasAsync(List<int> areaIds, bool disable = false)
        {
            var areas = await _context.Areas.Where(a => areaIds.Contains(a.Id)).ToListAsync();

            if (!areas.Any()) return false;

            if (disable)
            {
                foreach (var area in areas)
                {
                    area.Status = false; // Disable by setting status to inactive
                    area.UpdatedAt = DateTime.UtcNow;
                }
            }
            else
            {
                _context.Areas.RemoveRange(areas); // Delete permanently
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignUsersToAreaAsync(int areaId, List<int> userIds)
        {
            var area = await _context.Areas.FindAsync(areaId);
            if (area == null) return false;

            var existingUsers = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            if (!existingUsers.Any()) return false;

            foreach (var user in existingUsers)
            {
                if (!_context.AreaUsers.Any(au => au.AreaId == areaId && au.UserId == user.Id))
                {
                    _context.AreaUsers.Add(new AreaUser
                    {
                        AreaId = areaId,
                        UserId = user.Id
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUsersFromAreaAsync(int areaId, List<int> userIds)
        {
            var areaUsers = await _context.AreaUsers
                .Where(au => au.AreaId == areaId && userIds.Contains(au.UserId))
                .ToListAsync();

            if (!areaUsers.Any()) return false;

            _context.AreaUsers.RemoveRange(areaUsers);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<User>> GetUsersByAreaIdAsync(int areaId)
        {
            var users = await _context.AreaUsers
                .Where(au => au.AreaId == areaId)
                .Select(au => au.User)
                .ToListAsync();

            return users;
        }

        public async Task<string> ImportAreasAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            List<Area> areas = new List<Area>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; 
                    int rowCount = worksheet.Dimension.Rows; 

                    for (int row = 2; row <= rowCount; row++) 
                    {
                        var name = worksheet.Cells[row, 1].Text; // Column 1: Name
                        var code = worksheet.Cells[row, 2].Text; // Column 2: Code
                        var city = worksheet.Cells[row, 3].Text; // Column 3: City
                        var status = worksheet.Cells[row, 4].Text.ToLower() == "true"; // Column 4: Status

                        if (string.IsNullOrWhiteSpace(name))
                            throw new ArgumentException($"Row {row}: Name is required.");

                        areas.Add(new Area
                        {
                            Name = name,
                            Code = code,
                            City = city,
                            Status = status,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                        });
                    }
                }
            }

            _context.Areas.AddRange(areas);
            await _context.SaveChangesAsync();
            return $"Successfully imported {areas.Count} areas.";
        }



    }
}
