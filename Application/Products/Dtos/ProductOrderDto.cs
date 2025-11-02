namespace Application.Products.Dtos
{
    public class ProductOrderDto
    {
        public int OrderId { get; init; }
        public int Quantity { get; init; }
        public DateTime OrderDate { get; init; }
        public decimal Subtotal { get; init; }
        public string CustomerName { get; init; }
    }
}