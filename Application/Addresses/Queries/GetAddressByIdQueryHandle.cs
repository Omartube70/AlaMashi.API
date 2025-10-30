using MediatR;
using Application.Interfaces;
using Application.Exceptions;
using Application.Addresses.Dtos;

namespace Application.Addresses.Queries
{
    public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, AddressDto>
    {
        private readonly IAddressRepository _addressRepository;

        public GetAddressByIdQueryHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<AddressDto> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            // 🔍 جلب العنوان مباشرة
            var address = await _addressRepository.GetAddressByIdAsync(request.AddressId);

            if (address == null)
            {
                throw new NotFoundException($"Address with ID {request.AddressId} not found.");
            }

            // 🛡️ تحقق من الصلاحيات (إذا مش أدمن، لازم يكون مالك العنوان)
            if (!request.IsAdmin && address.UserId != request.CurrentUserId)
            {
                throw new ForbiddenAccessException();
            }

            return new AddressDto
            {
                AddressId = address.AddressId,
                Street = address.Street,
                City = address.City,
                AddressDetails = address.AddressDetails,
                AddressType = address.AddressType.ToString(),
                UserId = address.UserId
            };
        }
    }
}