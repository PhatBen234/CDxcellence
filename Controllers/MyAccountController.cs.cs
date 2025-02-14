using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Models.Dto;
using Unilever.CDExcellent.API.Services.IService;
using System.Security.Claims;

namespace Unilever.CDExcellent.API.Controllers
{
    [Route("api/my-account")]
    [ApiController]
    [Authorize] 
    public class MyAccountController : ControllerBase
    {
        private readonly IMyAccountService _myAccountService;

        public MyAccountController(IMyAccountService myAccountService)
        {
            _myAccountService = myAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyAccount()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0) return Unauthorized();

            var result = await _myAccountService.GetMyAccountAsync(userId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMyAccount([FromBody] UpdateUserDto updateUserDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0) return Unauthorized();

            var success = await _myAccountService.UpdateUserInfoAsync(userId, updateUserDto);
            if (!success) return BadRequest("Cập nhật thất bại");

            return Ok("Cập nhật thành công");
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (userId == 0) return Unauthorized();

            try
            {
                var success = await _myAccountService.ChangePasswordAsync(userId, changePasswordDto);
                if (!success) return BadRequest("Đổi mật khẩu thất bại");

                return Ok("Đổi mật khẩu thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
