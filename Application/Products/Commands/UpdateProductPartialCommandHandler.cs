using Application.Exceptions;
using Application.Interfaces;
using Application.Products.Dtos;
using AutoMapper;
using FluentValidation;
using MediatR;


namespace Application.Products.Commands
{
    public class UpdateProductPartialCommandHandler : IRequestHandler<UpdateProductPartialCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateProductDto> _validator;

        public UpdateProductPartialCommandHandler(
            IProductRepository productRepository,
            IMapper mapper,
            IValidator<UpdateProductDto> validator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task Handle(UpdateProductPartialCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ تأكد إن المنتج موجود
            var productEntity = await _productRepository.GetProductByIdAsync(request.ProductId);

            if (productEntity == null)
                throw new NotFoundException($"Product with ID {request.ProductId} not found.");

            // 2️⃣ حوّل الـ Entity إلى DTO لتطبيق الـ Patch عليه
            var dtoToPatch = _mapper.Map<UpdateProductDto>(productEntity);

            // 3️⃣ طبّق التغييرات
            request.PatchDoc.ApplyTo(dtoToPatch);

            // 4️⃣ تحقق من صحة القيم بعد التحديث
            var validationResult = await _validator.ValidateAsync(dtoToPatch, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // 5️⃣ حدّث الـ Entity بالقيم الجديدة
            _mapper.Map(dtoToPatch, productEntity);

            // 6️⃣ حفظ التغييرات
            await _productRepository.UpdateProductAsync(productEntity);
        }
    }
}
