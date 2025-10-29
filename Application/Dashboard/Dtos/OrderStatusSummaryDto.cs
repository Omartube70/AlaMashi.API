using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Dtos
{
    public class OrderStatusSummaryDto
    {
        public int PendingCount { get; set; }
        public int InPreparationCount { get; set; }
        public int OutForDeliveryCount { get; set; }
        public int DeliveredCount { get; set; }
        public int CanceledCount { get; set; }
        public int TotalOrders { get; set; }
    }
}
