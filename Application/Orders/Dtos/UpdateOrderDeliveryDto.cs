using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Dtos
{
    public class UpdateOrderDeliveryDto
    {
        public DateTime? NewDeliveryDate { get; set; }
        public string? NewDeliveryTimeSlot { get; set; }
        public int? NewAddressId { get; set; }
    }
}
