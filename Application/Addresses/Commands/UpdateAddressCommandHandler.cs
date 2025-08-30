using MediatR;
using Application.Interfaces;
using Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


namespace Application.Addresses.Commands
{

    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateAddressCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetUserWithAddressesAsync(request.CurrentUserId);
            if (user == null)
            {
                throw new UserNotFoundException(request.CurrentUserId);
            }

            var addressToUpdate = user.Addresses.FirstOrDefault(a => a.AddressId == request.AddressId);
            if (addressToUpdate == null)
            {
                throw new NotFoundException($"Address with ID {request.AddressId} not found for this user.");
            }

            addressToUpdate.Update(request.Street, request.City, request.AddressDetails, request.AddressType);

            await _userRepository.UpdateUserAsync(user);
        }
    }
}
