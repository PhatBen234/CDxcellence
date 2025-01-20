namespace Unilever.CDExcellent.API.Models.Entities
{
    public class AreaUser
    {
        public int AreaId { get; set; }
        public int UserId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public Area Area { get; set; } = null!;
        public User User { get; set; } = null!;
    }

}
