using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Models.Entities;
using Unilever.CDExcellent.API.Services.IService;

namespace Unilever.CDExcellent.API.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly AppDbContext _context;

        public UserTaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserTaskDto> CreateUserTaskAsync(UserTaskDto userTaskDto)
        {
            var userTask = new UserTask
            {
                VisitPlanId = userTaskDto.VisitPlanId,
                AssigneeId = userTaskDto.AssigneeId,
                Title = userTaskDto.Title,
                Description = userTaskDto.Description,
                StartDate = userTaskDto.StartDate,
                EndDate = userTaskDto.EndDate
            };

            _context.UserTasks.Add(userTask);
            await _context.SaveChangesAsync();

            return new UserTaskDto
            {
                Id = userTask.Id,
                VisitPlanId = userTask.VisitPlanId,
                AssigneeId = userTask.AssigneeId,
                Title = userTask.Title,
                Description = userTask.Description,
                StartDate = userTask.StartDate,
                EndDate = userTask.EndDate
            };
        }

        public async Task<UserTaskDto> UpdateUserTaskAsync(int id, UserTaskDto userTaskDto)
        {
            var existingUserTask = await _context.UserTasks.FindAsync(id);

            if (existingUserTask == null)
                return null;

            existingUserTask.VisitPlanId = userTaskDto.VisitPlanId;
            existingUserTask.AssigneeId = userTaskDto.AssigneeId;
            existingUserTask.Title = userTaskDto.Title;
            existingUserTask.Description = userTaskDto.Description;
            existingUserTask.StartDate = userTaskDto.StartDate;
            existingUserTask.EndDate = userTaskDto.EndDate;

            _context.UserTasks.Update(existingUserTask);
            await _context.SaveChangesAsync();

            return new UserTaskDto
            {
                Id = existingUserTask.Id,
                VisitPlanId = existingUserTask.VisitPlanId,
                AssigneeId = existingUserTask.AssigneeId,
                Title = existingUserTask.Title,
                Description = existingUserTask.Description,
                StartDate = existingUserTask.StartDate,
                EndDate = existingUserTask.EndDate
            };
        }

        public async Task<IEnumerable<UserTaskDto>> GetUserTasksByVisitPlanIdAsync(int visitPlanId)
        {
            var userTasks = await _context.UserTasks
                .Where(ut => ut.VisitPlanId == visitPlanId)
                .ToListAsync();

            return userTasks.Select(ut => new UserTaskDto
            {
                Id = ut.Id,
                VisitPlanId = ut.VisitPlanId,
                AssigneeId = ut.AssigneeId,
                Title = ut.Title,
                Description = ut.Description,
                StartDate = ut.StartDate,
                EndDate = ut.EndDate
            });
        }

        public async Task<UserTaskDto> GetUserTaskByIdAsync(int id)
        {
            var userTask = await _context.UserTasks.FindAsync(id);

            if (userTask == null) return null;

            return new UserTaskDto
            {
                Id = userTask.Id,
                VisitPlanId = userTask.VisitPlanId,
                AssigneeId = userTask.AssigneeId,
                Title = userTask.Title,
                Description = userTask.Description,
                StartDate = userTask.StartDate,
                EndDate = userTask.EndDate
            };
        }

        public async Task DeleteUserTaskAsync(int id)
        {
            var userTask = await _context.UserTasks.FindAsync(id);

            if (userTask != null)
            {
                _context.UserTasks.Remove(userTask);
                await _context.SaveChangesAsync();
            }
        }
    }
}
