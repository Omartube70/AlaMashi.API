using Application.Exceptions;
using Application.Interfaces;
using Application.Offers.Dtos;
using Application.Users.DTOs;
using AutoMapper;
using Domain.Entities;
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
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUserRepository userRepository , IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // 1. جلب كل المستخدمين من قاعدة البيانات عبر الواجهة
            var users = await _userRepository.GetAllUsersAsync();

            // 2. تحويل قائمة الـ User Entities إلى قائمة UserDto
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
