using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Orders.Commands
{
    public class GetAllOrdersQuery : IRequest<Unit>
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public int CurrentUserId { get; set; }
        public string CurrentUserRole { get; set; }
    }
}
