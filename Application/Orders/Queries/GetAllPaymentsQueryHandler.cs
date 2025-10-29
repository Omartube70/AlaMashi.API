using Application.Interfaces;
using Application.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders.Queries
{
    public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, IReadOnlyList<PaymentDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllPaymentsQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IReadOnlyList<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            // جلب جميع الدفعات من Database (مع filtration اختياري حسب OrderId)
            var payments = await _orderRepository.GetAllPaymentsAsync(request.OrderId, cancellationToken);

            // تحويل من Payment Entity إلى PaymentDto
            var paymentDtos = payments
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    PaymentMethod = p.PaymentMethod.ToString(),
                    PaymentStatus = p.PaymentStatus.ToString(),
                    TransactionId = p.TransactionId
                })
                .ToList();

            return paymentDtos;
        }
    }
}