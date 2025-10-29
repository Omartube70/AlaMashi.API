using Application.Addresses.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Orders.Dtos;
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

        public GetUserOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IReadOnlyList<OrderDto>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            bool isAdmin = request.CurrentUserRole == "Admin";
            bool isSelf = request.CurrentUserId == request.UserId;

            if (!isAdmin && !isSelf)
                throw new ForbiddenAccessException("You don't have permission to view these orders.");

            var orders = await _orderRepository.GetOrdersByUserIdAsync(request.UserId, includeDetails: true);

            return orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                DeliveryDate = o.DeliveryDate,
                DeliveryTimeSlot = o.DeliveryTimeSlot,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),
                UserId = o.UserId,
                Address = new AddressDto
                {
                    AddressId = o.Address.AddressId,
                    Street = o.Address.Street,
                    City = o.Address.City,
                    AddressDetails = o.Address.AddressDetails
                },
                OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    ProductName = od.Product.ProductName,
                    Quantity = od.Quantity,
                    PriceAtOrder = od.PriceAtOrder,
                    Subtotal = od.Subtotal
                }).ToList()
            }).ToList();
        }
    }
}
