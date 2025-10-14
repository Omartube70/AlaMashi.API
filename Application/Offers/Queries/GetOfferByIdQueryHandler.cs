using Application.Exceptions;
using Application.Interfaces;
using Application.Offers.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Queries
{
    public class GetOfferByIdQueryHandler : IRequestHandler<GetOfferByIdQuery, OfferDto>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public GetOfferByIdQueryHandler(IOfferRepository offerRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<OfferDto> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
        {
            var offer = await _offerRepository.GetOfferByIdAsync(request.OfferId);

            if (offer == null)
                throw new NotFoundException($"Offer With Id {request.OfferId} was not found");

            return _mapper.Map<OfferDto>(offer);
        }
    }
}
