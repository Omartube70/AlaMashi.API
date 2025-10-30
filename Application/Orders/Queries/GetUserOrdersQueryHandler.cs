using Application.Addresses.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Orders.Dtos;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, IReadOnlyList<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetUserOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<OrderDto>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            bool isAdmin = request.CurrentUserRole == "Admin";
            bool isSelf = request.CurrentUserId == request.UserId;

            if (!isAdmin && !isSelf)
                throw new ForbiddenAccessException("You don't have permission to view these orders.");

            var orders = await _orderRepository.GetOrdersByUserIdAsync(request.UserId, includeDetails: true);

            return _mapper.Map<IReadOnlyList<OrderDto>>(orders);
        }
    }
}
