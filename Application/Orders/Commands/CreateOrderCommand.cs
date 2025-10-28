using Application.Orders.Dtos;
using Domain.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest<OrderDto>
    {
        public int CurrentUserId { get; set; }
        public int AddressId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
