using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService; 

        public ResetPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // 1. التحقق من صحة التوكن واستخراج البيانات منه
            var principal = _jwtService.ValidateToken(request.Token); 
            if (principal == null)
            {
                throw new InvalidTokenException("Invalid or expired password reset token.");
            }

            // تأكد من أن التوكن مخصص لإعادة تعيين كلمة المرور
            var purpose = principal.FindFirst("purpose")?.Value;
            if (purpose != "PasswordReset")
            {
                throw new InvalidTokenException("Invalid token purpose.");
            }

            var userIdString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                throw new InvalidTokenException("Invalid user identifier in token.");
            }

            // 2. جلب المستخدم من قاعدة البيانات
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                // يمكن إرجاع خطأ عام هنا أيضًا لمزيد من الأمان
                throw new UserNotFoundException(userId);
            }

            // 3. تشفير كلمة المرور الجديدة
            var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);

            // 4. استدعاء دالة التحديث في الـ Domain Entity
            user.ChangePassword(newPasswordHash);

            // 5. حفظ التغييرات في قاعدة البيانات
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
