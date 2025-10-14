using Application.Categories.Dtos;
using Application.Home.Dtos;
using Application.Home.Queries.Application.Home.Queries;
using Application.Interfaces;
using Application.Offers.Dtos;
using Application.Products.Dtos;
using AutoMapper;
using MediatR;


namespace Application.Home.Queries
{
    public class GetHomeDataQueryHandler : IRequestHandler<GetHomeDataQuery, HomeDataDto>
    {
        private readonly IOfferRepository _offerRepo;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public GetHomeDataQueryHandler(
            IOfferRepository offerRepo,
            IProductRepository productRepo,
            ICategoryRepository categoryRepo,
            IMapper mapper)
        {
            _offerRepo = offerRepo;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<HomeDataDto> Handle(GetHomeDataQuery request, CancellationToken cancellationToken)
        {
            // ✅ استرجاع البيانات
            var offers = await _offerRepo.GetActiveOffersAsync();
            var categories = await _categoryRepo.GetAllCategoriesAsync();
            var products = await _productRepo.GetAllProductsAsync();

            // ✅ تجهيز الفئات الرئيسية (Flat)
            var flatCategories = categories
                .Where(c => c.ParentID == null)
                .Select(c => _mapper.Map<CategoryDto>(c))
                .ToList();

            // ✅ تجهيز الفئات الشجرية (Tree)
            var categoryTree = BuildCategoryTree(categories);

            // ✅ المنتجات
            var newProducts = products
                .OrderByDescending(p => p.ProductID)
                .Take(6)
                .Select(p => _mapper.Map<ProductDto>(p))
                .ToList();

            var discountedProducts = products
                .Where(p => p.OfferID != null)
                .Select(p => _mapper.Map<ProductDto>(p))
                .ToList();

            // ✅ تجميع الكل في DTO
            var dto = new HomeDataDto
            {
                FeaturedOffers = offers.Select(o => _mapper.Map<OfferDto>(o)).ToList(),
                Categories = flatCategories,
                CategoryTree = categoryTree,
                NewProducts = newProducts,
                DiscountedProducts = discountedProducts
            };

            return dto;
        }

        // 🧩 دالة بناء شجرة الفئات
        private List<CategoryTreeDto> BuildCategoryTree(IReadOnlyList<Domain.Entities.Category> allCategories)
        {
            var map = allCategories.ToDictionary(
                c => c.CategoryID,
                c => new CategoryTreeDto
                {
                    CategoryId = c.CategoryID,
                    CategoryName = c.CategoryName,
                    IconName = c.IconName,
                    SubCategories = new List<CategoryTreeDto>()
                }
            );

            var roots = new List<CategoryTreeDto>();

            foreach (var category in allCategories)
            {
                if (category.ParentID.HasValue && map.ContainsKey(category.ParentID.Value))
                {
                    map[category.ParentID.Value].SubCategories.Add(map[category.CategoryID]);
                }
                else
                {
                    roots.Add(map[category.CategoryID]);
                }
            }

            return roots;
        }
    }
}
