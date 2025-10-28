using Application.Orders.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Orders.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. تحقق من وجود اليوزر
            var user = await _userRepository.GetUserByIdAsync(request.CurrentUserId);
            if (user == null)
                throw new UserNotFoundException(request.CurrentUserId);

            // 2. تحقق من وجود العنوان
            var userWithAddresses = await _userRepository.GetUserWithAddressesAsync(request.CurrentUserId);
            var address = userWithAddresses?.Addresses.FirstOrDefault(a => a.AddressId == request.AddressId);
            if (address == null)
                throw new NotFoundException($"Address with ID {request.AddressId} not found for this user.");

            // 3. إنشاء الأوردر
            var order = Order.Create(request.CurrentUserId, request.AddressId);
            await _orderRepository.AddOrderAsync(order);


            // 4. إضافة OrderDetails
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product == null)
                    throw new NotFoundException($"Product with ID {item.ProductId} not found.");

                if (product.QuantityInStock < item.Quantity)
                    throw new ArgumentException($"Not enough stock for product {product.ProductName}");

                var orderDetail = OrderDetail.Create(
                    order.OrderId,
                    product.ProductID,
                    item.Quantity,
                    product.Price // حفظ السعر وقت الطلب
                );

                order.AddOrderDetail(orderDetail);
                product.RemoveStock(item.Quantity); // تقليل المخزون
                await _productRepository.UpdateProductAsync(product);
            }

            // 5. حفظ الأوردر
            await _orderRepository.UpdateOrderAsync(order);

            // 6. إرجاع DTO
            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                UserId = order.UserId,
                UserName = user.UserName,
                AddressId = order.AddressId,
                AddressDetails = $"{address.Street}, {address.City}",
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    ProductName = od.Product.ProductName,
                    Quantity = od.Quantity,
                    PriceAtOrder = od.PriceAtOrder,
                    Subtotal = od.Subtotal
                }).ToList(),
                Payments = new List<PaymentDto>(),
                TotalPaid = 0,
                RemainingAmount = order.TotalAmount
            };
        }
    }
}