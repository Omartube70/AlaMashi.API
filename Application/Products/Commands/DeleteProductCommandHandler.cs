using MediatR;
using Application.Interfaces;
using Application.Exceptions;

namespace Application.Products.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileUploadService _fileUploadService;


        public DeleteProductCommandHandler(IProductRepository productRepository , IFileUploadService fileUploadService)
        {
            _productRepository = productRepository;
            _fileUploadService = fileUploadService;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productToDelete = await _productRepository.GetProductByIdAsync(request.ProductID);

            if (productToDelete == null)
            {
                throw new NotFoundException($"Product with ID {request.ProductID} not found.");
            }

            if (!string.IsNullOrEmpty(productToDelete.MainImageURL))
            {
                await _fileUploadService.DeleteFileAsync(productToDelete.MainImageURL);
            }

            await _productRepository.DeleteProductAsync(productToDelete);

        }
    }
}