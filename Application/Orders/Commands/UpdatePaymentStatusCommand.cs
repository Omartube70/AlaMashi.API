using Application.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class GetOrderPaymentsQuery : IRequest<Unit>
    {
        public int PaymentId { get; set; }
        public string Status { get; set; }
        public string? TransactionId { get; set; }
        public int CurrentUserId { get; set; }
        public string CurrentUserRole { get; set; }
    }
}
