using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Commands
{
    public class DeleteOfferCommand : IRequest<Unit>
    {
        public int OfferId { get; set; }

        public DeleteOfferCommand(int offerId)
        {
            OfferId = offerId;
        }
    }
}
