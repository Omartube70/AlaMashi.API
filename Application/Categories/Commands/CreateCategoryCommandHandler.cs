using Application.Categories.Dtos;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Categories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category NewCategory = Category.Create(request.CategoryName, request.ParentId);

            await _categoryRepository.AddCategoryAsync(NewCategory);


            return new CategoryDto 
            { 
               CategoryId   = NewCategory.CategoryID,
               CategoryName = NewCategory.CategoryName,
               ParentId     = NewCategory.ParentID,
            };
        }
    }
}

