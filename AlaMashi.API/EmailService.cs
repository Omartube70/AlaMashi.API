using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AlaMashi.Services 

{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("AlaMashi", _configuration["EmailSettings:SenderEmail"]));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "إعادة تعيين كلمة المرور";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h1>إعادة تعيين كلمة المرور</h1>" +
                           $"<p>اضغط على الرابط التالي لإعادة تعيين كلمة المرور:</p>" +
                           $"<a href='{resetLink}'>إعادة تعيين كلمة المرور</a>" +
                           $"<p>إذا لم تطلب إعادة تعيين كلمة المرور، تجاهل هذا الإيميل.</p>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(
                        _configuration["EmailSettings:SmtpServer"],
                        int.Parse(_configuration["EmailSettings:Port"]),
                        false
                    );
                    await client.AuthenticateAsync(
                        _configuration["EmailSettings:Username"],
                        _configuration["EmailSettings:Password"]
                    );
                    await client.SendAsync(message);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}