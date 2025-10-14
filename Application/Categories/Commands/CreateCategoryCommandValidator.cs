using FluentValidation;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.Commands
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryName)
                    .NotEmpty().WithMessage("CategoryName is required.")
                    .NotNull()
                    .MaximumLength(200).WithMessage("CategoryName must not exceed 200 characters.");

            // ParentId Validation (Optional, but must be valid if provided)
            When(p => p.ParentId.HasValue, () =>
            {
                RuleFor(p => p.ParentId)
                    .GreaterThan(0).WithMessage("ParentId must be a valid ID.");
            });

            // ProductImageFile Validation
            RuleFor(p => p.IconName)
                .NotNull().WithMessage("IconName is required.");
        }
    
    }

}


