using Application.Exceptions;
using Application.Interfaces;
using Application.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // 1. جلب كل المستخدمين من قاعدة البيانات عبر الواجهة
            var users = await _userRepository.GetAllUsersAsync();

            // 2. تحويل قائمة الـ User Entities إلى قائمة UserDto
            var usersDto = users.Select(user => new UserDto
            {
                UserId = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                UserRole = user.UserPermissions.ToString()
            });

            return usersDto;
        }
    }
}
