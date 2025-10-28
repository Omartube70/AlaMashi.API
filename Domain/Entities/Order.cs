using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order
    {
        public int OrderId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public DateTime? DeliveryDate { get; private set; }
        public string? DeliveryTimeSlot { get; private set; }
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

        private Order(DateTime orderDate, int userId, int addressId, OrderStatus status)
        {
            OrderDate = orderDate;
            UserId = userId;
            AddressId = addressId;
            Status = status;
            TotalAmount = 0;
        }

        public static Order Create(int userId, int addressId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.", nameof(userId));

            if (addressId <= 0)
                throw new ArgumentException("Invalid address ID.", nameof(addressId));

            var defaultStatus = OrderStatus.Pending;

            return new Order(DateTime.UtcNow, userId, addressId, defaultStatus);
        }

        public void AddOrderDetail(OrderDetail detail)
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail));

            OrderDetails.Add(detail);
            RecalculateTotalAmount();
        }

        public void RemoveOrderDetail(OrderDetail detail)
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail));

            OrderDetails.Remove(detail);
            RecalculateTotalAmount();
        }

        public void RecalculateTotalAmount()
        {
            TotalAmount = 0;
            foreach (var detail in OrderDetails)
            {
                TotalAmount += detail.Subtotal;
            }
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }

        public void SetDeliveryDetails(DateTime deliveryDate, string deliveryTimeSlot)
        {
            if (string.IsNullOrWhiteSpace(deliveryTimeSlot))
                throw new ArgumentException("Delivery time slot is required.", nameof(deliveryTimeSlot));

            DeliveryDate = deliveryDate;
            DeliveryTimeSlot = deliveryTimeSlot;
        }

        public void AddPayment(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment));

            Payments.Add(payment);
        }

        public decimal GetTotalPaid()
        {
            decimal totalPaid = 0;
            foreach (var payment in Payments)
            {
                if (payment.PaymentStatus == PaymentStatus.Completed)
                {
                    totalPaid += payment.Amount;
                }
            }
            return totalPaid;
        }

        public decimal GetRemainingAmount()
        {
            return TotalAmount - GetTotalPaid();
        }

        public bool IsFullyPaid()
        {
            return GetRemainingAmount() <= 0;
        }
    }
}