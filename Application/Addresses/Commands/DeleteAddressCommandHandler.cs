using MediatR;
using Application.Interfaces;
using Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Addresses.Commands
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteAddressCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserWithAddressesAsync(request.CurrentUserId);
            if (user == null)
            {
                throw new UserNotFoundException(request.CurrentUserId);
            }

            var addressToDelete = user.Addresses.FirstOrDefault(a => a.AddressId == request.AddressId);
            if (addressToDelete == null)
            {
                throw new NotFoundException($"Address with ID {request.AddressId} not found for this user.");
            }

            user.Addresses.Remove(addressToDelete);

            await _userRepository.UpdateUserAsync(user);
        }
    }
}
