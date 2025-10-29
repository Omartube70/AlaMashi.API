using Application.Addresses.Dtos;
using Application.Interfaces;
using Application.Orders.Dtos;
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

        public GetAllOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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

            return orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                DeliveryDate = o.DeliveryDate,
                DeliveryTimeSlot = o.DeliveryTimeSlot,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),
                UserId = o.UserId,
                UserName = o.User.UserName,
                UserPhone = o.User.Phone,
                Address = new AddressDto
                {
                    AddressId = o.Address.AddressId,
                    Street = o.Address.Street,
                    City = o.Address.City,
                    AddressDetails = o.Address.AddressDetails,
                    AddressType = o.Address.AddressType.ToString(),
                    UserId = o.UserId,
                }
            }).ToList();
        }
    }

}
