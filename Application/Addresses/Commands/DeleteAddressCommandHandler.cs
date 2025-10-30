using MediatR;
using Application.Interfaces;
using Application.Exceptions;

namespace Application.Addresses.Commands
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;

        public DeleteAddressCommandHandler(
            IUserRepository userRepository,
            IAddressRepository addressRepository)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }

        public async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            // 🔍 جلب العنوان مباشرة
            var addressToDelete = await _addressRepository.GetAddressByIdAsync(request.AddressId);

            if (addressToDelete == null)
            {
                throw new NotFoundException($"Address with ID {request.AddressId} not found.");
            }

            // 🛡️ تحقق من الصلاحيات (إذا مش أدمن، لازم يكون مالك العنوان)
            if (!request.IsAdmin && addressToDelete.UserId != request.CurrentUserId)
            {
                throw new ForbiddenAccessException();
            }

            // ❌ حذف العنوان
            await _addressRepository.DeleteAddressAsync(addressToDelete);

            return Unit.Value;
        }
    }
}