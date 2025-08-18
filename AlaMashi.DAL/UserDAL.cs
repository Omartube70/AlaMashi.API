using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace AlaMashi.DAL
{
    public class UserDAL
    {

        public static bool  GetUserInfoByID(int UserID , ref string UserName , ref string Email , ref string Phone , ref string PasswordHash,ref int Persmissions)
        {
            string query = "SELECT UserName, Email, Phone, PasswordHash, Permissions FROM Users WHERE UserID = @UserID";
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    UserName = (string)reader["UserName"];
                    Email = (string)reader["Email"];
                    Phone = (string)reader["Phone"];
                    PasswordHash = (string)reader["PasswordHash"];
                    Persmissions = (int)reader["Permissions"];
                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetUserInfoByEmail(string Email , ref int UserID, ref string UserName, ref string Phone, ref string PasswordHash,ref int Persmissions)
        {
            string query = "SELECT UserID, UserName, Phone, PasswordHash, Permissions FROM Users WHERE Email = @Email";
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Email", Email);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    UserName = (string)reader["UserName"];
                    Phone = (string)reader["Phone"];
                    PasswordHash = (string)reader["PasswordHash"];
                    Persmissions = (int)reader["Permissions"];
                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetUserByRefreshToken(string refreshToken, ref int userID, ref string userName, ref string email, ref string phone, ref string passwordHash, ref int permissions, ref DateTime refreshTokenExpiryTime)
        {
            string query = "SELECT * FROM Users WHERE RefreshToken = @RefreshToken";
            bool isFound = false;

            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RefreshToken", refreshToken);
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            userID = (int)reader["UserID"];
                            userName = (string)reader["UserName"];
                            email = (string)reader["Email"];
                            phone = (string)reader["Phone"];
                            passwordHash = (string)reader["PasswordHash"];
                            permissions = (int)reader["Permissions"];
                            refreshTokenExpiryTime = (DateTime)reader["RefreshTokenExpiryTime"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                    // TODO: Log error
                }
            }
            return isFound;
        }

        public static int AddNewUser(string userName, string email, string phone, string PasswordHash, int permissions)
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
                new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 255) { Value = PasswordHash },
                new SqlParameter("@Permissions", SqlDbType.Int) { Value = permissions }
            };

            return Execute.ExecuteScalar<int>(query, parameters);
        }

        public static bool UpdateUser(int userID, string userName, string email, string phone, string PasswordHash, int permissions)
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
                new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 255) { Value = PasswordHash },
                new SqlParameter("@Permissions", SqlDbType.Int) { Value = permissions }
            };

            return Execute.ExecuteNonQuery(query, parameters) > 0;
        }

        public static bool DeleteUser(int userID)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";
            var parameter = new SqlParameter("@UserID", SqlDbType.Int) { Value = userID };

            return Execute.ExecuteNonQuery(query, new[] { parameter }) > 0;
        }

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            string query = "SELECT UserID, UserName, Email, Phone, PasswordHash, Permissions FROM Users";

            using (var connection = new SqlConnection(Settings.ConnectionString))
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

        public static bool IsUserExist(int userID)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE UserID = @UserID";
            var parameter = new SqlParameter("@UserID", SqlDbType.Int) { Value = userID };

            return Execute.ExecuteScalar<int>(query, new[] { parameter }) > 0;
        }
        
        public static bool IsEmailExists(string email)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            var parameter = new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = email };

            return Execute.ExecuteScalar<int>(query, new[] { parameter }) > 0;
        }


        // Token Mathoud
        public static bool SaveRefreshToken(int userID, string refreshToken, DateTime expiryTime)
        {
            string query = @"UPDATE Users 
                       SET RefreshToken = @RefreshToken, RefreshTokenExpiryTime = @ExpiryTime 
                       WHERE UserID = @UserID";

            var parameters = new[]
            {
               new SqlParameter("@RefreshToken", SqlDbType.NVarChar, 256) { Value = (object)refreshToken ?? DBNull.Value },
               new SqlParameter("@ExpiryTime", SqlDbType.DateTime2) { Value = expiryTime },
               new SqlParameter("@UserID", SqlDbType.Int) { Value = userID }
            };

            // افترض أن لديك كلاس Execute للتعامل مع الأوامر
            return Execute.ExecuteNonQuery(query, parameters) > 0;
        }

    }
}