using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Dtos
{
    public class MonthlyReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
   
