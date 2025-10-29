using Application.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public class GetAllPaymentsQuery : IRequest<IReadOnlyList<PaymentDto>>
    {
        public int? OrderId { get; set; } // Optional: filter by specific order
    }
}
