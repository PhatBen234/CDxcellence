using Unilever.CDExcellent.API.Models.Dto;

namespace Unilever.CDExcellent.API.Services
{
    public interface IVisitPlanService
    {
        Task<VisitPlanDto> CreateVisitPlanAsync(VisitPlanDto dto);
        Task<IEnumerable<VisitPlanDto>> GetAllVisitPlansAsync();
        Task<VisitPlanDto?> GetVisitPlanByIdAsync(int id);
        Task<VisitPlanDto?> UpdateVisitPlanAsync(int id, VisitPlanDto dto);
        Task<bool> DeleteVisitPlanAsync(int id);
    }
}
