using MediatR;
using Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Commands
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToDelete = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);

            if (categoryToDelete != null)
            {
               await _categoryRepository.DeleteCategory(categoryToDelete);
            }

        }
    }
}