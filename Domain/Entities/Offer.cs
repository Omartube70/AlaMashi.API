using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Offer 
    {
        public int OfferID { get; private set; }
        public string Title { get; private set; } // عنوان العرض (مثلاً: "عروض تصل إلى 30%")
        public string? Description { get; private set; } // وصف اختياري للعرض
        public decimal DiscountPercentage { get; private set; } // نسبة الخصم (مثلاً: 0.30 لـ 30%)
        public DateTime StartDate { get; private set; } // تاريخ بداية العرض
        public DateTime EndDate { get; private set; } // تاريخ نهاية العرض
        public bool IsActive { get; private set; } // هل العرض نشط حالياً؟)

        public virtual ICollection<Product> Products { get; private set; } = new List<Product>();


#pragma warning disable CS8618
        private Offer() { } 
#pragma warning restore CS8618

        private Offer(string title, string? description, decimal discountPercentage, DateTime startDate, DateTime endDate , bool isActive)
        {
            Title = title;
            Description = description;
            DiscountPercentage = discountPercentage;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
        }
        
        // --- Business Logic Methods ---

        public static Offer Create(string title, string? description, decimal discountPercentage, DateTime startDate, DateTime endDate)
        {
            // --- Validation ---   

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Offer title is required.", nameof(title));

            if (discountPercentage <= 0 || discountPercentage > 1)
                throw new ArgumentException("Discount percentage must be between 0 and 1 (exclusive).", nameof(discountPercentage));

            if (startDate >= endDate)
                throw new ArgumentException("Start date must be before end date.", nameof(startDate));

            bool isActive = (DateTime.UtcNow >= startDate && DateTime.UtcNow <= endDate); // تحديث الحالة عند الإنشاء

            return new Offer(title, description, discountPercentage, startDate, endDate, isActive);
        }

        public void UpdateOfferDetails(string newTitle, string? newDescription, decimal newDiscountPercentage, DateTime newStartDate, DateTime newEndDate)
        {
            // --- Validation ---   
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Offer title is required.", nameof(newTitle));

            if (newDiscountPercentage <= 0 || newDiscountPercentage > 1)
                throw new ArgumentException("Discount percentage must be between 0 and 1 (exclusive).", nameof(newDiscountPercentage));

            if (newStartDate >= newEndDate)
                throw new ArgumentException("Start date must be before end date.", nameof(newStartDate));

            Title = newTitle;
            Description = newDescription;
            DiscountPercentage = newDiscountPercentage;
            StartDate = newStartDate;
            EndDate = newEndDate;
            UpdateActiveStatus(); 
        }

        public void ActivateOffer()
        {
            if (DateTime.UtcNow < EndDate)
            {
                IsActive = true;
            }
            else
            {
                throw new InvalidOperationException("Cannot activate an expired offer.");
            }
        }

        public void DeactivateOffer()
        {
            IsActive = false;
        }

        public void UpdateActiveStatus()
        {
            IsActive = (DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate);
        }

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (!Products.Contains(product))
            {
                Products.Add(product);
            }
        }

        public void RemoveProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            Products.Remove(product);
        }
    }
}