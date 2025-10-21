using MediatR;
using Application.Interfaces;
using Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class PromoteUserToAdminCommandHandler : IRequestHandler<PromoteUserToAdminCommand,Unit>
    {
        private readonly IUserRepository _userRepository;

        public PromoteUserToAdminCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(PromoteUserToAdminCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب المستخدم
            var userToPromote = await _userRepository.GetUserByIdAsync(request.UserId);
            if (userToPromote == null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            // 2. استدعاء دالة الترقية في الـ Domain
            userToPromote.PromoteToAdmin();

            // 3. حفظ التغييرات
            await _userRepository.UpdateUserAsync(userToPromote);

            return Unit.Value;
        }
    }
}
