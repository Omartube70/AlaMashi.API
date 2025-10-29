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

            return MapToOrderDto(order);
        }

        private OrderDto MapToOrderDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                DeliveryTimeSlot = order.DeliveryTimeSlot,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                UserId = order.UserId,
                UserName = order.User.UserName,
                UserPhone = order.User.Phone,
                Address = new AddressDto
                {
                    AddressId = order.Address.AddressId,
                    Street = order.Address.Street,
                    City = order.Address.City,
                    AddressDetails = order.Address.AddressDetails
                },
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    ProductName = od.Product.ProductName,
                    Quantity = od.Quantity,
                    PriceAtOrder = od.PriceAtOrder,
                    Subtotal = od.Subtotal
                }).ToList(),
                Payments = order.Payments.Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    PaymentMethod = p.PaymentMethod.ToString(),
                    PaymentStatus = p.PaymentStatus.ToString(),
                    TransactionId = p.TransactionId
                }).ToList()
            };
        }
    }
}
