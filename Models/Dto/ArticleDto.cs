namespace Unilever.CDExcellent.API.Models.Dto
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; } 
        public string CategoryName { get; set; }
    }

}
