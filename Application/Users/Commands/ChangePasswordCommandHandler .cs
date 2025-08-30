using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ChangePasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب المستخدم الحالي من قاعدة البيانات
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            // 2. التحقق من أن كلمة المرور القديمة صحيحة
            if (!_passwordHasher.VerifyPassword(request.OldPassword, user.PasswordHash))
            {
                throw new InvalidCredentialException("The old password you entered is incorrect.");
            }

            // 3. تشفير كلمة المرور الجديدة
            var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);

            // 4. استدعاء الدالة في الـ Domain لتغيير كلمة المرور
            user.ChangePassword(newPasswordHash);

            // 5. حفظ التغييرات
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
