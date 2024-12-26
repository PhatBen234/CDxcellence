using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Services;
using Unilever.CDExcellent.API.Models;
using System.Linq;

namespace Unilever.CDExcellent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.Success)
            {
                return Unauthorized(new { success = false, message = result.Message });
            }

            return Ok(new
            {
                success = true,
                message = "Login successful",
                token = result.Token,
                fullName = result.FullName,
                role = result.Role
            });
        }
    }
}
