using MediatR;
using Application.Interfaces;
using Application.Exceptions;
using Application.Addresses.Dtos;
using AutoMapper;

namespace Application.Addresses.Queries
{
    public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, AddressDto>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public GetAddressByIdQueryHandler(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<AddressDto> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var address = await _addressRepository.GetAddressByIdAsync(request.AddressId);

            if (address == null)
                throw new NotFoundException($"Address with ID {request.AddressId} not found.");

            if (!request.IsAdmin && address.UserId != request.CurrentUserId)
                throw new ForbiddenAccessException();

            return _mapper.Map<AddressDto>(address);
        }
    }
}
