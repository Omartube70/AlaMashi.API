using MediatR;
using Application.Categories.Dtos;
using Application.Interfaces;


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
                ParentId = category.ParentID
            }).ToList();
        }
    }
}