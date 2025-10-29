using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Dtos
{
    public class EnhancedDashboardSummaryDto
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalActiveOffers { get; set; }

        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public decimal TodayRevenue { get; set; }
        public decimal MonthRevenue { get; set; }

        public RevenueAnalysisDto RevenueAnalysis { get; set; }
        public OrderStatusSummaryDto OrderStatusSummary { get; set; }
        public List<TopProductDto> TopSellingProducts { get; set; }
    }
}
