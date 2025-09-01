using MediatR;
using Application.Categories.Dtos;
using Application.Interfaces;
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
            var categoryMap = new Dictionary<int, CategoryTreeDto>();
            var rootCategories = new List<CategoryTreeDto>();

            // الخطوة 1: قم بإنشاء DTO لكل فئة وضعها في الـ Map لسهولة الوصول
            foreach (var category in allCategories)
            {
                categoryMap[category.CategoryID] = new CategoryTreeDto
                {
                    CategoryId = category.CategoryID,
                    CategoryName = category.CategoryName
                };
            }

            // الخطوة 2: قم ببناء هيكل الشجرة
            foreach (var category in allCategories)
            {
                if (category.ParentID.HasValue && categoryMap.ContainsKey(category.ParentID.Value))
                {
                    // إذا كانت فئة فرعية، أضفها إلى قائمة الفئات الفرعية لوالدها
                    var parentDto = categoryMap[category.ParentID.Value];
                    parentDto.SubCategories.Add(categoryMap[category.CategoryID]);
                }
                else
                {
                    // إذا كانت فئة رئيسية، أضفها إلى القائمة الجذرية
                    rootCategories.Add(categoryMap[category.CategoryID]);
                }
            }

            return rootCategories;
        }
    }
}