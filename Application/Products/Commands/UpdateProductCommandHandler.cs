using MediatR;
using Application.Interfaces;
using Application.Exceptions;


namespace Application.Products.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productToUpdate = await _productRepository.GetProductByIdAsync(request.ProductID);

            if (productToUpdate == null)
            {
                 throw new NotFoundException($"Product with ID {request.ProductID} not found.");
            }

            productToUpdate.UpdateDetails(request.ProductName, request.ProductDescription);
            productToUpdate.ChangePrice(request.Price);


            await _productRepository.UpdateProductAsync(productToUpdate);
        }
    }
}
