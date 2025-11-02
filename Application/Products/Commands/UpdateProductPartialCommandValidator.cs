using FluentValidation;
using SixLabors.ImageSharp;

namespace Application.Products.Commands
{
    public class UpdateProductPartialCommandValidator : AbstractValidator<UpdateProductPartialCommand>
    {
        public UpdateProductPartialCommandValidator()
        {
            // Product ID Validation
            RuleFor(p => p.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be valid.");

            // Product Name Validation (Optional)
            When(p => !string.IsNullOrEmpty(p.ProductName), () =>
            {
                RuleFor(p => p.ProductName)
                    .MaximumLength(100)
                    .WithMessage("Product Name must not exceed 100 characters.");
            });

            // Product Description Validation (Optional)
            When(p => p.ProductDescription != null, () =>
            {
                RuleFor(p => p.ProductDescription)
                    .MaximumLength(500)
                    .WithMessage("Product Description must not exceed 500 characters.");
            });

            // Price Validation (Optional)
            When(p => p.Price.HasValue, () =>
            {
                RuleFor(p => p.Price)
                    .GreaterThan(0)
                    .WithMessage("Price must be greater than zero.");
            });

            // Category ID Validation (Optional)
            When(p => p.CategoryID.HasValue, () =>
            {
                RuleFor(p => p.CategoryID)
                    .GreaterThan(0)
                    .WithMessage("Category ID must be valid.");
            });

            // Image File Validation (Optional)
            When(p => p.ProductImageFile != null, () =>
            {
                // File Size Validation (max 5 MB)
                RuleFor(p => p.ProductImageFile!.Length)
                    .LessThanOrEqualTo(5 * 1024 * 1024)
                    .WithMessage("Image size must not exceed 5 MB.");

                // File Type Validation
                RuleFor(p => p.ProductImageFile!.ContentType)
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
                            // Recommended: 600x600 to 1200x1200
                            // Maximum: 2000x2000
                            return image.Width >= 300 && image.Height >= 300
                                   && image.Width <= 2000 && image.Height <= 2000;
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .WithMessage("Image dimensions must be between 300x300 and 2000x2000 pixels. Recommended: 600x600 to 1200x1200.");

                // Aspect Ratio Validation (Square or near-square images preferred)
                RuleFor(p => p.ProductImageFile)
                    .MustAsync(async (file, cancellation) =>
                    {
                        if (file == null) return true;

                        try
                        {
                            using var stream = file.OpenReadStream();
                            using var image = await Image.LoadAsync(stream, cancellation);

                            // Accept aspect ratios from 0.8 to 1.25 (nearly square)
                            double aspectRatio = (double)image.Width / image.Height;
                            return aspectRatio >= 0.8 && aspectRatio <= 1.25;
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .WithMessage("Image should have a square or near-square aspect ratio (recommended: 1:1).");
            });
        }
    }
}