using System;
using System.Collections.Generic;
using System.Data;
using BCrypt.Net;
using AlaMashi.DAL;

namespace AlaMashi.BLL
{
    public class UserBLL
    {
        public enum enPermissions { User = 1, Admin = -1 }
        public enum enMode { AddNew = 0, Update = 1 };

        public int UserID { get; private set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; } // يتم تعيينها مؤقتاً للتشفير فقط
        public string PasswordHash { get; private set; }
        public enPermissions Permissions { get; set; }
        public enMode Mode { get; private set; }

        public UserBLL()
        {
            UserID = -1;
            UserName = "";
            Email = "";
            Phone = "";
            PasswordHash = "";
            Permissions = enPermissions.User;
            Mode = enMode.AddNew;
        }

        private UserBLL(int UserID , string UserName , string Email , string Phone , string PasswordHash , enPermissions Permissions)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.Email = Email;
            this.Phone = Phone;
            this.PasswordHash = PasswordHash;
            this.Permissions = Permissions;
            this.Mode = enMode.Update;
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }

        public static UserBLL GetUserByUserID(int UserID)
        {
            string Email = "" , UserName = "" , Phone = "" , PasswordHash = "";
            int Permissions = 0;

            bool IsFound = UserDAL.GetUserInfoByID(UserID, ref UserName , ref Email , ref Phone , ref PasswordHash , ref Permissions);

            if (IsFound)
            {
                return new UserBLL(UserID, UserName , Email , Phone , PasswordHash ,(enPermissions)Permissions);
            }

            return null;
        }

        public static UserBLL GetUserByEmail(string Email)
        {
            string UserName = "", Phone = "", PasswordHash = "";
            int UserID = -1 , Permissions = 0;

            bool IsFound = UserDAL.GetUserInfoByEmail(Email, ref UserID, ref UserName, ref Phone, ref PasswordHash, ref Permissions);

            if (IsFound)
            {
                return new UserBLL(UserID, UserName, Email, Phone, PasswordHash, (enPermissions)Permissions);
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
                if (UserDAL.IsEmailExists(Email))
                {
                    throw new ArgumentException("Email already exists.");
                }

                PasswordHash = HashPassword(Password);
                int newUserID = UserDAL.AddNewUser(UserName, Email, Phone, PasswordHash, (int)Permissions);

                if (newUserID != -1 & newUserID > 0)
                {
                    UserID = newUserID;
                    Mode = enMode.Update;
                    return true;
                }

                throw new ArgumentException("Failed To Save");
            }
            else // Mode == enMode.Update
            {
                if (!string.IsNullOrEmpty(Password))
                {
                    PasswordHash = HashPassword(Password);
                }

                return UserDAL.UpdateUser(UserID, UserName, Email, Phone, PasswordHash, (int)Permissions);
            }
        }

        public bool Delete()
        {
            return UserDAL.DeleteUser(this.UserID);
        }

        public static bool DeleteUser(int UserID)
        {
            return UserDAL.DeleteUser(UserID);
        }

        public static List<UserBLL> GetAllUsers()
        {
            List<UserBLL> users = new List<UserBLL>();
            DataTable dt = UserDAL.GetAllUsers();

            foreach (DataRow row in dt.Rows)
            {
                UserBLL userbll = new UserBLL();
                userbll.UserID = Convert.ToInt32(row["UserID"]);
                userbll.UserName = row["UserName"].ToString();
                userbll.Email = row["Email"].ToString();
                userbll.Phone = row["Phone"].ToString();
                userbll.PasswordHash = row["PasswordHash"].ToString();
                userbll.Permissions = (enPermissions) Convert.ToInt32(row["Permissions"]);
                users.Add(userbll);
            }
            return users;
        }

        // Helpers
        public static bool IsEmailExists(string Email)
        {
            return UserDAL.IsEmailExists(Email);
        }
        public static bool isUserExist(int userID) 
        {
            return UserDAL.IsUserExist(userID);
        }

        // Token
        public static bool SaveRefreshToken(int userID, string refreshToken, DateTime expiryTime)
        {
            // هذه الدالة تستدعي دالة الـ DAL المقابلة لها مباشرة
            return UserDAL.SaveRefreshToken(userID, refreshToken, expiryTime);
        }

        public static UserBLL GetUserByRefreshToken(string refreshToken)
        {
            int userID = -1, permissions = 0;
            string userName = "", email = "", phone = "", passwordHash = "";
            DateTime refreshTokenExpiryTime = DateTime.MinValue;

            // نستدعي دالة الـ DAL التي أنشأناها
            bool isFound = UserDAL.GetUserByRefreshToken(refreshToken, ref userID, ref userName, ref email, ref phone, ref passwordHash, ref permissions, ref refreshTokenExpiryTime);

            // نتحقق من شيئين: هل التوكن موجود؟ وهل تاريخ صلاحيته لم ينته بعد؟
            if (isFound && refreshTokenExpiryTime > DateTime.UtcNow)
            {
                // إذا كان كل شيء سليمًا، نرجع بيانات المستخدم
                return new UserBLL(userID, userName, email, phone, passwordHash, (enPermissions)permissions);
            }

            // إذا كان التوكن غير موجود أو انتهت صلاحيته، نرجع null
            return null;
        }

    }
}