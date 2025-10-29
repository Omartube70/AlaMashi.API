using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Payment
    {
        public int PaymentId { get; private set; }
        public int OrderId { get; private set; } // FK صح
        public decimal Amount { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public string? TransactionId { get; private set; }

        // Navigation Property
        public Order Order { get; private set; }

#pragma warning disable CS8618
        private Payment() { }
#pragma warning restore CS8618

        private Payment(int orderId, decimal amount, PaymentMethod method, string? transactionId = null)
        {
            if (orderId <= 0)
                throw new ArgumentException("Order ID must be valid.", nameof(orderId));

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            OrderId = orderId;
            Amount = amount;
            PaymentDate = DateTime.UtcNow;
            PaymentMethod = method;
            TransactionId = transactionId;
            PaymentStatus = PaymentStatus.Pending;
        }

        // ✅ Factory Method الصحيح
        public static Payment Create(int orderId, decimal amount, PaymentMethod method, string? transactionId = null)
        {
            return new Payment(orderId, amount, method, transactionId);
        }

        // Business Logic
        public void MarkAsCompleted(string? transactionId = null)
        {
            if (PaymentStatus == PaymentStatus.Completed)
                throw new InvalidOperationException("Payment is already completed.");

            if (!string.IsNullOrEmpty(transactionId))
                 TransactionId = transactionId;

            PaymentStatus = PaymentStatus.Completed;
        }

        public void MarkAsFailed()
        {
            if (PaymentStatus == PaymentStatus.Failed)
                throw new InvalidOperationException("Payment is already failed.");

            PaymentStatus = PaymentStatus.Failed;
        }
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }
}
