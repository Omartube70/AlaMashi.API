using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Dtos
{
    public class DailyReportDto
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
        public int DeliveredOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CanceledOrders { get; set; }
    }
}
