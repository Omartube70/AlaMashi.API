using Application.Dashboard.Dtos;

namespace Application.Interfaces
{
    public interface IDashboardService
    {
        Task<EnhancedDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
    }
}