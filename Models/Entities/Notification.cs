namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Người nhận
        public int SenderId { get; set; } // Người gửi
        public string Title { get; set; } = string.Empty; // Tiêu đề
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
