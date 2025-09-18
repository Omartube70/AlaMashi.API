using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Addresses.Commands
{
    public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressCommandValidator()
        {
            // CurrentUserId Validation for authorization
            RuleFor(p => p.CurrentUserId)
                .GreaterThan(0).WithMessage("Current User ID must be valid.");

            // AddressId Validation to identify the target address
            RuleFor(p => p.AddressId)
                .GreaterThan(0).WithMessage("Address ID must be valid.");

            // Street Validation
            RuleFor(p => p.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(200).WithMessage("Street must not exceed 200 characters.");

            // City Validation
            RuleFor(p => p.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

            // AddressDetails Validation (Optional)
            RuleFor(p => p.AddressDetails)
                .MaximumLength(500).WithMessage("AddressDetails must not exceed 500 characters.");

            // AddressType Validation (Enum)
            RuleFor(p => p.AddressType)
              .IsInEnum().WithMessage("A valid AddressType must be specified (e.g., Home or Work or Another).");
        }
    }
}

