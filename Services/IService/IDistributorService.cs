using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Services.IService
{
    public interface IDistributorService
    {
        Task<DistributorDto> CreateDistributorAsync(DistributorDto dto);
        Task<IEnumerable<DistributorDto>> GetAllDistributorsAsync();
        Task<DistributorDto?> GetDistributorByIdAsync(int id);
        Task<DistributorDto?> UpdateDistributorAsync(int id, DistributorDto dto);
        Task<bool> DeleteDistributorAsync(int id);
    }
}
