using Application.Addresses.Dtos;
using Application.Exceptions;
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
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId, includeDetails: true);

            if (order == null)
                throw new NotFoundException($"Order with ID {request.OrderId} not found.");

            // التحقق من الصلاحيات
            bool isAdmin = request.CurrentUserRole == "Admin";
            bool isOwner = order.UserId == request.CurrentUserId;

            if (!isAdmin && !isOwner)
                throw new ForbiddenAccessException("You don't have permission to view this order.");

            return _mapper.Map<OrderDto>(order);
        }
    }
}
