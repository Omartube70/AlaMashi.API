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
    public class GetMonthlyReportQueryHandler : IRequestHandler<GetMonthlyReportQuery, MonthlyReportDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetMonthlyReportQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<MonthlyReportDto> Handle(GetMonthlyReportQuery request, CancellationToken cancellationToken)
        {
            var startDate = new DateTime(request.Year, request.Month, 1);
            var endDate = startDate.AddMonths(1);

            var sales = await _orderRepository.GetMonthlySalesAsync(request.Year, request.Month);

            var totalSales = sales.Values.Sum();
            var orderCount = sales.Count;

            return new MonthlyReportDto
            {
                Year = request.Year,
                Month = request.Month,
                MonthName = new DateTime(request.Year, request.Month, 1).ToString("MMMM"),
                TotalSales = totalSales,
                OrderCount = orderCount,
                AverageOrderValue = orderCount > 0 ? totalSales / orderCount : 0
            };
        }
    }
}
