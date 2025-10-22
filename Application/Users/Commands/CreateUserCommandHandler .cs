using Application.Exceptions;
using Application.Interfaces;
using Application.Users.DTOs;
using Application.Users.Events;
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
        private readonly IMediator _mediator; 

        public CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IMediator mediator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mediator = mediator;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.IsEmailTakenAsync(request.Email))
            
                throw new EmailAlreadyExistsException(request.Email);


                if (await _userRepository.IsPhoneTakenAsync(request.Phone))
                
                    throw new PhoneAlreadyExistsException(request.Phone);

            

            string hashedPassword = _passwordHasher.HashPassword(request.Password);
            User newUser = await User.CreateAsync(request.UserName, request.Email, request.Phone, hashedPassword);

            await _userRepository.AddUserAsync(newUser);

            var notification = new UserCreatedNotification(newUser.Email, newUser.UserName);

            await _mediator.Publish(notification, cancellationToken);

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
