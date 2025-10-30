using Application.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class CreatePaymentCommand : IRequest<PaymentDto>
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
    }

}
