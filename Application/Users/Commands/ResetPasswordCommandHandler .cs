using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Users.Commands;
using Application.Interfaces;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand,Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordCommandHandler(IUserRepository userRepository ,IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception("Invalid email or OTP code.");
        }

        if (user.PasswordResetOtp != request.Otp)
        {
            throw new Exception("Invalid email or OTP code.");
        }

        if (user.OtpExpiryTime <= DateTime.UtcNow)
        {
            throw new Exception("OTP code has expired. Please request a new one.");
        }

        string NewPasswordhash = _passwordHasher.HashPassword(request.NewPassword);

        user.ChangePassword(NewPasswordhash);

        user.RemovePasswordResetOtp();

        await _userRepository.UpdateUserAsync(user);

        return Unit.Value;
    }
}