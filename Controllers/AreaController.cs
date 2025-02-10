using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Models.Entities;
using Unilever.CDExcellent.API.Services.IService;

namespace Unilever.CDExcellent.API.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAllAreas()
        {
            var areas = await _areaService.GetAllAreasAsync();
            return Ok(areas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Area>> GetAreaById(int id)
        {
            var area = await _areaService.GetAreaByIdAsync(id);
            if (area == null)
                return NotFound();

            return Ok(area);
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        public async Task<ActionResult<Area>> CreateArea([FromBody] Area area)
        {
            var createdArea = await _areaService.CreateAreaAsync(area);
            return CreatedAtAction(nameof(GetAreaById), new { id = createdArea.Id }, createdArea);
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArea(int id, [FromBody] Area area)
        {
            var updatedArea = await _areaService.UpdateAreaAsync(id, area);
            if (updatedArea == null)
                return NotFound("Area not found.");

            return Ok(updatedArea);
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArea(int id)
        {
            var success = await _areaService.DeleteAreaAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAreas([FromBody] List<int> areaIds, [FromQuery] bool disable = false)
        {
            if (areaIds == null || !areaIds.Any())
                return BadRequest("Area IDs are required.");

            var result = await _areaService.DeleteAreasAsync(areaIds, disable);
            if (!result)
                return NotFound("No areas found with the provided IDs.");

            var message = disable ? "Areas have been disabled successfully." : "Areas have been deleted successfully.";
            return Ok(new { message });
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPost("import")]
        public async Task<IActionResult> ImportAreas([FromForm] IFormFile file)
        {
            try
            {
                var result = await _areaService.ImportAreasAsync(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPost("{areaId}/users/assign")]
        public async Task<IActionResult> AssignUsersToArea(int areaId, [FromBody] List<int> userIds)
        {
            if (userIds == null || !userIds.Any())
                return BadRequest("UserIds cannot be null or empty.");

            var result = await _areaService.AssignUsersToAreaAsync(areaId, userIds);
            if (!result)
                return NotFound("Area or some users not found.");

            return Ok("Users assigned to the area successfully.");
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpDelete("{areaId}/users/remove")]
        public async Task<IActionResult> RemoveUsersFromArea(int areaId, [FromBody] List<int> userIds)
        {
            if (userIds == null || !userIds.Any())
                return BadRequest("UserIds cannot be null or empty.");

            var result = await _areaService.RemoveUsersFromAreaAsync(areaId, userIds);
            if (!result)
                return NotFound("Area or some users not found.");

            return Ok("Users removed from the area successfully.");
        }

        [HttpGet("{areaId}/users")]
        public async Task<IActionResult> GetUsersByAreaId(int areaId)
        {
            var users = await _areaService.GetUsersByAreaIdAsync(areaId);
            if (!users.Any())
                return NotFound("No users found for this area.");

            return Ok(users);
        }
    }
}
