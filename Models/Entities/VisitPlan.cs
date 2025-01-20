using Unilever.CDExcellent.API.Models.Entities;

public class VisitPlan
{
    public int Id { get; set; }
    public int ActorId { get; set; }
    public DateTime VisitDate { get; set; }
    public string VisitTime { get; set; }
    public int DistributorId { get; set; }
    public string Purpose { get; set; }
    public bool IsConfirmed { get; set; }

    public Distributor Distributor { get; set; } = null!;

    // Danh sách khách mời
    public List<VisitPlanGuest> Guests { get; set; } = new();
}
