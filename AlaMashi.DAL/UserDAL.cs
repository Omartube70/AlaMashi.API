using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace AlaMashi.DAL
{
    // يمكننا إنشاء DTO بسيط (Data Transfer Object) لتمرير البيانات
    // هذا الكلاس لا يحتوي على أي منطق، فقط خصائص لتخزين البيانات
    public class UserData
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public int Permissions { get; set; }
    }

    public class UserDAL
    {
        private readonly string _connectionString;

        public UserDAL(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        private T ExecuteScalar<T>(string query, SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return (T)Convert.ChangeType(result, typeof(T));
                    }
                }
                catch (Exception ex)
                {
                    // TODO: تسجيل الخطأ هنا (استخدم مكتبة logging)
                    Console.WriteLine($"Error in ExecuteScalar: {ex.Message}");
                }
                return default(T);
            }
        }

        private int ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // TODO: تسجيل الخطأ هنا
                    Console.WriteLine($"Error in ExecuteNonQuery: {ex.Message}");
                }
                return 0;
            }
        }

        public UserData GetUserInfoByID(int userID)
        {
            string query = "SELECT UserID, UserName, Email, Phone, PasswordHash, Permissions FROM Users WHERE UserID = @UserID";
            var parameters = new[] { new SqlParameter("@UserID", SqlDbType.Int) { Value = userID } };

            return GetUserFromQuery(query, parameters);
        }

        public UserData GetUserInfoByEmail(string email)
        {
            string query = "SELECT UserID, UserName, Email, Phone, PasswordHash, Permissions FROM Users WHERE Email = @Email";
            var parameters = new[] { new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = email } };

            return GetUserFromQuery(query, parameters);
        }

        private UserData GetUserFromQuery(string query, SqlParameter[] parameters)
        {
            UserData userData = null;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userData = new UserData
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                Permissions = reader.GetInt32(reader.GetOrdinal("Permissions"))
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO: تسجيل الخطأ
                    Console.WriteLine($"Error in GetUserFromQuery: {ex.Message}");
                }
            }
            return userData;
        }

        public int AddNewUser(string userName, string email, string phone, string passwordHash, int permissions)
        {
            string query = @"
                INSERT INTO Users (UserName, Email, Phone, PasswordHash, Permissions)
                VALUES (@UserName, @Email, @Phone, @PasswordHash, @Permissions);
                SELECT SCOPE_IDENTITY();";

            var parameters = new[]
            {
                new SqlParameter("@UserName", SqlDbType.NVarChar, 50) { Value = userName },
                new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = email },
                new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = phone },
                new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 255) { Value = passwordHash },
                new SqlParameter("@Permissions", SqlDbType.Int) { Value = permissions }
            };

            return ExecuteScalar<int>(query, parameters);
        }

        public bool UpdateUser(int userID, string userName, string email, string phone, string passwordHash, int permissions)
        {
            string query = @"
                UPDATE Users
                SET UserName = @UserName, Email = @Email, Phone = @Phone,
                    PasswordHash = @PasswordHash, Permissions = @Permissions
                WHERE UserID = @UserID";

            var parameters = new[]
            {
                new SqlParameter("@UserID", SqlDbType.Int) { Value = userID },
                new SqlParameter("@UserName", SqlDbType.NVarChar, 50) { Value = userName },
                new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = email },
                new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = phone },
                new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 255) { Value = passwordHash },
                new SqlParameter("@Permissions", SqlDbType.Int) { Value = permissions }
            };

            return ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteUser(int userID)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";
            var parameter = new SqlParameter("@UserID", SqlDbType.Int) { Value = userID };

            return ExecuteNonQuery(query, new[] { parameter }) > 0;
        }

        public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            string query = "SELECT UserID, UserName, Email, Phone, PasswordHash, Permissions FROM Users";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            using (var adapter = new SqlDataAdapter(command))
            {
                try
                {
                    connection.Open();
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    // TODO: تسجيل الخطأ
                    Console.WriteLine($"Error in GetAllUsers: {ex.Message}");
                }
            }
            return dt;
        }

        public bool IsUserExist(int userID)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE UserID = @UserID";
            var parameter = new SqlParameter("@UserID", SqlDbType.Int) { Value = userID };

            return ExecuteScalar<int>(query, new[] { parameter }) > 0;
        }

        public bool IsEmailExists(string email)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            var parameter = new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = email };

            return ExecuteScalar<int>(query, new[] { parameter }) > 0;
        }
    }
}