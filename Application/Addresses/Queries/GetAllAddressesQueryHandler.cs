using MediatR;
using Application.Addresses.Dtos;
using Application.Interfaces;

namespace Application.Addresses.Queries
{
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, IReadOnlyList<AddressDto>>
    {
        private readonly IAddressRepository _addressRepository; // ✅ تغيير

        public GetAllAddressesQueryHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IReadOnlyList<AddressDto>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _addressRepository.GetAllAddressesAsync();

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