namespace Unilever.CDExcellent.API.Models
{
    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;  // Tên đầy đủ của người dùng
        public string Email { get; set; } = string.Empty;     // Email
        public string Password { get; set; } = string.Empty;  // Mật khẩu
        public string Role { get; set; } = "User";            // Vai trò (mặc định là "User")
    }
}
