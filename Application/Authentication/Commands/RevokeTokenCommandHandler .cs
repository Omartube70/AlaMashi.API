using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
    {
        private readonly IUserRepository _userRepository;

        public RevokeTokenCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            // الخطوة 1: التحقق من الصلاحيات (Authorization)
            // القاعدة: "إما أن تكون أدمن، أو أنك تلغي التوكن الخاص بك"
            bool isAdmin = request.CurrentUserRole == "Admin";
            bool isRevokingSelf = request.CurrentUserId == request.TargetUserId;


            if (!isAdmin && !isRevokingSelf)
            {
                // 🚫 إذا لم يتحقق الشرط، امنعه
                throw new ForbiddenAccessException();
            }

            // الخطوة 2: إذا كان مسموحًا له، استمر في تنفيذ المنطق الأصلي
            await _userRepository.SaveRefreshTokenAsync(request.TargetUserId, null,null);
        }
    }
}
