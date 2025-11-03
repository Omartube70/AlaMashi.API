using Application.Categories.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Categories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // 🏗️ إنشاء الفئة
            var newCategory = Category.Create(request.CategoryName, request.IconName);

            // 💾 الحفظ
            await _categoryRepository.AddCategoryAsync(newCategory);

            // 🔁 المابينج التلقائي (AutoMapper)
            return _mapper.Map<CategoryDto>(newCategory);
        }
    }
}
