namespace Unilever.CDExcellent.API.Models.Dto
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;  // Token JWT
        public string FullName { get; set; } = string.Empty; // Tên người dùng
        public string Role { get; set; } = string.Empty;  // Vai trò
    }
}
