using Unilever.CDExcellent.API.Models.Dto;
namespace Unilever.CDExcellent.API.Services.IService
{
    public interface IUserTaskService
    {
        Task<UserTaskDto> CreateUserTaskAsync(UserTaskDto userTaskDto);
        Task<UserTaskDto> UpdateUserTaskAsync(int id, UserTaskDto userTaskDto);
        Task<IEnumerable<UserTaskDto>> GetUserTasksByVisitPlanIdAsync(int visitPlanId);
        Task<UserTaskDto> GetUserTaskByIdAsync(int id);
        Task DeleteUserTaskAsync(int id);
    }
}
