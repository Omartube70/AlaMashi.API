using Application.Dashboard.Dtos;
using Application.Interfaces;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Queries
{
    public class GetOrderStatusSummaryQueryHandler : IRequestHandler<GetOrderStatusSummaryQuery, OrderStatusSummaryDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderStatusSummaryQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderStatusSummaryDto> Handle(GetOrderStatusSummaryQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            return new OrderStatusSummaryDto
            {
                PendingCount = orders.Count(o => o.Status == OrderStatus.Pending),
                InPreparationCount = orders.Count(o => o.Status == OrderStatus.InPreparation),
                OutForDeliveryCount = orders.Count(o => o.Status == OrderStatus.OutForDelivery),
                DeliveredCount = orders.Count(o => o.Status == OrderStatus.Delivered),
                CanceledCount = orders.Count(o => o.Status == OrderStatus.Canceled),
                TotalOrders = orders.Count
            };
        }
    }
}
