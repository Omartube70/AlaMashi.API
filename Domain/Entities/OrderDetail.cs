using System;

namespace Domain.Entities
{
    /// <summary>
    /// يمثل تفاصيل منتج واحد داخل الطلب
    /// </summary>
    public class OrderDetail
    {
        public int OrderDetailId { get; private set; }
        public int Quantity { get; private set; }
        public decimal PriceAtOrder { get; private set; } // السعر بعد الخصم
        public decimal? OriginalPriceAtOrder { get; private set; } // السعر الأصلي قبل الخصم
        public decimal Subtotal { get; private set; }

        // Foreign Keys
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }

        // Navigation Properties
        public Order Order { get; private set; }
        public Product Product { get; private set; }

#pragma warning disable CS8618
        private OrderDetail() { }
#pragma warning restore CS8618

        private OrderDetail(int quantity, decimal priceAtOrder, int productId, decimal? originalPriceAtOrder = null)
        {
            Quantity = quantity;
            PriceAtOrder = priceAtOrder;
            OriginalPriceAtOrder = originalPriceAtOrder;
            ProductId = productId;
            Subtotal = quantity * priceAtOrder;
        }

        // Factory Method
        public static OrderDetail Create(int quantity, decimal priceAtOrder, int productId, decimal? originalPriceAtOrder = null)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            if (priceAtOrder <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(priceAtOrder));

            if (productId <= 0)
                throw new ArgumentException("Product ID must be valid.", nameof(productId));

            return new OrderDetail(quantity, priceAtOrder, productId, originalPriceAtOrder);
        }

        // Business Logic Methods
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

            Quantity = newQuantity;
            Subtotal = Quantity * PriceAtOrder;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(newPrice));

            PriceAtOrder = newPrice;
            Subtotal = Quantity * PriceAtOrder;
        }

        // Helper Methods
        public decimal CalculateSubtotal()
        {
            return Quantity * PriceAtOrder;
        }

        public void RecalculateSubtotal()
        {
            Subtotal = CalculateSubtotal();
        }
    }
}