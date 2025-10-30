using Application.Addresses.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.Addresses.Commands
{
    public class UpdateAddressPartialCommandHandler : IRequestHandler<UpdateAddressPartialCommand, Unit>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateAddressPartialCommand> _validator;

        public UpdateAddressPartialCommandHandler(
            IAddressRepository addressRepository,
            IMapper mapper,
            IValidator<UpdateAddressPartialCommand> validator)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Unit> Handle(UpdateAddressPartialCommand request, CancellationToken cancellationToken)
        {
            // 🔍 جلب العنوان مباشرة
            var addressEntity = await _addressRepository.GetAddressByIdAsync(request.AddressId);

            if (addressEntity == null)
                throw new NotFoundException($"Address with ID {request.AddressId} not found.");

            // 🛡️ تحقق من الصلاحيات (إذا مش أدمن، لازم يكون مالك العنوان)
            if (!request.IsAdmin && addressEntity.UserId != request.CurrentUserId)
                throw new ForbiddenAccessException();

            // 4️⃣ حوّل الكيان لـ DTO
            var dtoToPatch = _mapper.Map<AddressDto>(addressEntity);

            // 5️⃣ طبق الـ Patch
            request.PatchDoc.ApplyTo(dtoToPatch);

            // 6️⃣ تحقق من صحة البيانات
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // 7️⃣ حدث الكيان نفسه
            _mapper.Map(dtoToPatch, addressEntity);

            // 8️⃣ احفظ التغييرات
            await _addressRepository.UpdateAddressAsync(addressEntity);

            return Unit.Value;
        }
    }
}