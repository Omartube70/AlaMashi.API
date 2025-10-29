using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Dtos
{
    public class CreateOrderDto
    {
        public int AddressId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? DeliveryTimeSlot { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
