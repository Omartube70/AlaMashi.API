using Application.Dashboard.Dtos;
using Application.Dashboard.Queries;
using Application.Interfaces;    
using MediatR;

namespace Application.Dashboard.Queries
{
    public class GetMonthlySalesQueryHandler : IRequestHandler<GetMonthlySalesQuery, List<SalesOverTimeDto>>
    {
        // Optional: Inject IDashboardService if you add GetMonthlySalesAsync to it
        // private readonly IDashboardService _dashboardService;

        // public GetMonthlySalesQueryHandler(IDashboardService dashboardService)
        // {
        //     _dashboardService = dashboardService;
        // }

        public async Task<List<SalesOverTimeDto>> Handle(GetMonthlySalesQuery request, CancellationToken cancellationToken)
        {
            // --- Uncomment this if you add GetMonthlySalesAsync to IDashboardService ---
            /*
            var salesData = await _dashboardService.GetMonthlySalesAsync(request.Months, cancellationToken);
            return salesData;
            */

            // --- Dummy Data (Current implementation) ---
            await Task.Delay(100, cancellationToken); // Simulate async work
            var dummySales = new List<SalesOverTimeDto> {
                new SalesOverTimeDto { Period = "2025-08", TotalSales = 1200.50m, OrderCount = 15 },
                new SalesOverTimeDto { Period = "2025-09", TotalSales = 1500.75m, OrderCount = 20 },
                new SalesOverTimeDto { Period = "2025-10", TotalSales = 1350.00m, OrderCount = 18 },
            };
            return dummySales;
        }
    }
}