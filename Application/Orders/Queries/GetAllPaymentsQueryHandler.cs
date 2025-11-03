using Application.Interfaces;
using Application.Orders.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Orders.Queries
{
    public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, IReadOnlyList<PaymentDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetAllPaymentsQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            // جلب كل الدفعات (ممكن مع فلترة بـ OrderId)
            var payments = await _orderRepository.GetAllPaymentsAsync(request.OrderId, cancellationToken);

            var paymentDtos = _mapper.Map<IReadOnlyList<PaymentDto>>(payments);

            return paymentDtos;
        }
    }
}
