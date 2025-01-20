namespace Unilever.CDExcellent.API.Models.Entities
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; } = string.Empty; // Email người dùng
        public string NewPassword { get; set; } = string.Empty; // Mật khẩu mới
        public string Otp { get; set; } = string.Empty; // Mã OTP
    }
}
