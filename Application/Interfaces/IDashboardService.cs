using Application.Dashboard.Dtos;

namespace Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
    }
}