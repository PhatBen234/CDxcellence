namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Người nhận thông báo
        public string Message { get; set; } = string.Empty; // Nội dung thông báo
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Thời gian tạo
        public bool IsRead { get; set; } = false; // Trạng thái đã đọc
    }
}
