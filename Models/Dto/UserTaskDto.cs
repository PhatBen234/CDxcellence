namespace Unilever.CDExcellent.API.Models.Dto
{
    public class UserTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VisitPlanId { get; set; }
        public int AssigneeId { get; set; }
    }
}
