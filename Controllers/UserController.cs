using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Services;
using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Example endpoint for getting all users
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
