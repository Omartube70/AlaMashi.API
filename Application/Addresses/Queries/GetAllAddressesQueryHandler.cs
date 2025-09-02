using MediatR;
using Application.Addresses.Dtos;
using Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Addresses.Queries
{
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, IReadOnlyList<AddressDto>>
    {
        private readonly IUserRepository _userRepository; // أو IAddressRepository

        public GetAllAddressesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IReadOnlyList<AddressDto>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _userRepository.GetAllAddressesAsync();

            return addresses.Select(address => new AddressDto
            {
                AddressId = address.AddressId,
                Street = address.Street,
                City = address.City,
                AddressDetails = address.AddressDetails,
                AddressType = address.AddressType.ToString(),
                UserId = address.UserId
            }).ToList();
        }
    }
}