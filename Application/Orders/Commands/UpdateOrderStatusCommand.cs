using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class UpdateOrderStatusCommand : IRequest<Unit>
    {
        public int OrderId { get; set; }
        public string NewStatus { get; set; }
    }
}
