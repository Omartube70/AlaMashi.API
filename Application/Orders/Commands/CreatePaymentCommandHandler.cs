using Application.Orders.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Orders.Commands
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;

        public CreatePaymentCommandHandler(
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب الأوردر
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);
            if (order == null)
                throw new NotFoundException($"Order with ID {request.OrderId} not found.");

            // 2. تحقق من الصلاحيات
            if (order.UserId != request.CurrentUserId)
                throw new ForbiddenAccessException();

            // 3. تحقق من المبلغ
            if (request.Amount <= 0)
                throw new ArgumentException("Payment amount must be greater than zero.");

            if (request.Amount > order.GetRemainingAmount())
                throw new ArgumentException("Payment amount exceeds remaining order amount.");

            // 4. إنشاء الدفعة
            if (!Enum.TryParse<PaymentMethod>(request.PaymentMethod, true, out var paymentMethod))
                throw new ArgumentException("Invalid payment method.");

            var payment = Payment.Create(order.OrderId, request.Amount, paymentMethod);

            // 5. حفظ الدفعة
            await _paymentRepository.AddPaymentAsync(payment);

            // 6. تحديث حالة الدفع لو اكتمل
            payment.MarkAsCompleted(); // أو حسب منطق الدفع بتاعك
            await _paymentRepository.UpdatePaymentAsync(payment);

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