using Application.Offers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Queries
{
    public class GetOfferByIdQuery : IRequest<OfferDto>
    {
        public int OfferId { get; set; }

        public GetOfferByIdQuery(int offerId)
        {
            OfferId = offerId;
        }
    }
}
