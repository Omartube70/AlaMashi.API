using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public enum Permissions { User = 1, Admin = 2 }

        public int UserID { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string PasswordHash { get; private set; }
        public Permissions UserPermissions { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }

        private User(int userId, string userName, string email, string phone, Permissions permissions, string passwordHash)
        {
            UserID = userId;
            UserName = userName;
            Email = email;
            Phone = phone;
            PasswordHash = passwordHash;
            UserPermissions = permissions;
        }

        #pragma warning disable CS8618
        private User() 
        {
        }
        #pragma warning restore CS8618

        public static async Task<User> CreateAsync(string userName, string email, string phone ,string passwordHash)
        {
            // --- Validation ---
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username is required.", nameof(userName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            if (!await ValidationHelper.IsEmailValidAsync(email))
                throw new ArgumentException("Invalid email format.", nameof(email));

            if (!string.IsNullOrWhiteSpace(phone) && !ValidationHelper.IsPhoneValid(phone))
                throw new ArgumentException("Invalid phone number format.", nameof(phone));

            // --- Business Rule Enforcement ---
            // المستخدم الجديد يحصل دائمًا على صلاحيات User بشكل افتراضي
            var defaultPermissions = Permissions.User;

            // --- Object Creation ---
            return new User(0,userName, email, phone, defaultPermissions, passwordHash);
        }

        public async Task UpdateProfileAsync(string userName, string email, string? phone)
        {
            // --- Validation ---
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username is required.", nameof(userName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone is required.", nameof(phone));

            if (!await ValidationHelper.IsEmailValidAsync(email))
                throw new ArgumentException("Invalid email format.", nameof(email));

            if (!ValidationHelper.IsPhoneValid(phone))
                throw new ArgumentException("Invalid phone number format.", nameof(phone));

            // --- State Update ---
            UserName = userName;
            Email = email;
            Phone = phone;
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash cannot be empty.");

            PasswordHash = newPasswordHash;
        }

        public void SetRefreshToken(string? refreshToken, DateTime? expiryTime)
        {
            if (refreshToken != null && expiryTime.HasValue && expiryTime.Value < DateTime.UtcNow)
            {
                throw new ArgumentException("Expiry time must be in the future.");
            }

            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = expiryTime;
        }

        public void PromoteToAdmin()
        {
            if (UserPermissions == Permissions.Admin)
            {
                return; 
            }

            UserPermissions = Permissions.Admin;
        }
    }
}
