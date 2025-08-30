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

		public User(int userId, string userName, string email, string phone, Permissions permissions, string passwordHash)
		{
			// هنا يمكنك وضع منطق التحقق الأساسي
			if (string.IsNullOrWhiteSpace(userName))
				throw new ArgumentException("Username cannot be empty.");

			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("Email cannot be empty.");

			UserID = userId;
			UserName = userName;
			Email = email;
			Phone = phone;
			PasswordHash = passwordHash;
			UserPermissions = permissions;
		}
		public User()
		{
			UserID = -1;
			UserName = string.Empty;
			Email = string.Empty;
			Phone = string.Empty;
			PasswordHash = string.Empty;
			UserPermissions = Permissions.User;
		}

		public void ChangePassword(string newPasswordHash)
		{
			if (string.IsNullOrWhiteSpace(newPasswordHash))
				return; // أو throw exception

			PasswordHash = newPasswordHash;
		}

		public void UpdateInfo(string newUserName, string newEmail, string newPhone)
		{
			if (!string.IsNullOrWhiteSpace(newUserName))
				UserName = newUserName;

			if (!string.IsNullOrWhiteSpace(newEmail))
				Email = newEmail;

			Phone = newPhone; // Phone يمكن أن يكون فارغًا
		}

		public void PromoteToAdmin()
		{
			UserPermissions = Permissions.Admin;
		}
	}
}
