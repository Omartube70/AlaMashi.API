using Application.Dashboard.Dtos;
using Application.Interfaces;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Queries
{
    public class GetTopSellingProductsQueryHandler : IRequestHandler<GetTopSellingProductsQuery, List<TopProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetTopSellingProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<TopProductDto>> Handle(GetTopSellingProductsQuery request, CancellationToken cancellationToken)
        {
            var topProducts = await _productRepository.GetTopSellingProductsAsync(
                request.StartDate,
                request.EndDate,
                request.TopCount,
                cancellationToken
            );

            return topProducts;
        }

    }
}
