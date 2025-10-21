using Application.Common.Interfaces;
using Application.Interfaces;
using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Runtime;
using System.Threading.Tasks;

public class SmtpEmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public SmtpEmailService(IOptions<EmailSettings> emailSettings)
    {
        _settings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        using (var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort))
        {
            client.EnableSsl = _settings.EnableSsl;
            client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

            var message = new MailMessage
            {
                From = new MailAddress(_settings.FromAddress, _settings.FromName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            await client.SendMailAsync(message);
        }
    }

}
