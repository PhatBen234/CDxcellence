using System.Threading.Tasks;
using Unilever.CDExcellent.API.Models.Dto;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardDataAsync(int userId);
}
