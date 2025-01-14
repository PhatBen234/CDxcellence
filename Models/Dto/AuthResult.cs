namespace Unilever.CDExcellent.API.Models.Dto
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; } = null;
        public string? FullName { get; set; } = null;
        public string? Role { get; set; } = null;
    }
}
