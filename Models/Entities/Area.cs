namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // Mã khu vực
        public string Name { get; set; } = string.Empty; // Tên khu vực
        public string City { get; set; } = string.Empty; // Thành phố
        public bool Status { get; set; } = true; // Trạng thái (active mặc định là true)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Quan hệ với Distributor
        public ICollection<Distributor> Distributors { get; set; } = new List<Distributor>();

        // Quan hệ với User qua bảng AreaUser
        public ICollection<AreaUser> AreaUsers { get; set; } = new List<AreaUser>();
    }


}
