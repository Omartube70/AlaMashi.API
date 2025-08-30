using MediatR;
using Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Application.Users.Commands
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService; // Or a dedicated token service
        private readonly IEmailService _emailService; // A new interface for sending emails

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IJwtService jwtService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            // لأسباب أمنية، لا نرسل خطأ إذا لم يتم العثور على المستخدم
            if (user != null)
            {
                // استخدم خدمة التوكن لإنشاء توكن قصير الأجل لإعادة التعيين
                var resetToken = _jwtService.GenerateAccessToken(user);
                var resetLink = $"https://your-app.com/reset-password?token={Uri.EscapeDataString(resetToken)}";

                await _emailService.SendPasswordResetEmailAsync(user.UserName, user.Email, resetLink);
            }
        }
    }
}
