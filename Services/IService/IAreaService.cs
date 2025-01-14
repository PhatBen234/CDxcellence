using Microsoft.AspNetCore.Http;
using Unilever.CDExcellent.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unilever.CDExcellent.API.Services.IService
{
    public interface IAreaService
    {
        Task<Area> CreateAreaAsync(Area area);
        Task<IEnumerable<Area>> GetAllAreasAsync();
        Task<Area?> GetAreaByIdAsync(int id);
        Task<Area?> UpdateAreaAsync(int id, Area area);
        Task<bool> DeleteAreaAsync(int id);
        Task<bool> DeleteAreasAsync(List<int> areaIds, bool disable = false);
        Task<bool> AssignUsersToAreaAsync(int areaId, List<int> userIds);
        Task<bool> RemoveUsersFromAreaAsync(int areaId, List<int> userIds);
        Task<IEnumerable<User>> GetUsersByAreaIdAsync(int areaId);
        Task<string> ImportAreasAsync(IFormFile file);
    }
}
