using Application.Exceptions;
using Application.Interfaces;
using Application.Products.Dtos;
using MediatR;

namespace Application.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailsDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDetailsDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // جلب المنتج مع كل التفاصيل المطلوبة
            var product = await _productRepository.GetProductWithDetailsAsync(request.ProductId);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.ProductId} not found.");
            }

            // تحويل الطلبات إلى DTO
            var orders = product.OrderDetails?
                .Where(od => od.Order != null)
                .OrderByDescending(od => od.Order.OrderDate)
                .Select(od => new ProductOrderDto
                {
                    OrderId = od.OrderId,
                    Quantity = od.Quantity,
                    OrderDate = od.Order.OrderDate,
                    Subtotal = od.Subtotal,
                    CustomerName = od.Order.User?.UserName ?? "N/A"
                })
                .ToList();

            return new ProductDetailsDto
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Barcode = product.Barcode,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                MainImageURL = product.MainImageURL,
                CategoryId = product.CategoryID,
                CategoryName = product.Category?.CategoryName ?? "N/A",
                Orders = orders
            };
        }
    }
}