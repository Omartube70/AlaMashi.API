using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;

            public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // 1. التحقق من الصلاحيات (Authorization)
            bool isAdmin = request.CurrentUserRole == "Admin";
            bool isUpdatingSelf = request.CurrentUserId == request.TargetUserId;

            if (!isAdmin && !isUpdatingSelf)
            {
                throw new ForbiddenAccessException();
            }

            // 2. جلب المستخدم من قاعدة البيانات
            var userToUpdate = await _userRepository.GetUserByIdAsync(request.TargetUserId);
            if (userToUpdate == null)
            {
                throw new UserNotFoundException(request.TargetUserId); 
            }

            // 3. التحقق من أن الإيميل الجديد غير مستخدم من قبل شخص آخر
            var userWithNewEmail = await _userRepository.GetUserByEmailAsync(request.Email);
            if (userWithNewEmail != null && userWithNewEmail.UserID != request.TargetUserId)
            {
                throw new EmailAlreadyExistsException(request.Email); 
            }

            // 3. التحقق من أن رقم الهاتف الجديد غير مستخدم من قبل شخص آخر
            var userWithNewPhone = await _userRepository.GetUserByPhoneAsync(request.Phone);
            if (userWithNewPhone != null && userWithNewPhone.UserID != request.TargetUserId)
            {
                throw new PhoneAlreadyExistsException(request.Phone);
            }

            // 4. استدعاء دالة التحديث في الـ Domain Entity
            await userToUpdate.UpdateProfileAsync(request.UserName, request.Email, request.Phone);

            // 5. حفظ التغييرات في قاعدة البيانات
            await _userRepository.UpdateUserAsync(userToUpdate);
        }
    }
}
