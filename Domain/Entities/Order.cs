using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Order
    {
        public int OrderId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public DateTime? DeliveryDate { get; private set; }
        public string? DeliveryTimeSlot { get; private set; } // مثل: "10:00 AM - 12:00 PM"
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; }

        // Foreign Keys
        public int UserId { get; private set; }
        public int AddressId { get; private set; }

        // Navigation Properties
        public User User { get; private set; }
        public Address Address { get; private set; }
        public ICollection<OrderDetail> OrderDetails { get; private set; } = new List<OrderDetail>();
        public ICollection<Payment> Payments { get; private set; } = new List<Payment>();

#pragma warning disable CS8618
        private Order() { }
#pragma warning restore CS8618

        private Order(int userId, int addressId, DateTime? deliveryDate, string? deliveryTimeSlot)
        {
            UserId = userId;
            AddressId = addressId;
            OrderDate = DateTime.UtcNow;
            DeliveryDate = deliveryDate;
            DeliveryTimeSlot = deliveryTimeSlot;
            Status = OrderStatus.Pending;
            TotalAmount = 0;
        }

        // Factory Method
        public static Order Create(int userId, int addressId, DateTime? deliveryDate = null, string? deliveryTimeSlot = null)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be valid.", nameof(userId));

            if (addressId <= 0)
                throw new ArgumentException("Address ID must be valid.", nameof(addressId));

            return new Order(userId, addressId, deliveryDate, deliveryTimeSlot);
        }

        // Business Logic Methods
        public void AddOrderItem(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            if (product.QuantityInStock < quantity)
                throw new InvalidOperationException($"Not enough stock for product {product.ProductName}. Available: {product.QuantityInStock}");

            var priceAtOrder = product.Price;

            // Apply offer discount if exists
            if (product.OfferID.HasValue && product.Offer != null)
            {
                priceAtOrder = product.Price * (1 - product.Offer.DiscountPercentage / 100);
            }

            var orderDetail = OrderDetail.Create(quantity, priceAtOrder, product.ProductID);
            OrderDetails.Add(orderDetail);

            RecalculateTotalAmount();
        }

        public void RemoveOrderItem(int orderDetailId)
        {
            var item = OrderDetails.FirstOrDefault(x => x.OrderDetailId == orderDetailId);
            if (item != null)
            {
                OrderDetails.Remove(item);
                RecalculateTotalAmount();
            }
        }

        public void RecalculateTotalAmount()
        {
            TotalAmount = OrderDetails.Sum(item => item.Subtotal);
        }

        public void ConfirmOrder()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be confirmed.");

            if (!OrderDetails.Any())
                throw new InvalidOperationException("Cannot confirm an empty order.");

            Status = OrderStatus.InPreparation;
        }

        public void StartDelivery()
        {
            if (Status != OrderStatus.InPreparation)
                throw new InvalidOperationException("Order must be in preparation before delivery.");

            Status = OrderStatus.OutForDelivery;
        }

        public void CompleteDelivery()
        {
            if (Status != OrderStatus.OutForDelivery)
                throw new InvalidOperationException("Order must be out for delivery to complete.");

            Status = OrderStatus.Delivered;
        }

        public void CancelOrder()
        {
            if (Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Cannot cancel a delivered order.");

            Status = OrderStatus.Canceled;
        }

        public void SetDeliveryDetails(DateTime deliveryDate, string? timeSlot)
        {
            if (deliveryDate < DateTime.UtcNow.Date)
                throw new ArgumentException("Delivery date cannot be in the past.");

            DeliveryDate = deliveryDate;
            DeliveryTimeSlot = timeSlot;
        }

        public void AddPayment(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            Payments.Add(payment);
        }
    }
}