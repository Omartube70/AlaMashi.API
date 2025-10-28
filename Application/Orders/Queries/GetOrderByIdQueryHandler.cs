using Application.Orders.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Orders.Queries
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId);
            if (order == null)
                throw new NotFoundException($"Order with ID {request.OrderId} not found.");

            // تحقق من الصلاحيات
            bool isAdmin = request.CurrentUserRole == "Admin";
            bool isOwner = order.UserId == request.CurrentUserId;

            if (!isAdmin && !isOwner)
                throw new ForbiddenAccessException();

            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                DeliveryTimeSlot = order.DeliveryTimeSlot,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                UserId = order.UserId,
                UserName = order.User.UserName,
                AddressId = order.AddressId,
                AddressDetails = $"{order.Address.Street}, {order.Address.City}",
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    ProductName = od.Product.ProductName,
                    Quantity = od.Quantity,
                    PriceAtOrder = od.PriceAtOrder,
                    Subtotal = od.Subtotal
                }).ToList(),
                Payments = order.Payments.Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    PaymentMethod = p.PaymentMethod.ToString(),
                    PaymentStatus = p.PaymentStatus.ToString(),
                    TransactionId = p.TransactionId
                }).ToList(),
                TotalPaid = order.GetTotalPaid(),
                RemainingAmount = order.GetRemainingAmount()
            };
        }
    }
}