using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(p => p.UserId)
               .GreaterThan(0).WithMessage("UserId is required.");

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New Password is required.")
                .NotEqual(x => x.OldPassword).WithMessage("New password must be different from the old password.")
                .MinimumLength(8).WithMessage("New Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("New Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("New Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("New Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("New password must contain at least one special character.");
        }
    }
}
