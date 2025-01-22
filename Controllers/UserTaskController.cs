using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Services.IService;

namespace Unilever.CDExcellent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        private readonly IUserTaskService _userTaskService;

        public UserTaskController(IUserTaskService userTaskService)
        {
            _userTaskService = userTaskService;
        }

        // Create a new UserTask
        [HttpPost]
        public async Task<IActionResult> CreateUserTask([FromBody] UserTaskDto userTaskDto)
        {
            if (userTaskDto == null)
            {
                return BadRequest("Invalid task data.");
            }

            var createdUserTask = await _userTaskService.CreateUserTaskAsync(userTaskDto);
            return CreatedAtAction(nameof(GetUserTaskById), new { id = createdUserTask.Id }, createdUserTask);
        }

        // Update an existing UserTask
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserTask(int id, [FromBody] UserTaskDto userTaskDto)
        {
            if (userTaskDto == null || id != userTaskDto.Id)
            {
                return BadRequest("Invalid task data.");
            }

            var updatedUserTask = await _userTaskService.UpdateUserTaskAsync(id, userTaskDto);

            if (updatedUserTask == null)
            {
                return NotFound($"UserTask with id {id} not found.");
            }

            return Ok(updatedUserTask);
        }

        // Get UserTask by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserTaskById(int id)
        {
            var userTask = await _userTaskService.GetUserTaskByIdAsync(id);

            if (userTask == null)
            {
                return NotFound($"UserTask with id {id} not found.");
            }

            return Ok(userTask);
        }

        // Get all UserTasks by VisitPlanId
        [HttpGet("by-visit-plan/{visitPlanId}")]
        public async Task<IActionResult> GetUserTasksByVisitPlanId(int visitPlanId)
        {
            var userTasks = await _userTaskService.GetUserTasksByVisitPlanIdAsync(visitPlanId);

            if (userTasks == null || !userTasks.Any())
            {
                return NotFound($"No tasks found for VisitPlan with id {visitPlanId}.");
            }

            return Ok(userTasks);
        }

        // Delete a UserTask
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTask(int id)
        {
            var userTask = await _userTaskService.GetUserTaskByIdAsync(id);

            if (userTask == null)
            {
                return NotFound($"UserTask with id {id} not found.");
            }

            await _userTaskService.DeleteUserTaskAsync(id);
            return NoContent();
        }
    }
}
