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
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId,true);

            if (order == null)
                throw new NotFoundException($"Order with ID {request.OrderId} not found.");

            // تحديث الحالة بناءً على القيمة المطلوبة
            switch (request.NewStatus)
            {
                case "InPreparation":
                    order.ConfirmOrder();
                    break;
                case "OutForDelivery":
                    order.StartDelivery();
                    break;
                case "Delivered":
                    order.CompleteDelivery();
                    break;
                case "Canceled":
                    order.CancelOrder();
                    break;
                default:
                    throw new ArgumentException($"Invalid status: {request.NewStatus}");
            }

            await _orderRepository.UpdateOrderAsync(order);

            return Unit.Value;
        }
    }
}
