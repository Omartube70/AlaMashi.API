using MediatR;
using Application.Interfaces;
using Application.Exceptions;
using Application.Addresses.Dtos;
using AutoMapper;

namespace Application.Addresses.Queries
{
    public class GetUserAddressesQueryHandler : IRequestHandler<GetUserAddressesQuery, IEnumerable<AddressDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserAddressesQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AddressDto>> Handle(GetUserAddressesQuery request, CancellationToken cancellationToken)
        {
            bool isAdmin = request.CurrentUserRole == "Admin";
            bool isFetchingSelf = request.CurrentUserId == request.UserId;

            if (!isAdmin && !isFetchingSelf)
                throw new ForbiddenAccessException();

            var user = await _userRepository.GetUserWithAddressesAsync(request.UserId);
            if (user == null)
                throw new UserNotFoundException(request.UserId);

            return _mapper.Map<IEnumerable<AddressDto>>(user.Addresses);
        }
    }
}
