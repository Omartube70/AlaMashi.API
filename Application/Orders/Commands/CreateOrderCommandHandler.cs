using Application.Addresses.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Orders.Dtos;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // التحقق من وجود المستخدم
            var user = await _userRepository.GetUserWithAddressesAsync(request.CurrentUserId);
            if (user == null)
                throw new NotFoundException($"User with ID {request.CurrentUserId} not found.");

            // التحقق من وجود العنوان
            var address = user.Addresses.FirstOrDefault(a => a.AddressId == request.AddressId);
            if (address == null)
                throw new NotFoundException($"Address with ID {request.AddressId} not found for this user.");

            // إنشاء الطلب
            var order = Order.Create(
                request.CurrentUserId,
                request.AddressId,
                request.DeliveryDate,
                request.DeliveryTimeSlot
            );

            // إضافة المنتجات للطلب
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product == null)
                    throw new NotFoundException($"Product with ID {item.ProductId} not found.");

                order.AddOrderItem(product, item.Quantity);

                // تقليل الكمية من المخزون
                product.RemoveStock(item.Quantity);
                await _productRepository.UpdateProductAsync(product);
            }

            // حفظ الطلب
            await _orderRepository.AddOrderAsync(order);

            // إرجاع DTO
            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                DeliveryTimeSlot = order.DeliveryTimeSlot,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                UserId = user.UserID,
                UserName = user.UserName,
                UserPhone = user.Phone,
                Address = new AddressDto
                {
                    AddressId = address.AddressId,
                    Street = address.Street,
                    City = address.City,
                    AddressDetails = address.AddressDetails,
                    AddressType = address.AddressType.ToString(),
                    UserId = address.UserId
                },
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    ProductName = od.Product.ProductName,
                    Quantity = od.Quantity,
                    PriceAtOrder = od.PriceAtOrder,
                    Subtotal = od.Subtotal
                }).ToList()
            };
        }
    }
}
