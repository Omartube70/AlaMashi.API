using MediatR;
using Application.Interfaces;
using Application.Exceptions;

namespace Application.Products.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productToDelete = await _productRepository.GetProductByIdAsync(request.ProductID);

            if (productToDelete == null)
            {
                throw new NotFoundException($"Product with ID {request.ProductID} not found.");
            }

           await _productRepository.DeleteProductAsync(productToDelete);

        }
    }
}