using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Addresses.Commands
{
    public class UpdateAddressPartialCommandValidator : AbstractValidator<UpdateAddressPartialCommand>
    {
        public UpdateAddressPartialCommandValidator()
        {
            // ✅ CurrentUserId Validation
            RuleFor(p => p.CurrentUserId)
                .GreaterThan(0)
                .WithMessage("Current User ID must be valid.");

            // ✅ AddressId Validation
            RuleFor(p => p.AddressId)
                .GreaterThan(0)
                .WithMessage("Address ID must be valid.");

            // ✅ Ensure PatchDoc is not null
            RuleFor(p => p.PatchDoc)
                .NotNull()
                .WithMessage("Patch document is required.");

            // ✅ Validate Patch Operations — optional but helpful for debugging
            RuleForEach(p => p.PatchDoc.Operations).ChildRules(operation =>
            {
                operation.RuleFor(o => o.path)
                    .NotEmpty().WithMessage("Patch operation path cannot be empty.");

                operation.RuleFor(o => o.op)
                    .Must(op => op == "replace" || op == "add" || op == "remove")
                    .WithMessage("Patch operation must be one of: replace, add, remove.");
            });

            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("street") && string.IsNullOrWhiteSpace(op.value?.ToString())))
                .WithMessage("Street cannot be empty when updated.");

            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("city") && string.IsNullOrWhiteSpace(op.value?.ToString())))
                .WithMessage("City cannot be empty when updated.");

            // ✅ تحقق من AddressType لو تم تحديثه
            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("addressType") &&
                    !Enum.TryParse(typeof(Domain.Common.AddressType), op.value?.ToString(), out _)))
                .WithMessage("Invalid AddressType value. Must be one of: Home, Work, or Another.");
        }
    }
}