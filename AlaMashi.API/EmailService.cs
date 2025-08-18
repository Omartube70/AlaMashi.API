using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Microsoft.AspNetCore.Hosting; // ## إضافة مهمة

public class EmailService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env; // ## إضافة مهمة

    // عدّل الـ Constructor
    public EmailService(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env; // ## إضافة مهمة
    }

    public async Task SendPasswordResetEmailAsync(string username, string email, string resetLink)
    {
        // 1. تحديد مسار ملف القالب
        var templatePath = Path.Combine(_env.ContentRootPath, "EmailTemplates", "PasswordReset.html");

        // 2. قراءة محتوى القالب
        var templateText = await File.ReadAllTextAsync(templatePath);

        // 3. استبدال المتغيرات بالقيم الفعلية
        var emailBody = templateText.Replace("{{resetLink}}", resetLink);
        emailBody = emailBody.Replace("{{username}}", username);

        // 4. بناء الرسالة (باقي الكود كما هو)
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("AlaMashi", _configuration["EmailSettings:SenderEmail"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "إعادة تعيين كلمة المرور";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = emailBody // ## هنا نستخدم القالب الجديد
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