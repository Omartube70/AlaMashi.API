using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Dtos
{
    public class OrderItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
