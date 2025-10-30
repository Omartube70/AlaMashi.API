using Application.Addresses.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Orders.Dtos;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _mapper = mapper;
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

                // Calculate prices (with or without discount)
                decimal originalPrice = product.Price;
                decimal finalPrice = product.Price;
                decimal? originalPriceAtOrder = null;

                // If product has an offer, apply discount
                if (product.OfferID.HasValue && product.Offer != null)
                {
                    var now = DateTime.Now;
                    if (now >= product.Offer.StartDate && now <= product.Offer.EndDate)
                    {
                        originalPriceAtOrder = originalPrice;
                        finalPrice = originalPrice * (1 - product.Offer.DiscountPercentage);
                    }
                }

                // Create order detail with original price if there's a discount
                var orderDetail = OrderDetail.Create(
                    quantity: item.Quantity,
                    priceAtOrder: finalPrice,
                    productId: product.ProductID,
                    originalPriceAtOrder: originalPriceAtOrder
                );

                order.AddOrderDetail(orderDetail);

                // تقليل الكمية من المخزون
                product.RemoveStock(item.Quantity);
                await _productRepository.UpdateProductAsync(product);
            }

            // حفظ الطلب
            await _orderRepository.AddOrderAsync(order);

            // إرجاع DTO باستخدام AutoMapper
            return _mapper.Map<OrderDto>(order);
        }
    }
}
