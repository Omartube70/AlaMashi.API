using Application.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public class GetAllOrdersQuery : IRequest<IReadOnlyList<OrderDto>>
    {
        public string? Status { get; set; }
    }
}
