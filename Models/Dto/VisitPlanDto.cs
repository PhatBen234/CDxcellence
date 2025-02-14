namespace Unilever.CDExcellent.API.Models.Dto
{
    public class VisitPlanDto
    {
        public int Id { get; set; }
        public int ActorId { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitTime { get; set; }
        public int DistributorId { get; set; }
        public string Purpose { get; set; }
        public bool IsConfirmed { get; set; }
        public string VisitStatus { get; set; }
        public List<int> GuestIds { get; set; } = new();
        public List<UserTaskDto> Tasks { get; set; } = new(); 
    }

}
