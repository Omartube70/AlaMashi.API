using Application.Exceptions;
using Application.Interfaces;
using Application.Products.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailsDto?>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDetailsDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductWithDetailsAsync(request.ProductId);

            if (product == null)
                throw new NotFoundException($"Product with ID {request.ProductId} not found.");

           return _mapper.Map<ProductDetailsDto>(product);
        }
    }
}
