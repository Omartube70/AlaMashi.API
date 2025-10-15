using Application.Offers.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Commands
{
    public class UpdateOfferPartialCommandValidator : AbstractValidator<UpdateOfferPartialCommand>
    {
        public UpdateOfferPartialCommandValidator()
        {
            // ✅ Offer ID check
            RuleFor(p => p.OfferId)
                .GreaterThan(0)
                .WithMessage("Offer ID must be greater than 0.");

            // ✅ Patch document presence
            RuleFor(p => p.PatchDoc)
                .NotNull()
                .WithMessage("Patch document is required.");

            // ✅ Validate each operation in the patch
            RuleForEach(p => p.PatchDoc.Operations).ChildRules(operation =>
            {
                operation.RuleFor(o => o.path)
                    .NotEmpty().WithMessage("Patch operation path cannot be empty.");

                operation.RuleFor(o => o.op)
                    .Must(op => op == "replace" || op == "add" || op == "remove")
                    .WithMessage("Patch operation must be one of: replace, add, remove.");
            });

            // ✅ Title validation if being updated
            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("title") &&
                    string.IsNullOrWhiteSpace(op.value?.ToString())))
                .WithMessage("Title cannot be empty when updated.");

            // ✅ Description max length
            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("description") &&
                    (op.value?.ToString()?.Length > 500)))
                .WithMessage("Description must not exceed 500 characters.");

            // ✅ Discount percentage range check
            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("discountPercentage") &&
                    (!decimal.TryParse(op.value?.ToString(), out var val) || val < 0 || val > 100)))
                .WithMessage("Discount percentage must be between 0 and 100.");

            // ✅ StartDate / EndDate validation if updated
            RuleFor(p => p.PatchDoc)
                .Must(doc =>
                {
                    var start = doc.Operations.FirstOrDefault(op => op.path.Contains("startDate"))?.value?.ToString();
                    var end = doc.Operations.FirstOrDefault(op => op.path.Contains("endDate"))?.value?.ToString();

                    if (start == null || end == null) return true; // not both updated

                    if (DateTime.TryParse(start, out var s) && DateTime.TryParse(end, out var e))
                        return e > s;

                    return false;
                })
                .WithMessage("End date must be after start date.");
        }
    }
}
