using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Dtos
{
    public class CreatePaymentCommand
    {
        [Required]
        public string Status { get; set; }

        public string? TransactionId { get; set; }
    }
}
