using MediatR;
using Application.Products.Dtos;
using Application.Interfaces;
using AutoMapper;

namespace Application.Products.Queries
{
    public class GetAllProductsByCategoryQueryHandler : IRequestHandler<GetAllProductsByCategoryQuery, IReadOnlyList<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsByCategoryQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProductDto>> Handle(GetAllProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProductsByCategoryIdAsync(request.CategoryId);

            var productDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            return productDtos;
        }
    }
}
