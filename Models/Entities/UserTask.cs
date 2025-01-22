namespace Unilever.CDExcellent.API.Models.Entities
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VisitPlanId { get; set; }
        public VisitPlan VisitPlan { get; set; }
        public int AssigneeId { get; set; }
        public User Assignee { get; set; }

    }
}
