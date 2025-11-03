using MediatR;
using Application.Addresses.Dtos;
using Application.Interfaces;
using AutoMapper;

namespace Application.Addresses.Queries
{
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, IReadOnlyList<AddressDto>>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public GetAllAddressesQueryHandler(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<AddressDto>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _addressRepository.GetAllAddressesAsync();

            return _mapper.Map<IReadOnlyList<AddressDto>>(addresses);
        }
    }
}
