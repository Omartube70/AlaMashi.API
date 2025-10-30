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
    public class GetRevenueAnalysisQueryHandler : IRequestHandler<GetRevenueAnalysisQuery, RevenueAnalysisDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetRevenueAnalysisQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<RevenueAnalysisDto> Handle(GetRevenueAnalysisQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.Now; // استخدم التوقيت المحلي بدل UTC
            var todayStart = now.Date;
            var todayEnd = todayStart.AddDays(1);
            var weekStart = now.Date.AddDays(-(int)now.DayOfWeek);
            var weekEnd = weekStart.AddDays(7);
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var monthEnd = monthStart.AddMonths(1);
            var yearStart = new DateTime(now.Year, 1, 1);
            var yearEnd = yearStart.AddYears(1);

            var deliveredOrders = await _orderRepository.GetOrdersDeliveredAsync(cancellationToken);

            return new RevenueAnalysisDto
            {
                TodayRevenue = deliveredOrders.Where(o => o.OrderDate >= todayStart && o.OrderDate < todayEnd).Sum(o => o.TotalAmount),
                ThisWeekRevenue = deliveredOrders.Where(o => o.OrderDate >= weekStart && o.OrderDate < weekEnd).Sum(o => o.TotalAmount),
                ThisMonthRevenue = deliveredOrders.Where(o => o.OrderDate >= monthStart && o.OrderDate < monthEnd).Sum(o => o.TotalAmount),
                ThisYearRevenue = deliveredOrders.Where(o => o.OrderDate >= yearStart && o.OrderDate < yearEnd).Sum(o => o.TotalAmount),
                TotalRevenue = deliveredOrders.Sum(o => o.TotalAmount)
            };
        }
    }
}