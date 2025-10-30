using MediatR;
using Application.Categories.Dtos;
using Application.Interfaces;
using Application.Products.Dtos;


namespace Application.Categories.Queries
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IReadOnlyList<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            return categories.Select(category => new CategoryDto
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
            }).ToList();
        }
    }
}