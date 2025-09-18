using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            // ProductID Validation
            RuleFor(p => p.ProductID)
                .GreaterThan(0).WithMessage("Product ID is required and must be a valid ID.");

            // ProductName Validation
            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Product Name is required.")
                .NotNull()
                .MaximumLength(200).WithMessage("Product Name must not exceed 200 characters.");

            // ProductDescription Validation (Optional, but has a max length if provided)
            RuleFor(p => p.ProductDescription)
                .MaximumLength(500).WithMessage("Product Description must not exceed 500 characters.");

            // Price Validation
            RuleFor(p => p.Price)
               .GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}
