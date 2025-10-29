using Application.Dashboard.Dtos;
using Application.Dashboard.Queries;
using Application.Interfaces;
using Domain.Common;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore; 
using System.Linq;

namespace Infrastructure.Repositories
{ 
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public DashboardService(ApplicationDbContext context , IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<EnhancedDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            // البيانات الأساسية
            var totalUsers = await _context.Users.CountAsync(cancellationToken);
            var totalProducts = await _context.Products.CountAsync(cancellationToken);
            var totalCategories = await _context.Categories.CountAsync(cancellationToken);
            var totalActiveOffers = await _context.Offers
                .CountAsync(o => o.EndDate >= now && o.StartDate <= now, cancellationToken);

            // بيانات الطلبات
            var totalOrders = await _context.Orders.CountAsync(cancellationToken);
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending, cancellationToken);

            // الإيرادات
            var revenueAnalysis = await _mediator.Send(new GetRevenueAnalysisQuery(), cancellationToken);
            var orderStatusSummary = await _mediator.Send(new GetOrderStatusSummaryQuery(), cancellationToken);
            var topProducts = await _mediator.Send(new GetTopSellingProductsQuery { TopCount = 5 }, cancellationToken);

            return new EnhancedDashboardSummaryDto
            {
                TotalUsers = totalUsers,
                TotalProducts = totalProducts,
                TotalCategories = totalCategories,
                TotalActiveOffers = totalActiveOffers,
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                TodayRevenue = revenueAnalysis.TodayRevenue,
                MonthRevenue = revenueAnalysis.ThisMonthRevenue,
                RevenueAnalysis = revenueAnalysis,
                OrderStatusSummary = orderStatusSummary,
                TopSellingProducts = topProducts
            };
        }
    }
}
