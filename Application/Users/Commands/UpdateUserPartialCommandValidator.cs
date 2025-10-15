using Application.Users.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class UpdateUserPartialCommandValidator : AbstractValidator<UpdateUserPartialCommand>
    {
        public UpdateUserPartialCommandValidator()
        {
            // ✅ CurrentUserId
            RuleFor(p => p.CurrentUserId)
                .GreaterThan(0)
                .WithMessage("Current User ID must be valid.");

            // ✅ TargetUserId
            RuleFor(p => p.TargetUserId)
                .GreaterThan(0)
                .WithMessage("Target User ID must be valid.");

            // ✅ PatchDoc موجود
            RuleFor(p => p.PatchDoc)
                .NotNull()
                .WithMessage("Patch document is required.");

            // ✅ التأكد من إن العمليات داخل الـ Patch صالحة
            RuleForEach(p => p.PatchDoc.Operations).ChildRules(operation =>
            {
                operation.RuleFor(o => o.path)
                    .NotEmpty()
                    .WithMessage("Patch operation path cannot be empty.");

                operation.RuleFor(o => o.op)
                    .Must(op => op == "replace" || op == "add" || op == "remove")
                    .WithMessage("Patch operation must be one of: replace, add, remove.");
            });

            // ✅ تحقق من UserName لو تم تغييره
            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("userName") &&
                    string.IsNullOrWhiteSpace(op.value?.ToString())))
                .WithMessage("UserName cannot be empty when updated.");

            // ✅ تحقق من Email لو تم تغييره
            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("email") &&
                    !IsValidEmail(op.value?.ToString())))
                .WithMessage("Invalid email address format.");

            // ✅ تحقق من Phone لو تم تغييره
            RuleFor(p => p.PatchDoc)
                .Must(doc => !doc.Operations.Any(op =>
                    op.path.Contains("phone") &&
                    !System.Text.RegularExpressions.Regex.IsMatch(
                        op.value?.ToString() ?? "", @"^(010|011|012|015)\d{8}$")))
                .WithMessage("Invalid Egyptian phone number format.");
        }

        private bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
