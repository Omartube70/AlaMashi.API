using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Commands
{

    namespace Application.Categories.Commands
    {
        public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IFileUploadService _fileUploadService;

            public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository,IFileUploadService fileUploadService) 
            {
                _categoryRepository = categoryRepository;
                _fileUploadService = fileUploadService;
            }

            public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                var categoryToDelete = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
                if (categoryToDelete == null)
                {
                    throw new NotFoundException($"Category id {request.CategoryId} was not found");
                }

                await _categoryRepository.DeleteCategory(categoryToDelete);
            }
        }
    }
}