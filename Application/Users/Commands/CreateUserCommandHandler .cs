//using Application.Exceptions;
using Application.Exceptions;
using Application.Interfaces;
using Application.Users.DTOs;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{


    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.IsEmailTakenAsync(request.Email))
            {
                throw new EmailAlreadyExistsException(request.Email);
            }



            string hashedPassword = _passwordHasher.HashPassword(request.Password);
            User newUser = await User.CreateAsync(request.UserName, request.Email, request.Phone, hashedPassword);

            await _userRepository.AddUserAsync(newUser);

            return new UserDto
            {
                UserId = newUser.UserID,
                UserName = newUser.UserName,
                Email = newUser.Email,
                Phone = newUser.Phone
            };
        }
    }
}
