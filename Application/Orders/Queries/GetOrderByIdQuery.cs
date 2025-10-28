using Application.Orders.Dtos;
using Domain.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderDto>
    {
        public int OrderId { get; set; }
        public int CurrentUserId { get; set; }
        public string CurrentUserRole { get; set; }
    }
}
