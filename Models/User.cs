namespace Unilever.CDExcellent.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }  // Full name of the user
        public string Email { get; set; }     // Email of the user
        public string Password { get; set; }  // Plain text password

        public string Role { get; set; }      // Role of the user (e.g., "User", "Admin")

        // Flag to indicate if the user is an admin
        public bool IsAdmin { get; set; } = false;  // Default to false

        // Fields for password reset
        // These fields will not be required when creating a user
        //public string ResetPasswordOtp { get; set; }  // OTP for password reset (not needed when creating user)
        //public DateTime? ResetPasswordOtpExpiry { get; set; } // Expiry time for OTP (not needed when creating user)
    }
}
