﻿namespace Unilever.CDExcellent.API.Models
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;  // Email người dùng
        public string Password { get; set; } = string.Empty;  // Mật khẩu
    }
}
