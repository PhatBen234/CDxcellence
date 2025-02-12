﻿namespace Unilever.CDExcellent.API.Models.Entities
{
    public class OtpCode
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
    }
}
