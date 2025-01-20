namespace Unilever.CDExcellent.API.Models.Entities
{
    public class VisitPlanGuest
    {
        public int VisitPlanId { get; set; }
        public VisitPlan VisitPlan { get; set; } = null!;

        public int GuestId { get; set; }
        public User Guest { get; set; } = null!;
    }

}
