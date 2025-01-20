namespace Unilever.CDExcellent.API.Services.IService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string body);
    }


}
