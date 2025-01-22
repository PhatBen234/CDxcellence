namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string Message { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
