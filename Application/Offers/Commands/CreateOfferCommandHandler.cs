using Domain.Entities;
using Application.Interfaces;
using Application.Offers.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Offers.Commands
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, OfferDto>
    {
        private readonly IOfferRepository _offerRepo;
        private readonly IMapper _mapper;

        public CreateOfferCommandHandler(IOfferRepository offerRepo, IMapper mapper)
        {
            _offerRepo = offerRepo;
            _mapper = mapper;
        }

        public async Task<OfferDto> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Offer>(request.Offer);

            await _offerRepo.AddOfferAsync(entity);

            return _mapper.Map<OfferDto>(entity);
        }
    }
}
