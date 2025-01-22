﻿namespace Unilever.CDExcellent.API.Models.Entities
{
    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty;  
        public string Role { get; set; } = "User";  
    }
}
