using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Categories.Commands
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToUpdate = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);

            if (categoryToUpdate == null)
            {
                throw new NotFoundException($"Category with ID {request.CategoryId} not found.");
            }

            // Update category name if provided
            if (request.CategoryName != null)
            {
                categoryToUpdate.UpdateCategoryName(request.CategoryName);
            }

            // Update icon name if provided
            if (request.IconName != null)
            {
                categoryToUpdate.UpdateIconName(request.IconName);
            }

            await _categoryRepository.UpdateCategory(categoryToUpdate);

            return Unit.Value;
        }
    }
}