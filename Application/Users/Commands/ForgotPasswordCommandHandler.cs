using MediatR;
using Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;

namespace Application.Users.Commands
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService, IWebHostEnvironment env)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _env = env;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user != null)
            {
                // 1. إنشاء وتخزين الـ OTP
                var random = new Random();
                var otp = random.Next(100000, 999999).ToString();

                user.SetPasswordResetOtp(otp, DateTime.UtcNow.AddMinutes(10));
                await _userRepository.UpdateUserAsync(user);

                var templatePath = Path.Combine(_env.ContentRootPath, "EmailTemplates", "PasswordReset.html");
                var templateText = await File.ReadAllTextAsync(templatePath, cancellationToken);

                var emailBody = templateText.Replace("{{username}}", user.UserName)
                    .Replace("{{otpCode}}", otp);

                var subject = "كود إعادة تعيين كلمة المرور";

                await _emailService.SendEmailAsync(user.Email, subject, emailBody);

            }
        }
    }
}
