using Application.Offers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Commands
{
    public class CreateOfferCommand : IRequest<OfferDto>
    {
        public CreateOrderDto Offer { get; set; }

        public CreateOfferCommand(CreateOrderDto offer)
        {
            Offer = offer;
        }
    }
}
