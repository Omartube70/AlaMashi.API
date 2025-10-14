using Application.Interfaces;
using Application.Offers.Dtos;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Queries
{
    public class GetAllOffersQueryHandler : IRequestHandler<GetAllOffersQuery, IEnumerable<OfferDto>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public GetAllOffersQueryHandler(IOfferRepository offerRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OfferDto>> Handle(GetAllOffersQuery request, CancellationToken cancellationToken)
        {
            var offers = await _offerRepository.GetAllOffersAsync();
            return _mapper.Map<IEnumerable<OfferDto>>(offers);
        }
    }
}
