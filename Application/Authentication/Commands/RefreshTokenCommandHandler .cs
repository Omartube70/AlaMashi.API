using Application.Authentication.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        public RefreshTokenCommandHandler(IUserRepository userRepository, IJwtService jwtService , IRefreshTokenGenerator refreshTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // الخطوة 1: ابحث عن المستخدم المرتبط بهذا الـ Refresh Token
            // ستحتاج لإضافة دالة GetUserByRefreshTokenAsync في IUserRepository
            var user = await _userRepository.GetUserByRefreshTokenAsync(request.RefreshToken);

            // الخطوة 2: التحقق من صحة التوكن
            // (يمكن إضافة تحقق من تاريخ الصلاحية هنا أيضًا)
            if (user == null)
            {
                throw new InvalidTokenException("Invalid or expired refresh token.");
            }

            // الخطوة 3: إصدار توكنات جديدة
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _refreshTokenGenerator.Generate(); // استبدل بمنطق توليد التوكن

            // الخطوة 4: حفظ الـ Refresh Token الجديد في قاعدة البيانات
            await _userRepository.SaveRefreshTokenAsync(user.UserID, newRefreshToken.token, newRefreshToken.expiryTime);

            // الخطوة 5: إرجاع الرد
            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.token,
                User = new UserDto { UserId = user.UserID, UserName = user.UserName, Email = user.Email, Phone = user.Phone, UserRole = user.UserPermissions.ToString() }
            };
        }
    }
}
