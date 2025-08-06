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


        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public int Permissions { get; set; }




        public UserBLL()
        {
            this.UserID = -1;
            this.UserName = "";
            this.Email = "";
            this.Phone = "";
            this.PasswordHash = "";
            this.Permissions = 0;
            Mode = enMode.AddNew;
        }

        private UserBLL(int UserID, string UserName, string Email, string Phone, string PasswordHash , int Permissions)
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

            PasswordHash = HashPassword(this.PasswordHash);

            this.UserID = UserDAL.AddNewUser(this.UserName, this.Email, this.Phone, this.PasswordHash , this.Permissions);

            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            // نجيب الهاش القديم من الداتابيز
            string existingPasswordHash = "";
            string dbEmail = "", dbPhone = "" , dbUserName = "";
            int Permissions = 0;
            UserDAL.GetUserInfoByID(this.UserID, ref dbUserName, ref dbEmail, ref dbPhone, ref existingPasswordHash, ref Permissions);

            // لو الباسورد اتغير (يعني المستخدم كتب واحد جديد) → نشفره من جديد
            if (!BCrypt.Net.BCrypt.Verify(this.PasswordHash, existingPasswordHash) & this.PasswordHash != existingPasswordHash)
            {
                this.PasswordHash = HashPassword(this.PasswordHash);
            }
            else
            {
                // لو نفس الباسورد القديم، خليه زي ما هو
                this.PasswordHash = existingPasswordHash;
            }

            return UserDAL.UpdateUser(this.UserID, this.UserName, this.Email, this.Phone, this.PasswordHash, this.Permissions);
        }

        public static UserBLL FindByUserID(int UserID)
        {
            string UserName = "" ,Email = "", PasswordHash = "" , Phone = "";
            int Permissions = 0;


            if (UserDAL.GetUserInfoByID(UserID, ref UserName, ref Email, ref Phone, ref PasswordHash , ref Permissions))

                return new UserBLL(UserID, UserName,
                           Email, Phone, PasswordHash , Permissions);
            else
                return null;
        }

        public static UserBLL FindByUserUserName(string UserName)
        {
             string Email = "", PasswordHash = "" , Phone = "";
             int Permissions = 0;
             int UserID = -1;    

            if (UserDAL.GetUserInfoByUserName(UserName , ref UserID, ref Email, ref Phone, ref PasswordHash , ref Permissions))

                return new UserBLL(UserID, UserName,
                           Email, Phone, PasswordHash , Permissions);
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
                           Email, Phone, PasswordHash, Permissions);
            else
                return null;
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.Phone) || string.IsNullOrEmpty(this.PasswordHash))
            {
                return false; // Validation failed, return false
            }

            if (isUserExist(this.UserID))
            {
                this.Mode = enMode.Update;
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
                    Permissions = Convert.ToInt32(row["Permissions"])
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


        public static UserBLL GetUserByCredentials(string UserName, string Password)
        {
            int UserID = -1;
            string Email = "" , Phone = "" , PasswordHash = "";
            int Permissions = 0;



            if (UserDAL.GetUserInfoByUserName(UserName, ref UserID, ref Email, ref Phone, ref PasswordHash, ref Permissions))
            {
                if(VerifyPassword(Password, PasswordHash))
                {
                    return new UserBLL(UserID, UserName, Email, Phone, PasswordHash, Permissions);
                }
            }
                return null;
        }

        public static bool IsUserNameExists(string UserName)
        {
            return UserDAL.IsUserNameExists(UserName);
        }

    }
}

