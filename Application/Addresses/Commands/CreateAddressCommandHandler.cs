using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Application.Addresses.Dtos;
using Application.Exceptions;
using AutoMapper;

namespace Application.Addresses.Commands
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, AddressDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateAddressCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<AddressDto> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.CurrentUserId);

            if (user == null)
                throw new UserNotFoundException(request.CurrentUserId);

            var newAddress = Address.Create(
                request.Street,
                request.City,
                request.AddressDetails,
                request.AddressType,
                request.CurrentUserId
            );

            user.AddAddress(newAddress);
            await _userRepository.UpdateUserAsync(user);

            return _mapper.Map<AddressDto>(newAddress);
        }
    }
}
