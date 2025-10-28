using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities
{
    public class Address
    {
        public int AddressId { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string? AddressDetails { get; private set; }
        public AddressType AddressType { get; private set; }

        // Foreign Key to User
        public int UserId { get; private set; }

        // Navigation Property
        public User User { get; private set; }

#pragma warning disable CS8618
        private Address() { }
#pragma warning restore CS8618

        private Address(string street, string city, string? details, AddressType type, int userId)
        {
            Street = street;
            City = city;
            AddressDetails = details;
            AddressType = type;
            UserId = userId;
        }

        public static Address Create(string street, string city, string? details, AddressType type, int userId)
        {
            if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("Street and City cannot be empty.");
            }

            return new Address(street, city, details, type, userId);
        }

        public void Update(string street, string city, string? details, AddressType type)
        {
            if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("Street and City cannot be empty.");
            }
            Street = street;
            City = city;
            AddressDetails = details;
            AddressType = type;
        }
    }
}
