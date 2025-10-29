using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Dtos
{
    public class CreatePaymentDto
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
    }
}
