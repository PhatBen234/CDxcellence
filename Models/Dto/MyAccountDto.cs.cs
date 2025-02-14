namespace Unilever.CDExcellent.API.Models.Dto
{
    public class MyAccountDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty; // Đồng bộ với User.FullName
        public DateTime StartDate { get; set; }
        public double Rating { get; set; } = 0.0;
        public List<CommentDto> LatestComments { get; set; } = new();
        public List<UserTaskDto> DoneTasks { get; set; } = new();
        public List<UserTaskDto> NotDoneTasks { get; set; } = new();
    }
}
