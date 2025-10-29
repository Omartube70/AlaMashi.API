using Application.Exceptions;
using Application.Interfaces;
using Application.Orders.Dtos;
using Domain.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
    {
        private readonly IOrderRepository _orderRepository;

        public CreatePaymentCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId, includeDetails: true);

            if (order == null)
                throw new NotFoundException($"Order with ID {request.OrderId} not found.");

            if (!Enum.TryParse<PaymentMethod>(request.PaymentMethod, out var paymentMethod))
                throw new ArgumentException("Invalid payment method.");

            var payment = Payment.Create(order.OrderId,request.Amount, paymentMethod);

            if (!string.IsNullOrEmpty(request.TransactionId))
            {
                payment.MarkAsCompleted(request.TransactionId);
            }

            order.AddPayment(payment);
            await _orderRepository.UpdateOrderAsync(order);

            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod.ToString(),
                PaymentStatus = payment.PaymentStatus.ToString(),
                TransactionId = payment.TransactionId
            };
        }
    }

}
