using MediatR;
using Application.Interfaces;
using Application.Exceptions;

namespace Application.Products.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileStorageService _fileStorageService;


        public DeleteProductCommandHandler(IProductRepository productRepository , IFileStorageService fileStorageService)
        {
            _productRepository = productRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productToDelete = await _productRepository.GetProductByIdAsync(request.ProductID);

            if (productToDelete == null)
            {
                throw new NotFoundException($"Product with ID {request.ProductID} not found.");
            }

            if (!string.IsNullOrEmpty(productToDelete.MainImageURL))
            {
                await _fileStorageService.DeleteFileAsync(productToDelete.MainImageURL);
            }

            await _productRepository.DeleteProductAsync(productToDelete);

            return Unit.Value;
        }
    }
}