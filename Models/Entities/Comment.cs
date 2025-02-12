namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Liên kết với bài viết
        public int ArticleId { get; set; }
        public Article? Article { get; set; }

        // Liên kết với người bình luận
        public int UserId { get; set; }
        public User? User { get; set; }
    }

}
