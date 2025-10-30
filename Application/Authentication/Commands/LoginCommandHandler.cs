using Application.Authentication.Commands;
using Application.Authentication.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Users.DTOs;
using MediatR;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;


        public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService , IRefreshTokenGenerator refreshTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new InvalidCredentialsException();
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _refreshTokenGenerator.Generate();

            await _userRepository.SaveRefreshTokenAsync(user.UserID, refreshToken.token, refreshToken.expiryTime);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.token,
                User = new UserDto { UserId = user.UserID, UserName = user.UserName, Email = user.Email, Phone = user.Phone ,UserRole = user.UserPermissions.ToString() }
            };
        }
    }
}
