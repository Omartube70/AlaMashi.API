using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BCrypt.Net;
using AlaMashi.DAL;

namespace AlaMashi.BLL
{
    public class UserBLL
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enPermissions { User = 1 , Admin = -1 }
        public enPermissions Permissions;

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Password { get; set; }
        public string PasswordHash { get; set; }


        public UserBLL()
        {
            this.UserID = -1;
            this.UserName = "";
            this.Email = "";
            this.Phone = "";
            this.PasswordHash = "";
            this.Password = "";
            this.Permissions = enPermissions.User;
            Mode = enMode.AddNew;
        }

        private UserBLL(int UserID, string UserName, string Email, string Phone, string PasswordHash , enPermissions Permissions)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.Email = Email;
            this.Phone = Phone;
            this.PasswordHash = PasswordHash;
            this.Permissions = Permissions;
            Mode = enMode.Update;
        }

        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool _AddNewUser()
        {
            //call DataAccess Layer 

            this.PasswordHash = HashPassword(this.Password);

            // قم بمسح كلمة المرور من الذاكرة بعد استخدامها
            this.Password = "";

            this.UserID = UserDAL.AddNewUser(this.UserName, this.Email, this.Phone, this.PasswordHash , ((int)this.Permissions));

            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            // إذا كان المستخدم قد أدخل كلمة مرور جديدة، قم بتشفيرها
            if (!string.IsNullOrEmpty(this.Password))
            {
                this.PasswordHash = HashPassword(this.Password);
                // قم بمسح كلمة المرور من الذاكرة بعد استخدامها
                this.Password = "";
            }
            else
            {
                // إذا لم يدخل كلمة مرور جديدة، استخدم الهاش القديم من قاعدة البيانات
                this.PasswordHash = GetPassowrdHashFromDB(this.UserID);
            }

            return UserDAL.UpdateUser(this.UserID, this.UserName, this.Email, this.Phone, this.PasswordHash, ((int)this.Permissions));
        }

        public static UserBLL FindByUserID(int UserID)
        {
            string UserName = "" ,Email = "", PasswordHash = "" , Phone = "";
            int Permissions = 0;


            if (UserDAL.GetUserInfoByID(UserID, ref UserName, ref Email, ref Phone, ref PasswordHash , ref Permissions))

                return new UserBLL(UserID, UserName,
                           Email, Phone, PasswordHash , ((enPermissions)Permissions));
            else
                return null;
        }

        public static UserBLL GetUserByEmail(string Email)
        {
            string UserName = "", PasswordHash = "", Phone = "";
            int Permissions = 0;
            int UserID = -1;

            if (UserDAL.GetUserInfoByEmail(Email, ref UserID , ref UserName, ref Phone, ref PasswordHash, ref Permissions))

                return new UserBLL(UserID, UserName,
                           Email, Phone, PasswordHash, ((enPermissions)Permissions));
            else
                return null;
        }

        public static string GetPassowrdHashFromDB(int UserID)
        {
            string existingPasswordHash = "";
            string dbEmail = "", dbPhone = "", dbUserName = "";
            int dbPermissions = 0;
            UserDAL.GetUserInfoByID(UserID, ref dbUserName, ref dbEmail, ref dbPhone, ref existingPasswordHash, ref dbPermissions);

            return existingPasswordHash;
        }

        public bool Save()
        {
            if (isUserExist(this.UserID))
                this.Mode = enMode.Update;

            // التحقق من الحقول الأساسية
            if (string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.Phone))
            {
                throw new ArgumentException("All fields are required.");
            }

            // 1. التحقق من صحة صيغة البريد الإلكتروني
            if (!ValidationHelper.IsEmailValid(this.Email))
            {
                throw new ArgumentException("Invalid email format.");
            }

            // 2. التحقق من وجود البريد الإلكتروني في قاعدة البيانات (فقط عند الإضافة)
            if (Mode == enMode.AddNew && IsEmailExists(this.Email))
            {
                throw new ArgumentException("Email already exists.");
            }


            // التحقق من صحة صيغة رقم الهاتف
            if (!ValidationHelper.IsPhoneValid(this.Phone))
            {
                throw new ArgumentException("Invalid phone number format.");
            }

            // التحقق من صحة كلمة المرور فقط في حالتي:
            // 1. إضافة مستخدم جديد (حيث يجب إدخال كلمة مرور)
            // 2. تحديث مستخدم وكلمة المرور ليست فارغة (يعني المستخدم يريد تغييرها)
            if (Mode == enMode.AddNew || !string.IsNullOrEmpty(this.Password))
            {
                // استدعاء دالة التحقق من كلاس Validator
                if (!ValidationHelper.IsPasswordValid(this.Password))
                {
                    throw new ArgumentException("Password does not meet the requirements.");
                }
            }


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                    }
                    return this.UserID != -1;

                case enMode.Update:
                    // هنا ستستمر في التحديث، و_UpdateUser() ستتعامل مع كلمة المرور
                    return _UpdateUser();
            }

            return false;
        }

        public static List<UserBLL> ConvertDataTableToUserList(DataTable dt)
        {
            List<UserBLL> Users = new List<UserBLL>();

            foreach (DataRow row in dt.Rows)
            {
                UserBLL user = new UserBLL
                {
                    UserID = Convert.ToInt32(row["UserID"]),
                    UserName = row["UserName"].ToString(),
                    Email = row["Email"].ToString(),
                    Phone = row["Phone"].ToString(),
                    PasswordHash = row["PasswordHash"].ToString(),
                    Permissions = ((enPermissions)Convert.ToInt32(row["Permissions"]))
                };

                Users.Add(user);
            }

            return Users;
        }

        public static List<UserBLL> GetAllUsers()
        {
            return ConvertDataTableToUserList(UserDAL.GetAllUsers());
        }

        public static bool DeleteUser(int UserID)
        {
            return UserDAL.DeleteUser(UserID);
        }

        public bool DeleteUser()
        {
            return UserDAL.DeleteUser(this.UserID);
        }

        public static bool isUserExist(int UserID)
        {
            return UserDAL.IsUserExist(UserID);
        }

        public static bool IsEmailExists(string Email)
        {
            return UserDAL.IsEmailExists(Email);
        }
    }
}

