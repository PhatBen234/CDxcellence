using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models;
using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Services.IService;

namespace Unilever.CDExcellent.API.Services.Service
{
    public class DistributorService : IDistributorService
    {
        private readonly AppDbContext _context;

        public DistributorService(AppDbContext context)
        {
            _context = context;
        }

        // Create new distributor
        public async Task<DistributorDto> CreateDistributorAsync(DistributorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Address) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Phone))
            {
                throw new ArgumentException("All fields are required.");
            }

            var distributor = new Distributor
            {
                Name = dto.Name,
                Address = dto.Address,
                Email = dto.Email,
                Phone = dto.Phone,
                AreaId = dto.AreaId
            };

            _context.Distributors.Add(distributor);
            await _context.SaveChangesAsync();

            // Return the created distributor as a DTO
            return new DistributorDto
            {
                Id = distributor.Id,
                Name = distributor.Name,
                Address = distributor.Address,
                Email = distributor.Email,
                Phone = distributor.Phone,
                AreaId = distributor.AreaId
            };
        }

        // Get all distributors
        public async Task<IEnumerable<DistributorDto>> GetAllDistributorsAsync()
        {
            return await _context.Distributors
                .Include(d => d.Area) // Include related Area
                .Select(d => new DistributorDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Address = d.Address,
                    Email = d.Email,
                    Phone = d.Phone,
                    AreaId = d.AreaId
                })
                .ToListAsync();
        }

        // Get distributor by ID
        public async Task<DistributorDto?> GetDistributorByIdAsync(int id)
        {
            var distributor = await _context.Distributors
                .Include(d => d.Area) // Include related Area
                .FirstOrDefaultAsync(d => d.Id == id);

            if (distributor == null)
                return null;

            // Convert to DTO
            return new DistributorDto
            {
                Id = distributor.Id,
                Name = distributor.Name,
                Address = distributor.Address,
                Email = distributor.Email,
                Phone = distributor.Phone,
                AreaId = distributor.AreaId
            };
        }

        // Update distributor by ID
        public async Task<DistributorDto?> UpdateDistributorAsync(int id, DistributorDto dto)
        {
            var existingDistributor = await _context.Distributors.FindAsync(id);
            if (existingDistributor == null)
                return null;

            // Update fields
            existingDistributor.Name = dto.Name;
            existingDistributor.Address = dto.Address;
            existingDistributor.Email = dto.Email;
            existingDistributor.Phone = dto.Phone;
            existingDistributor.AreaId = dto.AreaId;

            await _context.SaveChangesAsync();

            // Convert updated distributor to DTO
            return new DistributorDto
            {
                Id = existingDistributor.Id,
                Name = existingDistributor.Name,
                Address = existingDistributor.Address,
                Email = existingDistributor.Email,
                Phone = existingDistributor.Phone,
                AreaId = existingDistributor.AreaId
            };
        }

        // Delete distributor by ID
        public async Task<bool> DeleteDistributorAsync(int id)
        {
            var distributor = await _context.Distributors.FindAsync(id);
            if (distributor == null)
                return false;

            _context.Distributors.Remove(distributor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
