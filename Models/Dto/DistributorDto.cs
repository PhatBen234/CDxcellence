namespace Unilever.CDExcellent.API.Models.Dto
{
    public class DistributorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int AreaId { get; set; }
    }

}
