using System;
using System.Data;
using System.Data.SqlClient;

namespace AlaMashi.DAL
{
    public class UserDAL
    {
        public static bool GetUserInfoByID(int UserID, ref string UserName, ref string Email, ref string Phone, ref string PasswordHash, ref int Permissions)
        {
            string query = "SELECT UserName, Email, Phone, PasswordHash, Permissions FROM Users WHERE UserID = @UserID";
            var parameter = new SqlParameter("@UserID", SqlDbType.Int) { Value = UserID };
            bool isFound = false;

            // 1. تعريف متغيرات مؤقتة
            string tempUserName = "", tempEmail = "", tempPhone = "", tempPasswordHash = "";
            int tempPermissions = 0;

            Execute.ExecuteReader(query, new[] { parameter }, reader =>
            {
                isFound = true;
                // 2. التخزين في المتغيرات المؤقتة
                tempUserName = reader["UserName"].ToString();
                tempEmail = reader["Email"].ToString();
                tempPhone = reader["Phone"].ToString();
                tempPasswordHash = reader["PasswordHash"].ToString();
                tempPermissions = (int)reader["Permissions"];
            });

            // 3. إسناد القيم للـ ref parameters بعد الخروج من اللامدا
            if (isFound)
            {
                UserName = tempUserName;
                Email = tempEmail;
                Phone = tempPhone;
                PasswordHash = tempPasswordHash;
                Permissions = tempPermissions;
            }

            return isFound;
        }

        public static bool GetUserInfoByEmail(string Email, ref int UserID, ref string UserName, ref string Phone, ref string PasswordHash, ref int Permissions)
        {
            string query = "SELECT UserID, UserName, Phone, PasswordHash, Permissions FROM Users WHERE Email = @Email";
            var parameter = new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = Email };
            bool isFound = false;

            int tempUserID = 0;
            string tempUserName = "", tempPhone = "", tempPasswordHash = "";
            int tempPermissions = 0;

            Execute.ExecuteReader(query, new[] { parameter }, reader =>
            {
                isFound = true;
                tempUserID = (int)reader["UserID"];
                tempUserName = reader["UserName"].ToString();
                tempPhone = reader["Phone"].ToString();
                tempPasswordHash = reader["PasswordHash"].ToString();
                tempPermissions = (int)reader["Permissions"];
            });

            if (isFound)
            {
                UserID = tempUserID;
                UserName = tempUserName;
                Phone = tempPhone;
                PasswordHash = tempPasswordHash;
                Permissions = tempPermissions;
            }

            return isFound;
        }

        public static bool GetUserByRefreshToken(string refreshToken, ref int userID, ref string userName, ref string email, ref string phone, ref string passwordHash, ref int permissions, ref DateTime refreshTokenExpiryTime)
        {
            string query = "SELECT * FROM Users WHERE RefreshToken = @RefreshToken";
            var parameter = new SqlParameter("@RefreshToken", SqlDbType.NVarChar, 256) { Value = refreshToken };
            bool isFound = false;

            // 1. تعريف متغيرات مؤقتة
            int tempUserID = 0;
            string tempUserName = "", tempEmail = "", tempPhone = "", tempPasswordHash = "";
            int tempPermissions = 0;
            DateTime tempExpiryTime = DateTime.MinValue;

            Execute.ExecuteReader(query, new[] { parameter }, reader =>
            {
                isFound = true;
                // 2. التخزين في المتغيرات المؤقتة
                tempUserID = (int)reader["UserID"];
                tempUserName = reader["UserName"].ToString();
                tempEmail = reader["Email"].ToString();
                tempPhone = reader["Phone"].ToString();
                tempPasswordHash = reader["PasswordHash"].ToString();
                tempPermissions = (int)reader["Permissions"];
                tempExpiryTime = (DateTime)reader["RefreshTokenExpiryTime"];
            });

            // 3. إسناد القيم للـ ref parameters
            if (isFound)
            {
                userID = tempUserID;
                userName = tempUserName;
                email = tempEmail;
                phone = tempPhone;
                passwordHash = tempPasswordHash;
                permissions = tempPermissions;
                refreshTokenExpiryTime = tempExpiryTime;
            }

            return isFound;
        }

        public static DataTable GetAllUsers()
        {
            string query = "SELECT UserID, UserName, Email, Phone, Permissions FROM Users";
            return Execute.GetDataTable(query);
        }
        public static int AddNewUser(string userName, string email, string phone, string PasswordHash, int permissions)
        {
            string query = @"INSERT INTO Users (UserName, Email, Phone, PasswordHash, Permissions) VALUES (@UserName, @Email, @Phone, @PasswordHash, @Permissions); SELECT SCOPE_IDENTITY();";
            var parameters = new[] { new SqlParameter("@UserName", userName), new SqlParameter("@Email", email), new SqlParameter("@Phone", phone), new SqlParameter("@PasswordHash", PasswordHash), new SqlParameter("@Permissions", permissions) };
            return Execute.ExecuteScalar<int>(query, parameters);
        }

        public static bool UpdateUser(int userID, string userName, string email, string phone, string PasswordHash, int permissions)
        {
            string query = @"UPDATE Users SET UserName = @UserName, Email = @Email, Phone = @Phone, PasswordHash = @PasswordHash, Permissions = @Permissions WHERE UserID = @UserID";
            var parameters = new[] { new SqlParameter("@UserID", userID), new SqlParameter("@UserName", userName), new SqlParameter("@Email", email), new SqlParameter("@Phone", phone), new SqlParameter("@PasswordHash", PasswordHash), new SqlParameter("@Permissions", permissions) };
            return Execute.ExecuteNonQuery(query, parameters) > 0;
        }

        public static bool DeleteUser(int userID)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";
            var parameter = new SqlParameter("@UserID", userID);
            return Execute.ExecuteNonQuery(query, new[] { parameter }) > 0;
        }

        public static bool IsUserExist(int userID)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE UserID = @UserID";
            var parameter = new SqlParameter("@UserID", userID);
            return Execute.ExecuteScalar<int>(query, new[] { parameter }) > 0;
        }

        public static bool IsEmailExists(string email)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            var parameter = new SqlParameter("@Email", email);
            return Execute.ExecuteScalar<int>(query, new[] { parameter }) > 0;
        }

        public static bool SaveRefreshToken(int userID, string refreshToken, DateTime expiryTime)
        {
            string query = @"UPDATE Users SET RefreshToken = @RefreshToken, RefreshTokenExpiryTime = @ExpiryTime WHERE UserID = @UserID";
            var parameters = new[] { new SqlParameter("@RefreshToken", (object)refreshToken ?? DBNull.Value), new SqlParameter("@ExpiryTime", expiryTime), new SqlParameter("@UserID", userID) };
            return Execute.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}