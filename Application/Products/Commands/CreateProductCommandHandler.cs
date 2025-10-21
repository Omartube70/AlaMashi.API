using Application.Exceptions;
using Application.Interfaces;
using Application.Products.Dtos;
using Domain.Entities;
using MediatR;


namespace Application.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileStorageService _fileStorageService;
        public CreateProductCommandHandler(IProductRepository productRepository , ICategoryRepository categoryRepository, IFileStorageService fileStorageService)
        {
            _productRepository = productRepository;
            _fileStorageService = fileStorageService;
            _categoryRepository = categoryRepository;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryID);
            if(category == null)
            {
                throw new NotFoundException($"Category id {request.CategoryID} was not found");
            }

            var ProductWithBarcode = await _productRepository.GetProductByBarcodeAsync(request.Barcode);
            if (ProductWithBarcode != null)
            {
                throw new ConflictException($"The Barcode '{request.Barcode}' is already Exits.");
            }


            string imageUrl = string.Empty;
            if (request.ProductImageFile != null && request.ProductImageFile.Length > 0)
            {
                imageUrl = await _fileStorageService.UploadFileAsync(request.ProductImageFile);
            }
            else
            {
                throw new ArgumentException("Product image is required.");
            }


            Product NewProduct = Product.Create(
                request.ProductName,
                request.Barcode,
                request.ProductDescription,
                request.Price,
                request.QuantityInStock,
                imageUrl,
                request.CategoryID
            );

            await _productRepository.AddProductAsync(NewProduct);

            return new ProductDto
            {
                ProductID = NewProduct.ProductID,
                ProductName = NewProduct.ProductName,
                ProductDescription = NewProduct.ProductDescription,
                Price = NewProduct.Price,
                QuantityInStock = NewProduct.QuantityInStock,
                MainImageURL = NewProduct.MainImageURL,
                CategoryName = category.CategoryName
            };
        }
    }
}