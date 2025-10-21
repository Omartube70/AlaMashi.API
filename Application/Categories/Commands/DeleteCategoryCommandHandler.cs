using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Commands
{

    namespace Application.Categories.Commands
    {
        public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
        {
            private readonly ICategoryRepository _categoryRepository;

            public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository) 
            {
                _categoryRepository = categoryRepository;
            }

            public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                var categoryToDelete = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
                if (categoryToDelete == null)
                {
                    throw new NotFoundException($"Category id {request.CategoryId} was not found");
                }

                await _categoryRepository.DeleteCategory(categoryToDelete);

                return Unit.Value;
            }
        }
    }
}