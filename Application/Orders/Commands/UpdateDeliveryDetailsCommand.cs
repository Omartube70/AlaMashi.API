using Application.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class GetUserOrdersQuery : IRequest<Unit>
    {
        public int OrderId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryTimeSlot { get; set; }
        public int CurrentUserId { get; set; }
        public string CurrentUserRole { get; set; }
    }
}
