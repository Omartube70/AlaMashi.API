using Application.Dashboard.Dtos;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Queries
{
    public class GetEnhancedDashboardSummaryQueryHandler : IRequestHandler<GetEnhancedDashboardSummaryQuery, EnhancedDashboardSummaryDto>
    {
        private readonly IDashboardService _dashboardService;


        public GetEnhancedDashboardSummaryQueryHandler(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<EnhancedDashboardSummaryDto> Handle(GetEnhancedDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
        
            var summaryDto = await _dashboardService.GetDashboardSummaryAsync();

            return summaryDto;
        }
    }
}
