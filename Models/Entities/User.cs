using Unilever.CDExcellent.API.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "User";
    public ICollection<AreaUser> AreaUsers { get; set; } = new List<AreaUser>();

    // Thêm quan hệ với bài viết
    public ICollection<Article> Articles { get; set; } = new List<Article>();

    // Thêm quan hệ với bình luận
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public bool HasAdminPrivileges() => Role == "Admin" || Role == "Owner";
}
