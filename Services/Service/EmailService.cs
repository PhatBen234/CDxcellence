using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Unilever.CDExcellent.API.Services.IService;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly string _smtpUser;
    private readonly string _smtpPassword;
    private readonly int _smtpPort;

    public EmailService(IConfiguration configuration)
    {
        var smtpSettings = configuration.GetSection("SmtpSettings");
        _smtpServer = smtpSettings["Server"];
        _smtpUser = smtpSettings["User"];
        _smtpPassword = smtpSettings["Password"];
        _smtpPort = int.Parse(smtpSettings["Port"]);
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("CDExcellent Support", _smtpUser));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;

        message.Body = new TextPart("plain") { Text = body };

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
