namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Distributor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Tên nhà phân phối
        public string Address { get; set; } = string.Empty; // Địa chỉ
        public string Email { get; set; } = string.Empty; // Email
        public string Phone { get; set; } = string.Empty; // Số điện thoại

        // Khóa ngoại
        public int AreaId { get; set; }
        public Area Area { get; set; } = null!;
        public List<VisitPlan> VisitPlans { get; set; } = new List<VisitPlan>();
    }

}
