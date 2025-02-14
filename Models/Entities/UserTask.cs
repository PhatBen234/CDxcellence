namespace Unilever.CDExcellent.API.Models.Entities
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Pending";

        public int VisitPlanId { get; set; }
        public VisitPlan VisitPlan { get; set; } = null!;
        public int AssigneeId { get; set; }
        public User Assignee { get; set; } = null!;
    }
}
