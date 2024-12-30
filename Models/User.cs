namespace Unilever.CDExcellent.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }  // Nullable
        public string? Email { get; set; }     // Nullable
        public string? PasswordHash { get; set; }  // Nullable
        public string? Role { get; set; }      // Nullable

        // Fields for password reset
        public string? ResetPasswordOtp { get; set; }  // OTP for password reset
        public DateTime? ResetPasswordOtpExpiry { get; set; } // Expiry time for OTP
    }
}
