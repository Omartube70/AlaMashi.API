using Application.Addresses.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Addresses.Commands
{
    public class UpdateAddressPartialCommandHandler : IRequestHandler<UpdateAddressPartialCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateAddressDto> _validator;

        public UpdateAddressPartialCommandHandler(IUserRepository userRepository,IMapper mapper,IValidator<UpdateAddressDto> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task Handle(UpdateAddressPartialCommand request, CancellationToken cancellationToken)
        {
            // 🧩 1. جيب المستخدم بالعناوين
            var user = await _userRepository.GetUserWithAddressesAsync(request.CurrentUserId);
            if (user == null)
                throw new UserNotFoundException(request.CurrentUserId);

            // 🏠 2. جيب العنوان المستهدف
            var addressEntity = user.Addresses.FirstOrDefault(a => a.AddressId == request.AddressId);
            if (addressEntity == null)
                throw new NotFoundException($"Address with ID {request.AddressId} not found for this user.");

            // 🛡️ 3. تحقق إن المستخدم يملك العنوان
            if (addressEntity.UserId != request.CurrentUserId)
                throw new ForbiddenAccessException();

            // 4️⃣ حوّل الكيان لـ DTO
            var dtoToPatch = _mapper.Map<UpdateAddressDto>(addressEntity);

            // 5️⃣ طبق الـ Patch
            request.PatchDoc.ApplyTo(dtoToPatch);

            // 6️⃣ تحقق من صحة البيانات
            var validationResult = await _validator.ValidateAsync(dtoToPatch, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // 7️⃣ حدث الكيان نفسه
            _mapper.Map(dtoToPatch, addressEntity);

            // 8️⃣ احفظ التغييرات عن طريق المستخدم
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
