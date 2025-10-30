using Application.Addresses.Dtos;
using Application.Interfaces;
using Application.Orders.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IReadOnlyList<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Order> orders;

            if (!string.IsNullOrEmpty(request.Status))
            {
                orders = await _orderRepository.GetOrdersByStatusAsync(request.Status, includeDetails: true);
            }
            else
            {
                orders = await _orderRepository.GetAllOrdersAsync(includeDetails: true);
            }

            return _mapper.Map<IReadOnlyList<OrderDto>>(orders);
        }
    }

}
