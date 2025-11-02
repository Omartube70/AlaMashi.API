using FluentValidation;
using SixLabors.ImageSharp;

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

            // ProductDescription Validation (Optional)
            RuleFor(p => p.ProductDescription)
                .MaximumLength(500).WithMessage("Product Description must not exceed 500 characters.");

            // Price Validation
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            // QuantityInStock Validation
            RuleFor(p => p.QuantityInStock)
                .GreaterThanOrEqualTo(0).WithMessage("QuantityInStock cannot be negative.");

            // CategoryID Validation
            RuleFor(p => p.CategoryID)
                .GreaterThan(0).WithMessage("CategoryID must be a valid ID.");

            // ProductImageFile Validation
            RuleFor(p => p.ProductImageFile)
                .NotNull().WithMessage("Product image is required.");

            // Image File Rules
            When(p => p.ProductImageFile != null, () =>
            {
                // File Size Validation (max 5 MB)
                RuleFor(p => p.ProductImageFile.Length)
                    .LessThanOrEqualTo(5 * 1024 * 1024)
                    .WithMessage("Image size must not exceed 5 MB.");

                // File Type Validation
                RuleFor(p => p.ProductImageFile.ContentType)
                    .Must(x => x.Equals("image/jpeg") || x.Equals("image/png") || x.Equals("image/jpg") || x.Equals("image/webp"))
                    .WithMessage("Only .jpg, .jpeg, .png, and .webp image types are allowed.");

                // Image Dimensions Validation
                RuleFor(p => p.ProductImageFile)
                    .MustAsync(async (file, cancellation) =>
                    {
                        if (file == null) return true;

                        try
                        {
                            using var stream = file.OpenReadStream();
                            using var image = await Image.LoadAsync(stream, cancellation);

                            // Minimum dimensions: 300x300
                            // Recommended: 600x600 to 1200x1200 for best quality
                            // Maximum: 2000x2000 to avoid huge files
                            return image.Width >= 300 && image.Height >= 300
                                   && image.Width <= 2000 && image.Height <= 2000;
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .WithMessage("Image dimensions must be between 300x300 and 2000x2000 pixels. Recommended: 600x600 to 1200x1200 for optimal display.");

                // Aspect Ratio Validation (prefer square images)
                RuleFor(p => p.ProductImageFile)
                    .MustAsync(async (file, cancellation) =>
                    {
                        if (file == null) return true;

                        try
                        {
                            using var stream = file.OpenReadStream();
                            using var image = await Image.LoadAsync(stream, cancellation);

                            // Accept aspect ratios from 0.8 to 1.25 (nearly square)
                            // Perfect square = 1.0
                            double aspectRatio = (double)image.Width / image.Height;
                            return aspectRatio >= 0.8 && aspectRatio <= 1.25;
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .WithMessage("Image should have a square or near-square aspect ratio for best display. Recommended: 1:1 (e.g., 800x800).");
            });
        }
    }
}