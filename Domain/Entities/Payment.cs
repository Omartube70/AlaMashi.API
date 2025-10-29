using Domain.Common;
using System;

namespace Domain.Entities
{
    /// <summary>
    /// كيان الدفع - يمثل عملية دفع مرتبطة بطلب معين
    /// </summary>
    public class Payment
    {
        // ==================== Properties ====================

        /// <summary>
        /// معرف الدفعة الفريد
        /// </summary>
        public int PaymentId { get; private set; }

        /// <summary>
        /// المبلغ المدفوع
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// تاريخ ووقت الدفع
        /// </summary>
        public DateTime PaymentDate { get; private set; }

        /// <summary>
        /// طريقة الدفع (نقدي، بطاقة ائتمان، محفظة إلكترونية، إلخ)
        /// </summary>
        public PaymentMethod PaymentMethod { get; private set; }

        /// <summary>
        /// حالة الدفع (قيد الانتظار، مكتمل، فاشل، ملغي)
        /// </summary>
        public PaymentStatus PaymentStatus { get; private set; }

        /// <summary>
        /// معرف المعاملة من بوابة الدفع (اختياري)
        /// </summary>
        public string? TransactionId { get; private set; }

        // ==================== Foreign Keys ====================

        /// <summary>
        /// معرف الطلب المرتبط بهذه الدفعة
        /// </summary>
        public int OrderId { get; private set; }

        // ==================== Navigation Properties ====================

        /// <summary>
        /// الطلب المرتبط بهذه الدفعة
        /// </summary>
        public Order Order { get; private set; }

        // ==================== Constructors ====================

#pragma warning disable CS8618
        /// <summary>
        /// Constructor خاص لـ Entity Framework
        /// </summary>
        private Payment() { }
#pragma warning restore CS8618

        /// <summary>
        /// Constructor خاص لإنشاء دفعة جديدة
        /// </summary>
        private Payment(decimal amount, PaymentMethod method, int orderId)
        {
            Amount = amount;
            PaymentMethod = method;
            OrderId = orderId;
            PaymentDate = DateTime.UtcNow;
            PaymentStatus = PaymentStatus.Pending;
        }

        // ==================== Factory Method ====================

        /// <summary>
        /// إنشاء دفعة جديدة
        /// </summary>
        /// <param name="amount">المبلغ المراد دفعه</param>
        /// <param name="method">طريقة الدفع</param>
        /// <param name="orderId">معرف الطلب</param>
        /// <returns>كائن Payment جديد</returns>
        /// <exception cref="ArgumentException">إذا كان المبلغ أقل من أو يساوي صفر</exception>
        public static Payment Create(decimal amount, PaymentMethod method, int orderId)
        {
            // التحقق من صحة المبلغ
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

            // التحقق من صحة معرف الطلب
            if (orderId <= 0)
                throw new ArgumentException("Order ID must be valid.", nameof(orderId));

            return new Payment(amount, method, orderId);
        }

        // ==================== Status Management Methods ====================

        /// <summary>
        /// تحديد الدفعة كمكتملة بنجاح
        /// </summary>
        /// <param name="transactionId">معرف المعاملة من بوابة الدفع (اختياري)</param>
        /// <exception cref="InvalidOperationException">إذا كانت الدفعة مكتملة بالفعل</exception>
        public void MarkAsCompleted(string? transactionId = null)
        {
            if (PaymentStatus == PaymentStatus.Completed)
                throw new InvalidOperationException("Payment is already completed.");

            PaymentStatus = PaymentStatus.Completed;
            TransactionId = transactionId;
        }

        /// <summary>
        /// تحديد الدفعة كفاشلة
        /// </summary>
        /// <exception cref="InvalidOperationException">إذا كانت الدفعة مكتملة</exception>
        public void MarkAsFailed()
        {
            if (PaymentStatus == PaymentStatus.Completed)
                throw new InvalidOperationException("Cannot mark completed payment as failed.");

            PaymentStatus = PaymentStatus.Failed;
        }

        /// <summary>
        /// إلغاء الدفعة
        /// </summary>
        /// <exception cref="InvalidOperationException">إذا كانت الدفعة مكتملة</exception>
        public void MarkAsCanceled()
        {
            if (PaymentStatus == PaymentStatus.Completed)
                throw new InvalidOperationException("Cannot cancel completed payment.");

            PaymentStatus = PaymentStatus.Canceled;
        }

        /// <summary>
        /// إعادة محاولة الدفعة الفاشلة
        /// </summary>
        /// <exception cref="InvalidOperationException">إذا لم تكن الدفعة فاشلة</exception>
        public void RetryPayment()
        {
            if (PaymentStatus != PaymentStatus.Failed)
                throw new InvalidOperationException("Only failed payments can be retried.");

            PaymentStatus = PaymentStatus.Pending;
            PaymentDate = DateTime.UtcNow;
            TransactionId = null;
        }

        // ==================== Data Update Methods ====================

