using System;
using Domain.Entities;

namespace Domain.Entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; private set; }
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal PriceAtOrder { get; private set; }
        public decimal Subtotal { get; private set; }

        public Order Order { get; private set; }
        public Product Product { get; private set; }

#pragma warning disable CS8618
        private OrderDetail() { }
#pragma warning restore CS8618

        private OrderDetail(int quantity, decimal priceAtOrder, int productId)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            if (priceAtOrder <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(priceAtOrder));

            Quantity = quantity;
            PriceAtOrder = priceAtOrder;
            ProductId = productId;
            Subtotal = quantity * priceAtOrder;
        }

        // Factory Method
        public static OrderDetail Create(int quantity, decimal priceAtOrder, int productId)
        {
            return new OrderDetail(quantity, priceAtOrder, productId);
        }

        // Business Logic
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            Quantity = newQuantity;
            Subtotal = newQuantity * PriceAtOrder;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            PriceAtOrder = newPrice;
            Subtotal = Quantity * newPrice;
        }
    }
}
