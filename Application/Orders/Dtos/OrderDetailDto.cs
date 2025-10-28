using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Dtos
{
    public class OrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
        public decimal Subtotal { get; set; }
    }
}
