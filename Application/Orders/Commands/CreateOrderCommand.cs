using Application.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest<OrderDto>
    {
        public int CurrentUserId { get; set; }
        public int AddressId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? DeliveryTimeSlot { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
