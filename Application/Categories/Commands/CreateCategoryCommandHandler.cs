using Application.Categories.Dtos;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Categories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileUploadService _fileUploadService;
        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository ,IFileUploadService fileUploadService)
        {
            _categoryRepository = categoryRepository;
            _fileUploadService = fileUploadService;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            string imageUrl = string.Empty;
            if (request.CategoryImageFile != null && request.CategoryImageFile.Length > 0)
            {
                imageUrl = await _fileUploadService.UploadFileAsync(request.CategoryImageFile, 800, 600);
            }
            else
            {
                throw new ArgumentException("Category image is required.");
            }

            Category NewCategory = Category.Create(request.CategoryName , imageUrl , request.ParentId);


            await _categoryRepository.AddCategoryAsync(NewCategory);


            return new CategoryDto 
            { 
               CategoryId   = NewCategory.CategoryID,
               CategoryName = NewCategory.CategoryName,
               CategoryImageURL = NewCategory.CategoryImageURL,
               ParentId     = NewCategory.ParentID,
            };
        }
    }
}

