using MediatR;
using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Application.Users.Events
{
    public class SendWelcomeEmailHandler : INotificationHandler<UserCreatedNotification>
    {
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;

        public SendWelcomeEmailHandler(IEmailService emailService, IWebHostEnvironment env)
        {
            _emailService = emailService;
            _env = env;
        }

        public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
        {
            var templatePath = Path.Combine(_env.ContentRootPath, "EmailTemplates", "WelcomeEmail.html");

            var templateText = await File.ReadAllTextAsync(templatePath, cancellationToken);

            var emailBody = templateText.Replace("{{username}}", notification.UserName);

            var subject = $"أهلاً بك يا {notification.UserName} في AlaMashi!";

            await _emailService.SendEmailAsync(notification.UserEmail, subject, emailBody);
        }
    }
}