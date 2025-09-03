using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product
    {
        public int ProductID { get; private set; }
        public string ProductName { get; private set; }
        public string Barcode { get; private set; }
        public string? ProductDescription { get; private set; }
        public decimal Price { get; private set; }
        public int QuantityInStock { get; private set; }
        public string MainImageURL { get; private set; }

        // --- Foreign Keys ---
        public int CategoryID { get; private set; }

        // --- Navigation Properties ---
        public Category Category { get; private set; }


#pragma warning disable CS8618
        private Product() { }
#pragma warning restore CS8618

        private Product(string name, string barcode, string? description, decimal price, int quantity, string imageUrl, int categoryId)
        {
            ProductName = name;
            Barcode = barcode;
            ProductDescription = description;
            Price = price;
            QuantityInStock = quantity;
            MainImageURL = imageUrl;
            CategoryID = categoryId;
        }

        public static Product Create(string name, string barcode, string? description, decimal price, int quantity, string imageUrl, int categoryId)
        {
            // --- Validation ---
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required.", nameof(name));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));

            var normalizeddescription = string.IsNullOrWhiteSpace(description) ? null : description;

            return new Product(name, barcode, normalizeddescription, price, quantity, imageUrl, categoryId);
        }

        // --- Business Logic Methods ---

        public void UpdateDetails(string newName, string? newDescription)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Product name is required.", nameof(newName));

            ProductName = newName;
            ProductDescription = newDescription;
        }

        public void UpdateMainImage(string newImageUrl)
        {
            if (string.IsNullOrWhiteSpace(newImageUrl))
                throw new ArgumentException("Image URL cannot be empty.", nameof(newImageUrl));

            MainImageURL = newImageUrl;
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            Price = newPrice;
        }

        public void AddStock(int amountToAdd)
        {
            if (amountToAdd <= 0)
                throw new ArgumentException("Amount to add must be positive.");

            QuantityInStock += amountToAdd;
        }

        public void RemoveStock(int amountToRemove)
        {
            if (amountToRemove <= 0)
                throw new ArgumentException("Amount to remove must be positive.");

            if (QuantityInStock < amountToRemove)
                throw new InvalidOperationException("Not enough stock available.");

            QuantityInStock -= amountToRemove;
        }
    }
}