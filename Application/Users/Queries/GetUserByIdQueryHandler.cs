using Application.Exceptions;
using Application.Interfaces;
using Application.Offers.Dtos;
using Application.Users.DTOs;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUserRepository userRepository , IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            bool isAdmin = request.CurrentUserRole == "Admin";
            bool isDeletingSelf = request.CurrentUserId == request.UserId;

            // إذا كان المستخدم ليس أدمن وليس هو نفسه الشخص المراد حذفه، امنعه.
            if (!isAdmin && !isDeletingSelf)
            {
                // 🚫 إلقاء استثناء يدل على عدم وجود صلاحية
                throw new ForbiddenAccessException("You are not authorized to view this user.");
            }

            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }

           return _mapper.Map<UserDto>(user);
        }
    }
}
