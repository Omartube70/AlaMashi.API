using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Application.Orders.Dtos
{
    public class CreateOrderDto
    {
        [Required]
        public int AddressId { get; set; }

        [Required]
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
