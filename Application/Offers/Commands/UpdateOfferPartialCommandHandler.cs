using Application.Exceptions;
using Application.Interfaces;
using Application.Offers.Dtos;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offers.Commands
{
    public class UpdateOfferPartialCommandHandler : IRequestHandler<UpdateOfferPartialCommand, Unit>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateOfferPartialCommand> _dtoValidator;

        public UpdateOfferPartialCommandHandler(IOfferRepository offerRepository, IMapper mapper, IValidator<UpdateOfferPartialCommand> dtoValidator)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
            _dtoValidator = dtoValidator;
        }

        public async Task<Unit> Handle(UpdateOfferPartialCommand request, CancellationToken cancellationToken)
        {
            // 1. Get the offer from DB
            var offerEntity = await _offerRepository.GetOfferByIdAsync(request.OfferId);

            if (offerEntity == null)
                throw new NotFoundException($"Offer With Id {request.OfferId} was not found");

            // 2. Map entity to DTO for patching
            var offerToPatch = _mapper.Map<UpdateOfferDto>(offerEntity);

            // 3. Apply the patch document
            request.PatchDoc.ApplyTo(offerToPatch);

            // 4. Validate patched data
            var validationResult = await _dtoValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // 5. Map updated fields back to entity
            _mapper.Map(offerToPatch, offerEntity);

            // 6. Save changes
            await _offerRepository.UpdateOfferAsync(offerEntity);

            // 7. Return updated DTO
            return Unit.Value;
        }
    }
}
