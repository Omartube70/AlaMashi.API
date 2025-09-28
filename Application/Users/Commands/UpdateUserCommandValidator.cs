using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            // --- Validation for Target and Authorization Data ---

            RuleFor(p => p.TargetUserId)
                .GreaterThan(0).WithMessage("Target User ID is required.");

            RuleFor(p => p.CurrentUserId)
                .GreaterThan(0).WithMessage("Current User ID is required for authorization.");

            RuleFor(p => p.CurrentUserRole)
                .NotEmpty().WithMessage("Current User Role is required for authorization.");

            // --- Validation for New Data ---

            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .MaximumLength(100).WithMessage("UserName must not exceed 100 characters.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            // Phone Validation ( but validates if provided)
            RuleFor(p => p.Phone)
                .NotEmpty().WithMessage("phone number is required.")
                .Matches(@"^(010|011|012|015)\d{8}$")
                .WithMessage("Please enter a valid Egyptian phone number.");

        }
    }
}
