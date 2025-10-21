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

            categoryToUpdate.UpdateCategoryName(request.NewCategoryName);


           await _categoryRepository.UpdateCategory(categoryToUpdate); 

            return Unit.Value;
        }
    }
}