using System;

namespace Domain.Entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; private set; }
        public int Quantity { get; private set; }
        public decimal PriceAtOrder { get; private set; } // السعر وقت الطلب
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

        private OrderDetail(int orderId, int productId, int quantity, decimal priceAtOrder)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            PriceAtOrder = priceAtOrder;
            Subtotal = quantity * priceAtOrder;
        }

        public static OrderDetail Create(int orderId, int productId, int quantity, decimal priceAtOrder)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid order ID.", nameof(orderId));

            if (productId <= 0)
                throw new ArgumentException("Invalid product ID.", nameof(productId));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            if (priceAtOrder <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(priceAtOrder));

            return new OrderDetail(orderId, productId, quantity, priceAtOrder);
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

            Quantity = newQuantity;
            Subtotal = Quantity * PriceAtOrder;
        }
    }
}