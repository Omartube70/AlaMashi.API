using Application.Exceptions;
using Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Products.Commands
{
    public class UpdateProductPartialCommandHandler : IRequestHandler<UpdateProductPartialCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IValidator<UpdateProductPartialCommand> _validator;

        public UpdateProductPartialCommandHandler(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IFileStorageService fileStorageService,
            IValidator<UpdateProductPartialCommand> validator)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _fileStorageService = fileStorageService;
            _validator = validator;
        }

        public async Task<Unit> Handle(UpdateProductPartialCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Validation
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // 2️⃣ Get Product
            var productEntity = await _productRepository.GetProductByIdAsync(request.ProductId);
            if (productEntity == null)
                throw new NotFoundException($"Product with ID {request.ProductId} not found.");

            // 3️⃣ Validate Category if changed
            if (request.CategoryID.HasValue)
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryID.Value);
                if (category == null)
                    throw new NotFoundException($"Category with ID {request.CategoryID.Value} not found.");
            }

            // 4️⃣ Update Product Name and Description
            if (!string.IsNullOrWhiteSpace(request.ProductName) || request.ProductDescription != null)
            {
                var newName = !string.IsNullOrWhiteSpace(request.ProductName)
                    ? request.ProductName
                    : productEntity.ProductName;

                var newDescription = request.ProductDescription ?? productEntity.ProductDescription;

                productEntity.UpdateDetails(newName, newDescription);
            }

            // 5️⃣ Update Price
            if (request.Price.HasValue)
            {
                productEntity.ChangePrice(request.Price.Value);
            }

            // 6️⃣ Update Image if provided
            if (request.ProductImageFile != null && request.ProductImageFile.Length > 0)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(productEntity.MainImageURL))
                {
                    await _fileStorageService.DeleteFileAsync(productEntity.MainImageURL);
                }

                // Upload new image with recommended dimensions
                // For dashboard: 800x800, For mobile: 600x600
                string newImageUrl = await _fileStorageService.UploadFileAsync(
                    request.ProductImageFile,
                    targetWidth: 800,
                    targetHeight: 800
                );

                productEntity.UpdateMainImage(newImageUrl);
            }

            // 7️⃣ Save Changes
            await _productRepository.UpdateProductAsync(productEntity);

            return Unit.Value;
        }
    }
}