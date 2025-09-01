using MediatR;
using Application.Products.Dtos;
using Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class GetAllProductsByCategoryQueryHandler : IRequestHandler<GetAllProductsByCategoryQuery, IReadOnlyList<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsByCategoryQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IReadOnlyList<ProductDto>> Handle(GetAllProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllProductsByCategoryIdAsync(request.CategoryId);

            var productDtos = products
                .Select(product => new ProductDto
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    Price = product.Price,
                    QuantityInStock = product.QuantityInStock,
                    MainImageURL = product.MainImageURL,
                    CategoryName = product.Category.CategoryName,
                }).ToList();

            return productDtos;
        }
    }
}