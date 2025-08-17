using System;
using System.Collections.Generic;
using System.Data;
using BCrypt.Net;
using AlaMashi.DAL; // افترض أن هذا هو الـ Namespace الصحيح

namespace AlaMashi.BLL
{
    public class UserBLL
    {
        private readonly UserDAL _userDAL;

        public enum enPermissions { User = 1, Admin = 2 }
        public enum enMode { AddNew = 0, Update = 1 };

        public int UserID { get; private set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; } // يتم تعيينها مؤقتاً للتشفير فقط
        public string PasswordHash { get; private set; }
        public enPermissions Permissions { get; set; }
        public enMode Mode { get; private set; }

        public bool IsUserExist => UserID != -1;

        public UserBLL(UserDAL userDAL)
        {
            _userDAL = userDAL ?? throw new ArgumentNullException(nameof(userDAL));

            UserID = -1;
            UserName = "";
            Email = "";
            Phone = "";
            PasswordHash = "";
            Permissions = enPermissions.User;
            Mode = enMode.AddNew;
        }

        // private constructor for existing users
        private UserBLL(UserDAL userDAL, UserData userData)
        {
            _userDAL = userDAL;
            UserID = userData.UserID;
            UserName = userData.UserName;
            Email = userData.Email;
            Phone = userData.Phone;
            PasswordHash = userData.PasswordHash;
            Permissions = (enPermissions)userData.Permissions;
            Mode = enMode.Update;
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }

        public static UserBLL FindByUserID(UserDAL userDAL, int userID)
        {
            UserData userData = userDAL.GetUserInfoByID(userID);
            if (userData != null)
            {
                return new UserBLL(userDAL, userData);
            }
            return null;
        }

        public static UserBLL GetUserByEmail(UserDAL userDAL, string email)
        {
            UserData userData = userDAL.GetUserInfoByEmail(email);
            if (userData != null)
            {
                return new UserBLL(userDAL, userData);
            }
            return null;
        }

        public bool Save()
        {
            // Validation (كما هي، لكن مع بعض التحسينات الطفيفة)
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Phone))
            {
                throw new ArgumentException("All required fields must be filled.");
            }

            if (!ValidationHelper.IsEmailValid(Email))
            {
                throw new ArgumentException("Invalid email format.");
            }
            if (!ValidationHelper.IsPhoneValid(Phone))
            {
                throw new ArgumentException("Invalid phone number format.");
            }

            if (Mode == enMode.AddNew || !string.IsNullOrEmpty(Password))
            {
                if (!ValidationHelper.IsPasswordValid(Password))
                {
                    throw new ArgumentException("Password does not meet the requirements.");
                }
            }

            if (Mode == enMode.AddNew)
            {
                if (_userDAL.IsEmailExists(Email))
                {
                    throw new ArgumentException("Email already exists.");
                }

                PasswordHash = HashPassword(Password);
                int newUserID = _userDAL.AddNewUser(UserName, Email, Phone, PasswordHash, (int)Permissions);

                if (newUserID != -1)
                {
                    UserID = newUserID;
                    Mode = enMode.Update;
                    return true;
                }

                return false;
            }
            else // Mode == enMode.Update
            {
                if (!string.IsNullOrEmpty(Password))
                {
                    PasswordHash = HashPassword(Password);
                }

                return _userDAL.UpdateUser(UserID, UserName, Email, Phone, PasswordHash, (int)Permissions);
            }
        }

        public bool Delete()
        {
            return _userDAL.DeleteUser(this.UserID);
        }

        public static bool DeleteUser(UserDAL userDAL, int userID)
        {
            return userDAL.DeleteUser(userID);
        }

        public static List<UserBLL> GetAllUsers(UserDAL userDAL)
        {
            List<UserBLL> users = new List<UserBLL>();
            DataTable dt = userDAL.GetAllUsers();

            foreach (DataRow row in dt.Rows)
            {
                var userData = new UserData
                {
                    UserID = Convert.ToInt32(row["UserID"]),
                    UserName = row["UserName"].ToString(),
                    Email = row["Email"].ToString(),
                    Phone = row["Phone"].ToString(),
                    PasswordHash = row["PasswordHash"].ToString(),
                    Permissions = Convert.ToInt32(row["Permissions"])
                };
                users.Add(new UserBLL(userDAL, userData));
            }
            return users;
        }

        // Helpers
        public bool isEmailExists() => _userDAL.IsEmailExists(Email);
        public static bool isUserExist(UserDAL userDAL, int userID) => userDAL.IsUserExist(userID);
    }
}