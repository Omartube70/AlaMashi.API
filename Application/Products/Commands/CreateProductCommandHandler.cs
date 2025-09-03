using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Application.Products.Dtos;


namespace Application.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileUploadService _fileUploadService;
        public CreateProductCommandHandler(IProductRepository productRepository, IFileUploadService fileUploadService)
        {
            _productRepository = productRepository;
            _fileUploadService = fileUploadService;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            string imageUrl = string.Empty;
            if (request.ProductImageFile != null && request.ProductImageFile.Length > 0)
            {
                imageUrl = await _fileUploadService.UploadFileAsync(request.ProductImageFile);
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
                MainImageURL = NewProduct.MainImageURL            
            };
        }
    }
}