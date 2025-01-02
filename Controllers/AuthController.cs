using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Services;
using Unilever.CDExcellent.API.Models;

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

        // Đăng ký người dùng mới
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Gọi dịch vụ đăng ký người dùng
            var result = await _authService.RegisterAsync(request);

            // Kiểm tra kết quả trả về và phản hồi với thông báo
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }

        // Đăng nhập người dùng và trả về JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Gọi dịch vụ đăng nhập và nhận kết quả
            var result = await _authService.LoginAsync(request);

            // Nếu đăng nhập không thành công
            if (!result.Success)
            {
                return Unauthorized(new { success = false, message = result.Message });
            }

            // Nếu đăng nhập thành công, trả về token và thông tin người dùng
            return Ok(new
            {
                success = true,
                message = "Login successful",
                token = result.Token,  // Token JWT
                fullName = result.FullName,
                role = result.Role
            });
        }

        // Quên mật khẩu - gửi OTP
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            // Gọi dịch vụ để gửi OTP cho việc đặt lại mật khẩu
            var result = await _authService.GenerateOtpForPasswordResetAsync(request.Email);

            // Nếu không tìm thấy email hoặc có lỗi
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            // Nếu gửi OTP thành công
            return Ok(new { success = true, message = "OTP sent to your email" });
        }

        // Đặt lại mật khẩu bằng OTP
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            // Gọi dịch vụ để thực hiện đặt lại mật khẩu
            var result = await _authService.ResetPasswordAsync(request.Email, request.Otp, request.NewPassword);

            // Nếu có lỗi khi đặt lại mật khẩu
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            // Nếu đặt lại mật khẩu thành công
            return Ok(new { success = true, message = "Password reset successful" });
        }
    }
}
