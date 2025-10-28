using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Payment
    {
        public int PaymentId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public string? TransactionId { get; private set; }

        // Foreign Key
        public int OrderId { get; private set; }

        // Navigation Property
        public Order Order { get; private set; }

#pragma warning disable CS8618
        private Payment() { }
#pragma warning restore CS8618

        private Payment(int orderId, decimal amount, PaymentMethod paymentMethod, PaymentStatus paymentStatus)
        {
            OrderId = orderId;
            Amount = amount;
            PaymentDate = DateTime.UtcNow;
            PaymentMethod = paymentMethod;
            PaymentStatus = paymentStatus;
        }

        public static Payment Create(int orderId, decimal amount, PaymentMethod paymentMethod)
        {
            if (orderId <= 0)
                throw new ArgumentException("Invalid order ID.", nameof(orderId));

            if (amount <= 0)
                throw new ArgumentException("Payment amount must be greater than zero.", nameof(amount));

            return new Payment(orderId, amount, paymentMethod, PaymentStatus.Pending);
        }

        public void MarkAsCompleted(string? transactionId = null)
        {
            PaymentStatus = PaymentStatus.Completed;
            TransactionId = transactionId;
        }

        public void MarkAsFailed()
        {
            PaymentStatus = PaymentStatus.Failed;
        }

        public void MarkAsCanceled()
        {
            PaymentStatus = PaymentStatus.Canceled;
        }
    }
}