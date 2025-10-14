using MediatR;
using Application.Interfaces;
using Application.Exceptions;
using Application.Addresses.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Addresses.Queries
{
    public class GetUserAddressesQueryHandler : IRequestHandler<GetUserAddressesQuery, IEnumerable<AddressDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserAddressesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
         
        public async Task<IEnumerable<AddressDto>> Handle(GetUserAddressesQuery request, CancellationToken cancellationToken)
        {
            bool isAdmin = request.CurrentUserRole == "Admin";
            bool isFetchingSelf = request.CurrentUserId == request.UserId;

            if (!isAdmin && !isFetchingSelf)
            {
                throw new ForbiddenAccessException();
            }

            var user = await _userRepository.GetUserWithAddressesAsync(request.UserId); 
            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            return user.Addresses.Select(a => new AddressDto
            {
                AddressId = a.AddressId,
                Street = a.Street,
                City = a.City,
                AddressDetails = a.AddressDetails,
                AddressType = a.AddressType.ToString(),
                UserId = a.UserId
            });
        }
    }
}
