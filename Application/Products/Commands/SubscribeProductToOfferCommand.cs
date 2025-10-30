using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class SubscribeProductToOfferCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public int OfferId { get; set; }
    }
}
