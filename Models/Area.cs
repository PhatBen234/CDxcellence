namespace Unilever.CDExcellent.API.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // Unique identifier
        public string Name { get; set; } = string.Empty; // Required
        public string City { get; set; } = string.Empty;
        public bool Status { get; set; } = true; // Active by default
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Distributor> Distributors { get; set; } = new List<Distributor>();
        public ICollection<AreaUser> AreaUsers { get; set; } = new List<AreaUser>();
    }

    public class Distributor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Required
        public string Address { get; set; } = string.Empty; // Required
        public string Email { get; set; } = string.Empty; // Required
        public string Phone { get; set; } = string.Empty; // Required
        public int AreaId { get; set; } // Foreign key
        public Area Area { get; set; } = null!;
    }

    public class AreaUser
    {
        public int AreaId { get; set; } 
        public int UserId { get; set; } 
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public Area Area { get; set; } = null!;
        public User User { get; set; } = null!;
    }

}
