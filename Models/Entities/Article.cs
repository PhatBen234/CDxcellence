namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }  // Ảnh đại diện bài viết
        public bool IsPublished { get; set; } = false; // Trạng thái đăng tải
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Liên kết với danh mục
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Liên kết với tác giả bài viết
        public int AuthorId { get; set; }
        public User? Author { get; set; }

        // Danh sách bình luận
        public List<Comment>? Comments { get; set; }
    }

}
