using Application.Exceptions;
using Application.Interfaces; 
using Application.Products.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.ProductId} not found.");
            }

            return new ProductDto
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                MainImageURL = product.MainImageURL,
                CategoryName = product.Category.CategoryName,
            };
        }
    }
}