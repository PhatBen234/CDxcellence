using Microsoft.AspNetCore.Mvc;
using Unilever.CDExcellent.API.Models.Entities;
using Unilever.CDExcellent.API.Services.IService;

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

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Call the service to register a new user
            var result = await _authService.RegisterAsync(request);

            // Check the result and respond with an appropriate message
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }

        // Authenticate a user and return a JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Call the service to authenticate the user and get the result
            var result = await _authService.LoginAsync(request);

            // If authentication fails
            if (!result.Success)
            {
                return Unauthorized(new { success = false, message = result.Message });
            }

            // If authentication is successful, return the token and user details
            return Ok(new
            {
                success = true,
                message = "Login successful",
                token = result.Token,  // JWT token
                fullName = result.FullName,
                role = result.Role
            });
        }

        // Forgot password - send OTP
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            // Call the service to send OTP for password reset
            var result = await _authService.GenerateOtpForPasswordResetAsync(request.Email);

            // If the email is not found or an error occurs
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            // If OTP is sent successfully
            return Ok(new { success = true, message = "OTP sent to your email" });
        }

        // Reset password using OTP
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            // Call the service to reset the password
            var result = await _authService.ResetPasswordAsync(request.Email, request.Otp, request.NewPassword);

            // If an error occurs during password reset
            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            // If password reset is successful
            return Ok(new { success = true, message = "Password reset successful" });
        }
    }
}
