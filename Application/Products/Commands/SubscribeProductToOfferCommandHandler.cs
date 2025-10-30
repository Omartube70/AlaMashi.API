using Application.Exceptions;
using Application.Interfaces;
using Application.Products.Commands;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class SubscribeProductToOfferCommandHandler : IRequestHandler<SubscribeProductToOfferCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOfferRepository _offerRepository;

        public SubscribeProductToOfferCommandHandler(IProductRepository productRepository , IOfferRepository offerRepository)
        {
            _productRepository = productRepository;
            _offerRepository = offerRepository;
        }

        public async Task<Unit> Handle(SubscribeProductToOfferCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.ProductId} not found.");
            }

            var offerEntity = await _offerRepository.GetOfferByIdAsync(request.OfferId);
            if (offerEntity == null)
                throw new NotFoundException($"Offer With Id {request.OfferId} was not found");

            product.AssignToOffer(request.OfferId);

            await _productRepository.UpdateProductAsync(product);

            return Unit.Value;
        }
    }
}
