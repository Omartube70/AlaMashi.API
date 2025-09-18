using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Application.Addresses.Commands
{
    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressCommandValidator() 
        {
            RuleFor(p => p.CurrentUserId)
                   .GreaterThan(0).WithMessage("User ID must be valid.");

            RuleFor(x => x.Street)
                   .NotEmpty().WithMessage("Street is required.")
                   .Length(3, 200).WithMessage("Street must be between 3 and 200 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

            RuleFor(x => x.AddressDetails)
                 .MaximumLength(500).WithMessage("Address Details must not exceed 500 characters.");

            RuleFor(p => p.AddressType)
              .IsInEnum().WithMessage("A valid AddressType must be specified (e.g., Home or Work or Another).");
        }
    }
}
