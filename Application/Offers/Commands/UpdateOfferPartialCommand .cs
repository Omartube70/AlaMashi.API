using Application.Offers.Dtos;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Commands
{
    public class UpdateOfferPartialCommand : IRequest<Unit>
    {
        public int OfferId { get; set; }
        public JsonPatchDocument<UpdateOfferDto> PatchDoc { get; set; }

        public UpdateOfferPartialCommand(int offerId, JsonPatchDocument<UpdateOfferDto> patchDoc)
        {
            OfferId = offerId;
            PatchDoc = patchDoc;
        }
    }
}
