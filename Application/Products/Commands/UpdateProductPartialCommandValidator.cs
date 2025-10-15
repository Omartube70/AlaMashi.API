using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class UpdateProductPartialCommandValidator : AbstractValidator<UpdateProductPartialCommand>
    {
        public UpdateProductPartialCommandValidator()
        {
            // ✅ تأكيد إن ID المنتج صحيح
            RuleFor(p => p.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be valid.");

            // ✅ تأكيد إن PatchDoc موجود
            RuleFor(p => p.PatchDoc)
                .NotNull()
                .WithMessage("Patch document is required.");

            // ✅ التحقق من العمليات داخل الـ Patch
            RuleForEach(p => p.PatchDoc.Operations).ChildRules(operation =>
            {
                operation.RuleFor(o => o.path)
                    .NotEmpty()
                    .WithMessage("Patch operation path cannot be empty.");

                operation.RuleFor(o => o.op)
                    .Must(op => op == "replace" || op == "add" || op == "remove")
                    .WithMessage("Patch operation must be one of: replace, add, remove.");
            });

            // ✅ Validation إضافي لو عايز تتأكد إن الحقول اللي بيتم تعديلها منطقية
            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("price") && (op.value == null || Convert.ToDecimal(op.value) <= 0)))
                .WithMessage("Product price must be greater than 0 when updated.");

            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("productName") && string.IsNullOrWhiteSpace(op.value?.ToString())))
                .WithMessage("Product name cannot be empty when updated.");
        }
    }
}
