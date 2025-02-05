namespace Unilever.CDExcellent.API.Models.Dto
{
    public class CreateNotificationRequest
    {
        public int ReceiverId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

}
