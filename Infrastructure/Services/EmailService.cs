using Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmailService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public async Task SendPasswordResetEmailAsync(string userName, string toEmail, string resetLink)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var templatePath = Path.Combine(_env.ContentRootPath, "EmailTemplates", "PasswordReset.html");

            var templateText = await File.ReadAllTextAsync(templatePath);

            var emailBody = templateText.Replace("{{username}}", userName);
            emailBody = emailBody.Replace("{{resetLink}}", resetLink);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings["FromName"], emailSettings["FromAddress"]));
            message.To.Add(new MailboxAddress(userName, toEmail));
            message.Subject = "إعادة تعيين كلمة المرور";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = emailBody 
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), false);
            await client.AuthenticateAsync(emailSettings["UserName"], emailSettings["Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}