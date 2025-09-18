using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public int OrderId { get; private set; }

        public DateTime OrderDate { get; private set; }

        public string DeliveryTimeSlot { get; private set; }


        /// <summary>
        /// المبلغ الإجمالي للطلب. يمكن أن يكون null في البداية
        /// ويتم حسابه لاحقًا بعد إضافة المنتجات.
        /// </summary>
        public decimal? TotalAmount { get; private set; }


        // Foreign Keys
        public int UserId { get; private set; }
        public int AddressId { get; private set; }

        // Navigation Propertys
        public User User { get; private set; }
        public Address Address { get; private set; }

        // Status of the order
        public OrderStatus Status { get; private set; }


        #pragma warning disable CS8618
        private Order() { }
        #pragma warning restore CS8618


        private Order(DateTime orderDate, string deliveryTimeSlot, decimal? totalAmount, int userId, int addressId, OrderStatus status)
        {
            OrderDate = orderDate;
            DeliveryTimeSlot = deliveryTimeSlot;
            TotalAmount = totalAmount;
            UserId = userId;
            AddressId = addressId;
            Status = status;
        }

        public static Order Create(DateTime orderDate, string deliveryTimeSlot, decimal? totalAmount, int userId, int addressId)
        {
            // --- Validation ---
            if (orderDate == default)
                throw new ArgumentException("Order date is required.", nameof(orderDate));

            if (string.IsNullOrWhiteSpace(deliveryTimeSlot))
                throw new ArgumentException("Delivery time slot is required.", nameof(deliveryTimeSlot));

            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.", nameof(userId));

            if (addressId <= 0)
                throw new ArgumentException("Invalid address ID.", nameof(addressId));

            // Default status for a new order
            var defaultStatus = OrderStatus.Pending;

            // --- Object Creation ---
            return new Order(orderDate, deliveryTimeSlot, totalAmount, userId, addressId, defaultStatus);
        }


        public void UpdateTotalAmount(decimal totalAmount)
        {
            if (totalAmount <= 0)
                throw new ArgumentException("Total amount cannot be negative.", nameof(totalAmount));

            TotalAmount = totalAmount;
        }

        public void UpdateDeliveryTimeSlot(string deliveryTimeSlot)
        {
            if (string.IsNullOrWhiteSpace(deliveryTimeSlot))
                throw new ArgumentException("Delivery time slot is required.", nameof(deliveryTimeSlot));


            DeliveryTimeSlot = deliveryTimeSlot;
        }



    }
}
