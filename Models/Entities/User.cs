namespace Unilever.CDExcellent.API.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Thêm CreatedAt

        public ICollection<AreaUser> AreaUsers { get; set; } = new List<AreaUser>();
        public ICollection<Article> Articles { get; set; } = new List<Article>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>(); 

        public bool HasAdminPrivileges() => Role == "Admin" || Role == "Owner";
    }
}
