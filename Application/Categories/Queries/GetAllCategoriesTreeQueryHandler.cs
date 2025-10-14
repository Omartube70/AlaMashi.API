using Application.Categories.Dtos;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Queries
{
    public class GetAllCategoriesTreeQueryHandler : IRequestHandler<GetAllCategoriesTreeQuery, IReadOnlyList<CategoryTreeDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesTreeQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IReadOnlyList<CategoryTreeDto>> Handle(GetAllCategoriesTreeQuery request, CancellationToken cancellationToken)
        {
            var allCategories = await _categoryRepository.GetAllCategoriesAsync();

            // 🧩 بناء القاموس لكل الفئات
            var categoryMap = allCategories.ToDictionary(
                c => c.CategoryID,
                c => new CategoryTreeDto
                {
                    CategoryId = c.CategoryID,
                    CategoryName = c.CategoryName,
                    IconName = c.IconName,
                    SubCategories = new List<CategoryTreeDto>()
                }
            );

            // 📚 بناء العلاقات بين الأب والابن
            var rootCategories = new List<CategoryTreeDto>();

            foreach (var category in allCategories)
            {
                if (category.ParentID.HasValue && categoryMap.ContainsKey(category.ParentID.Value))
                {
                    categoryMap[category.ParentID.Value].SubCategories.Add(categoryMap[category.CategoryID]);
                }
                else
                {
                    rootCategories.Add(categoryMap[category.CategoryID]);
                }
            }

            return rootCategories;
        }
    }
}