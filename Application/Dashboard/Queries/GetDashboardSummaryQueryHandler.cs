using Application.Dashboard.Dtos; 
using Application.Interfaces;  
using MediatR;

namespace Application.Dashboard.Queries
{
        public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
        {
            private readonly IDashboardService _dashboardService;

            public GetDashboardSummaryQueryHandler(IDashboardService dashboardService)
            {
                _dashboardService = dashboardService;
            }

            public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
            {
                var summary = await _dashboardService.GetDashboardSummaryAsync(cancellationToken);
                return summary;
            }
        }
}