        /// <summary>
        /// تحديث معرف المعاملة
        /// </summary>
        /// <param name="transactionId">معرف المعاملة الجديد</param>
        /// <exception cref="ArgumentException">إذا كان المعرف فارغاً</exception>
        public void UpdateTransactionId(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentException("Transaction ID cannot be empty.", nameof(transactionId));

            TransactionId = transactionId;
        }

        /// <summary>
        /// تحديث المبلغ المدفوع (فقط للدفعات قيد الانتظار)
        /// </summary>
        /// <param name="newAmount">المبلغ الجديد</param>
        /// <exception cref="ArgumentException">إذا كان المبلغ أقل من أو يساوي صفر</exception>
        /// <exception cref="InvalidOperationException">إذا لم تكن الدفعة قيد الانتظار</exception>
        public void UpdateAmount(decimal newAmount)
        {
            if (newAmount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(newAmount));

            if (PaymentStatus != PaymentStatus.Pending)
                throw new InvalidOperationException("Can only update amount for pending payments.");

            Amount = newAmount;
        }

        // ==================== Business Logic & Validation ====================

        /// <summary>
        /// التحقق من إمكانية تعديل الدفعة
        /// </summary>
        /// <returns>true إذا كانت الدفعة قابلة للتعديل</returns>
        public bool CanBeModified()
        {
            return PaymentStatus == PaymentStatus.Pending;
        }

        /// <summary>
        /// التحقق من نجاح الدفعة
        /// </summary>
        /// <returns>true إذا كانت الدفعة مكتملة بنجاح</returns>
        public bool IsSuccessful()
        {
            return PaymentStatus == PaymentStatus.Completed;
        }

        /// <summary>
        /// التحقق من فشل الدفعة
        /// </summary>
        /// <returns>true إذا كانت الدفعة فاشلة</returns>
        public bool IsFailed()
        {
            return PaymentStatus == PaymentStatus.Failed;
        }

        /// <summary>
        /// التحقق من إلغاء الدفعة
        /// </summary>
        /// <returns>true إذا كانت الدفعة ملغاة</returns>
        public bool IsCanceled()
        {
            return PaymentStatus == PaymentStatus.Canceled;
        }

        /// <summary>
        /// التحقق من أن الدفعة قيد الانتظار
        /// </summary>
        /// <returns>true إذا كانت الدفعة قيد الانتظار</returns>
        public bool IsPending()
        {
            return PaymentStatus == PaymentStatus.Pending;
        }

        /// <summary>
        /// التحقق من إمكانية إلغاء الدفعة
        /// </summary>
        /// <returns>true إذا كانت الدفعة قابلة للإلغاء</returns>
        public bool CanBeCanceled()
        {
            return PaymentStatus == PaymentStatus.Pending || PaymentStatus == PaymentStatus.Failed;
        }

        /// <summary>
        /// التحقق من إمكانية إعادة المحاولة
        /// </summary>
        /// <returns>true إذا كانت الدفعة قابلة لإعادة المحاولة</returns>
        public bool CanBeRetried()
        {
            return PaymentStatus == PaymentStatus.Failed;
        }

        /// <summary>
        /// الحصول على وصف حالة الدفع بالعربية
        /// </summary>
        /// <returns>وصف نصي لحالة الدفع</returns>
        public string GetStatusDescription()
        {
            return PaymentStatus switch
            {
                PaymentStatus.Pending => "قيد الانتظار",
                PaymentStatus.Completed => "مكتمل",
                PaymentStatus.Failed => "فاشل",
                PaymentStatus.Canceled => "ملغي",
                _ => "غير معروف"
            };
        }

        /// <summary>
        /// الحصول على وصف طريقة الدفع بالعربية
        /// </summary>
        /// <returns>وصف نصي لطريقة الدفع</returns>
        public string GetPaymentMethodDescription()
        {
            return PaymentMethod switch
            {
                PaymentMethod.Cash => "نقدي",
                PaymentMethod.CreditCard => "بطاقة ائتمان",
                PaymentMethod.DebitCard => "بطاقة خصم",
                PaymentMethod.Wallet => "محفظة إلكترونية",
                _ => "غير معروف"
            };
        }

        // ==================== Helper Methods ====================

        /// <summary>
        /// التحقق من مرور وقت معين منذ إنشاء الدفعة
        /// </summary>
        /// <param name="minutes">عدد الدقائق</param>
        /// <returns>true إذا مر الوقت المحدد</returns>
        public bool IsOlderThan(int minutes)
        {
            return DateTime.UtcNow.Subtract(PaymentDate).TotalMinutes > minutes;
        }

        /// <summary>
        /// الحصول على مدة الدفعة بالدقائق
        /// </summary>
        /// <returns>عدد الدقائق منذ إنشاء الدفعة</returns>
        public double GetAgeInMinutes()
        {
            return DateTime.UtcNow.Subtract(PaymentDate).TotalMinutes;
        }

        /// <summary>
        /// نسخ معلومات الدفعة لإنشاء دفعة جديدة (إعادة المحاولة)
        /// </summary>
        /// <returns>دفعة جديدة بنفس المعلومات</returns>
        public Payment Clone()
        {
            return Create(Amount, PaymentMethod, OrderId);
        }
    }
}