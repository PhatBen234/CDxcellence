using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Unilever.CDExcellent.API.Models.Entities;
using Unilever.CDExcellent.API.Services;

namespace Unilever.CDExcellent.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUserRole()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim != null ? int.Parse(userIdClaim) : (int?)null;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found");

            // Chỉ Admin, Owner hoặc chính người dùng mới có thể xem thông tin
            var currentUserRole = GetCurrentUserRole();
            var currentUserId = GetCurrentUserId();

            if (currentUserRole != "Admin" && currentUserRole != "Owner" && currentUserId != id)
            {
                return Forbid("You are not authorized to view this user’s details.");
            }

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User newUser)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(newUser);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("You are not authorized to perform this action.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User user)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, user);
            if (updatedUser == null)
                return NotFound("User not found");

            return Ok(updatedUser);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userToDelete = await _userService.GetUserByIdAsync(id);
            if (userToDelete == null)
                return NotFound("User not found");

            // Admin không thể xóa Owner
            if (userToDelete.Role == "Owner" && GetCurrentUserRole() != "Owner")
            {
                return Forbid("You are not authorized to delete an Owner.");
            }

            var success = await _userService.DeleteUserAsync(id);
            if (!success)
                return StatusCode(500, "An error occurred while deleting user.");

            return NoContent();
        }

        // POST: api/users/import
        [HttpPost("import")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> ImportUsersFromExcel([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var success = await _userService.ImportUsersFromExcelAsync(file);
            if (!success)
                return StatusCode(500, "An error occurred while importing users");

            return Ok("Users imported successfully");
        }

        // GET: api/users/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsers([FromQuery] string query)
        {
            var users = await _userService.SearchUsersAsync(query);
            return Ok(users);
        }
    }
}
