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
                .Select(c => _mapper.Map<CategoryDto>(c))
                .ToList();


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
                NewProducts = newProducts,
                DiscountedProducts = discountedProducts
            };

            return dto;
        }


    }
}
