using FluentValidation;
using Application.Users.Commands;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        // Email Validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        // OTP (One-Time Password) Validation
        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("The OTP code is required.")
            .Length(6).WithMessage("The OTP code must be 6 digits.")
            .Matches("^[0-9]*$").WithMessage("The OTP code must contain only digits.");

        // NewPassword Validation
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("New password must contain at least one special character.");
    }
}