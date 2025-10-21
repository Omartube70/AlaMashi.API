using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Commands
{
    public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand, Unit>
    {
        private readonly IOfferRepository _offerRepository;

        public DeleteOfferCommandHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<Unit> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
        {
            var offerEntity = await _offerRepository.GetOfferByIdAsync(request.OfferId);

            if (offerEntity == null)
                throw new NotFoundException($"Offer With Id {request.OfferId} was not found");

            await _offerRepository.DeleteOfferAsync(offerEntity);

            return Unit.Value;
        }
    }
}
