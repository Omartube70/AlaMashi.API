using MediatR;
using Application.Interfaces;
using Application.Exceptions;
using Application.Addresses.Dtos;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Addresses.Queries
{
    public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, AddressDto>
    {
        private readonly IUserRepository _userRepository;

        public GetAddressByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AddressDto> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserWithAddressesAsync(request.CurrentUserId);
            if (user == null) 
            {
                throw new UserNotFoundException(request.CurrentUserId);
            }

            var address = user.Addresses.FirstOrDefault(a => a.AddressId == request.AddressId);
            if (address == null) 
            { 
                throw new NotFoundException($"Address with ID {request.AddressId} not found for this user."); 
            }

            return new AddressDto
            {
                AddressId = address.AddressId,
                Street = address.Street,
                City = address.City,
                AddressDetails = address.AddressDetails,
                AddressType = address.AddressType.ToString()
            };
        }
    }
}
