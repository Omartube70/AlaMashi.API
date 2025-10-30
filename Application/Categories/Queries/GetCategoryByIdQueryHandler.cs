using Application.Categories.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Products.Dtos;
using MediatR;

namespace Application.Categories.Queries
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);

            if (category == null)
            {
                throw new NotFoundException($"Category with ID {request.CategoryId} not found.");
            }

            return new CategoryDto
            {
                CategoryId = category.CategoryID,
                CategoryName = category.CategoryName,
                IconName = category.IconName,
                Products = category.Products.Select(p => new ProductDto
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    Price = p.Price,
                    QuantityInStock = p.QuantityInStock,
                    MainImageURL = p.MainImageURL,
                    CategoryName = category.CategoryName
                }).ToList()
            };
        }
    }
}