using Application.Addresses.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? DeliveryTimeSlot { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public AddressDto Address { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new();
        public List<PaymentDto> Payments { get; set; } = new();
    }
}
