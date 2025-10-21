using Application.Exceptions;
using Application.Interfaces;
using Application.Users.Dtos;
using Application.Users.DTOs;
using AutoMapper;
using FluentValidation; 
using MediatR;

namespace Application.Users.Commands
{
    public class UpdateUserPartialCommandHandler : IRequestHandler<UpdateUserPartialCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUserPartialCommand> _dtoValidator;

        public UpdateUserPartialCommandHandler(IUserRepository userRepository, IMapper mapper, IValidator<UpdateUserPartialCommand> dtoValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _dtoValidator = dtoValidator; 
        }

        public async Task<UserDto> Handle(UpdateUserPartialCommand request, CancellationToken cancellationToken)
        {
            // 1. التحقق من الصلاحيات (Authorization)
            if (request.CurrentUserRole != "Admin" && request.CurrentUserId != request.TargetUserId)
            {
                throw new ForbiddenAccessException();
            }

            // 2. جلب المستخدم من قاعدة البيانات
            var userEntity = await _userRepository.GetUserByIdAsync(request.TargetUserId);
            if (userEntity == null)
            {
                throw new UserNotFoundException(request.TargetUserId);
            }

            // 3. تحويل الـ Entity إلى DTO لتطبيق التعديلات عليه
            var userToPatch = _mapper.Map<UpdateUserDto>(userEntity);

            // ✅ حفظ القيم القديمة قبل تطبيق الـ Patch
            var originalEmail = userToPatch.Email;
            var originalPhone = userToPatch.Phone;

            // 4. تطبيق التعديلات من الـ Patch Document
            request.PatchDoc.ApplyTo(userToPatch);

            // ✅  التحقق من صحة البيانات بعد تطبيق الـ Patch
            var validationResult = await _dtoValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // ✅  التحقق المشروط من الإيميل
            if (userToPatch.Email != originalEmail)
            {
                var userWithNewEmail = await _userRepository.GetUserByEmailAsync(userToPatch.Email);
                if (userWithNewEmail != null && userWithNewEmail.UserID != request.TargetUserId)
                {
                    throw new EmailAlreadyExistsException(userToPatch.Email);
                }
            }

            // ✅ التحقق المشروط من رقم الهاتف
            if (userToPatch.Phone != originalPhone)
            {
                var userWithNewPhone = await _userRepository.GetUserByPhoneAsync(userToPatch.Phone);
                if (userWithNewPhone != null && userWithNewPhone.UserID != request.TargetUserId)
                {
                    throw new PhoneAlreadyExistsException(userToPatch.Phone);
                }
            }

            // 5. تحديث الـ Entity الأصلية بالبيانات الجديدة من الـ DTO
            _mapper.Map(userToPatch, userEntity);

            // 6. حفظ التغييرات في قاعدة البيانات
            await _userRepository.UpdateUserAsync(userEntity);

            // 7. إرجاع الـ DTO النهائي
            return _mapper.Map<UserDto>(userEntity);
        }
    }
}