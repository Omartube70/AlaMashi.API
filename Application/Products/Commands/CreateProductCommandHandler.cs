using Application.Exceptions;
using Application.Interfaces;
using Application.Products.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;


        public CreateProductCommandHandler(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IFileStorageService fileStorageService,       
            IMapper mapper)
        {
            _productRepository = productRepository;
            _fileStorageService = fileStorageService;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Validate Category
            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryID);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {request.CategoryID} was not found");
            }

            // Check Barcode Uniqueness
            var productWithBarcode = await _productRepository.GetProductByBarcodeAsync(request.Barcode);
            if (productWithBarcode != null)
            {
                throw new ConflictException($"The Barcode '{request.Barcode}' already exists.");
            }

            // Upload Image with optimal dimensions
            string imageUrl = string.Empty;
            if (request.ProductImageFile != null && request.ProductImageFile.Length > 0)
            {
                // Upload with target dimensions for dashboard (800x800)
                // The service will resize while maintaining aspect ratio
                imageUrl = await _fileStorageService.UploadFileAsync(
                    request.ProductImageFile,
                    targetWidth: 800,
                    targetHeight: 800
                );
            }
            else
            {
                throw new ArgumentException("Product image is required.");
            }

            // Create Product Entity
            Product newProduct = Product.Create(
                request.ProductName,
                request.Barcode,
                request.ProductDescription,
                request.Price,
                request.QuantityInStock,
                imageUrl,
                request.CategoryID
            );

            await _productRepository.AddProductAsync(newProduct);

            // Return DTO
            return _mapper.Map<ProductDto>(newProduct);
        }
    }
}