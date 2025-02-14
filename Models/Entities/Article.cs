namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } 
        public bool IsPublished { get; set; } = false; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int AuthorId { get; set; }
        public User? Author { get; set; }

        public List<Comment>? Comments { get; set; }
    }

}
