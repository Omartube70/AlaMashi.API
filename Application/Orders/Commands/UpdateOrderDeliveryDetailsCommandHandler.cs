using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class UpdateOrderDeliveryDetailsCommandHandler : IRequestHandler<UpdateOrderDeliveryDetailsCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public UpdateOrderDeliveryDetailsCommandHandler(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(UpdateOrderDeliveryDetailsCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب الطلب
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId, includeDetails: true);

            if (order == null)
                throw new NotFoundException($"Order with ID {request.OrderId} not found.");

            // 2. التحقق من أن الطلب قابل للتعديل (يجب أن يكون في حالة Pending أو InPreparation فقط)
            if (order.Status != Domain.Common.OrderStatus.Pending &&
                order.Status != Domain.Common.OrderStatus.InPreparation)
                throw new InvalidOperationException(
                    "Can only update delivery details for orders in Pending or InPreparation status.");

            // 3. تحديث تاريخ التوصيل إذا تم توفيره
            if (request.NewDeliveryDate.HasValue)
            {
                order.SetDeliveryDetails(request.NewDeliveryDate.Value, request.NewDeliveryTimeSlot ?? order.DeliveryTimeSlot);
            }

            // 4. تحديث العنوان إذا تم توفيره
            if (request.NewAddressId.HasValue)
            {
                // التحقق من أن العنوان موجود ويخص نفس المستخدم
                var user = await _userRepository.GetUserWithAddressesAsync(order.UserId);

                if (user == null)
                    throw new UserNotFoundException(order.UserId);

                var newAddress = user.Addresses.FirstOrDefault(a => a.AddressId == request.NewAddressId.Value);

                if (newAddress == null)
                    throw new NotFoundException(
                        $"Address with ID {request.NewAddressId.Value} not found for this user.");

                // تحديث العنوان في الطلب
                order.UpdateAddress(request.NewAddressId.Value);
            }

            // 5. حفظ التحديثات
            await _orderRepository.UpdateOrderAsync(order);

            return Unit.Value;
        }
    }
}
