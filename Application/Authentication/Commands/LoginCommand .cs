using Application.Authentication.Dtos;
using MediatR;

namespace Application.Authentication.Commands
{

    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
