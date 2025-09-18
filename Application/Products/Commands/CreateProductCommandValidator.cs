using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FluentValidation;

namespace Application.Products.Commands
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            // ProductName Validation
            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Product Name is required.")
                .NotNull()
                .MaximumLength(200).WithMessage("Product Name must not exceed 200 characters.");

            // Barcode Validation
            RuleFor(p => p.Barcode)
                .NotEmpty().WithMessage("Barcode is required.")
                .NotNull()
                .MaximumLength(450).WithMessage("Barcode must not exceed 450 characters.");

            // ProductDescription Validation (Optional, but has a max length if provided)
            RuleFor(p => p.ProductDescription)
                .MaximumLength(500).WithMessage("Product Description must not exceed 500 characters.");

            // Price Validation
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            // QuantityInStock Validation
            RuleFor(p => p.QuantityInStock)
                .GreaterThan(0).WithMessage("QuantityInStock must be greater than zero.");

            // CategoryID Validation
            RuleFor(p => p.CategoryID)
                .GreaterThan(0).WithMessage("CategoryID must be a valid ID.");

            // ProductImageFile Validation
            RuleFor(p => p.ProductImageFile)
                .NotNull().WithMessage("Product image is required.");

            // Add more specific rules for the file when it's not null
            When(p => p.ProductImageFile != null, () =>
            {
                // File Size Validation (e.g., max 2 MB)
                RuleFor(p => p.ProductImageFile.Length)
                    .LessThanOrEqualTo(2 * 1024 * 1024) // 2 MB in bytes
                    .WithMessage("Image size must not exceed 2 MB.");

                // File Type Validation (e.g., only .jpg and .png)
                RuleFor(p => p.ProductImageFile.ContentType)
                    .Must(x => x.Equals("image/jpeg") || x.Equals("image/png") || x.Equals("image/jpg"))
                    .WithMessage("Only .jpg and .png image types are allowed.");
            });
        }
    }
}

