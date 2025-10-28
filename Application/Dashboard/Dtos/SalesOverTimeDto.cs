namespace Application.Dashboard.Dtos
{
    public class SalesOverTimeDto
    {
        public string Period { get; set; } 
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
    }
}