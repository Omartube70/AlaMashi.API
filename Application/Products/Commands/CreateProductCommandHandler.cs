using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Application.Products.Dtos;


namespace Application.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var NewProduct = Product.Create(
                request.ProductName,
                request.Barcode,
                request.ProductDescription,
                request.Price,
                request.QuantityInStock,
                request.MainImageURL,
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
                CategoryName = NewProduct.Category.CategoryName
            };
        }
    }
}