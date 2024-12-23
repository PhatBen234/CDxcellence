namespace Unilever.CDExcellent.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }  // Nullable
        public string? Email { get; set; }     // Nullable
        public string? PasswordHash { get; set; }  // Nullable
        public string? Role { get; set; }      // Nullable
    }


}
