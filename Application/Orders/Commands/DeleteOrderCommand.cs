using Domain.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Dtos
{
    public class GetOrderByIdQuery : IRequest<Unit>
    {
        public int OrderId { get; set; }
        public int CurrentUserId { get; set; }
        public string CurrentUserRole { get; set; }
    }
}
