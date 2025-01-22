namespace Unilever.CDExcellent.API.Models.Entities
{
    public class Distributor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int AreaId { get; set; }
        public Area Area { get; set; } = null!;
        public List<VisitPlan> VisitPlans { get; set; } = new List<VisitPlan>();
    }

}
