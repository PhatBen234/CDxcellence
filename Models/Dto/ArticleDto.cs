namespace Unilever.CDExcellent.API.Models.Dto
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; } // Chỉ lưu Id để tránh vòng lặp
        public string CategoryName { get; set; } // Nếu cần hiển thị tên danh mục
    }

}
