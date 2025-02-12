namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Article>? Articles { get; set; }
    }

}
