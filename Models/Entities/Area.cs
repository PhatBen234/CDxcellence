namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; 
        public string Name { get; set; } = string.Empty; 
        public string City { get; set; } = string.Empty; 
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Distributor> Distributors { get; set; } = new List<Distributor>();
        public ICollection<AreaUser> AreaUsers { get; set; } = new List<AreaUser>();
    }


}
