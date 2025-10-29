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
    public class GetDailyReportQueryHandler : IRequestHandler<GetDailyReportQuery, DailyReportDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetDailyReportQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<DailyReportDto> Handle(GetDailyReportQuery request, CancellationToken cancellationToken)
        {
            var startOfDay = request.Date.Date;
            var endOfDay = startOfDay.AddDays(1);

            var orders = await _orderRepository.GetOrdersDailyReport(startOfDay, endOfDay, cancellationToken);

            return new DailyReportDto
            {
                Date = request.Date.Date,
                TotalSales = orders.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.TotalAmount),
                OrderCount = orders.Count,
                DeliveredOrders = orders.Count(o => o.Status == OrderStatus.Delivered),
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
                CanceledOrders = orders.Count(o => o.Status == OrderStatus.Canceled)
            };
        }
    }
}
