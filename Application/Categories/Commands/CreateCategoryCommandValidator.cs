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
            RuleFor(p => p.CategoryImageFile)
                .NotNull().WithMessage("Product image is required.");

            // Add more specific rules for the file when it's not null
            When(p => p.CategoryImageFile != null, () =>
            {
                // File Size Validation (e.g., max 2 MB)
                RuleFor(p => p.CategoryImageFile.Length)
                    .LessThanOrEqualTo(2 * 1024 * 1024) // 2 MB in bytes
                    .WithMessage("Image size must not exceed 2 MB.");

                // File Type Validation (e.g., only .jpg and .png)
                RuleFor(p => p.CategoryImageFile.ContentType)
                    .Must(x => x.Equals("image/jpeg") || x.Equals("image/png") || x.Equals("image/jpg"))
                    .WithMessage("Only .jpg and .png image types are allowed.");

                RuleFor(p => p.CategoryImageFile)
                    .Must(BeAValidResolution)
                    .WithMessage("Image resolution is too low. Please upload an image with at least 800x600 resolution.");
            });

        }
        private bool BeAValidResolution(IFormFile file)
        {
            if (file == null)
                return true;

            try
            {
                using (var image = Image.Load(file.OpenReadStream()))
                {
                    return image.Width >= 800 && image.Height >= 600;
                }
            }
            catch
            {
                return false;
            }
        }
    
    }

}


