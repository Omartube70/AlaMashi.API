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
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.TargetUserId);
            if (user == null)
            {
                throw new UserNotFoundException(request.TargetUserId);
            }


            bool isAdmin = request.CurrentUserRole == "Admin";
            bool isDeletingSelf = request.CurrentUserId == request.TargetUserId;

            // إذا كان المستخدم ليس أدمن وليس هو نفسه الشخص المراد حذفه، امنعه.
            if (!isAdmin && !isDeletingSelf)
            {
                // 🚫 إلقاء استثناء يدل على عدم وجود صلاحية
                throw new ForbiddenAccessException("You are not authorized to delete this user.");
            }


            await _userRepository.DeleteUserAsync(request.TargetUserId);
        }
    }
}
