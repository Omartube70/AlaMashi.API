using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class UpdateOrderDeliveryDetailsCommand : IRequest<Unit>
    {
        public int OrderId { get; set; }
        public DateTime? NewDeliveryDate { get; set; }
        public string? NewDeliveryTimeSlot { get; set; }
        public int? NewAddressId { get; set; }
    }
}
