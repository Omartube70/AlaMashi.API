using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace AlaMashi.DAL
{
    public class UserDAL
    {
        public static bool GetUserInfoByID(int UserID, ref string UserName,
             ref string Email, ref string Phone, ref string PasswordHash, ref int Permissions)
        {
            bool isFound = false;
            string query = "SELECT * FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                UserName = (string)reader["UserName"];
                                Email = (string)reader["Email"];
                                Phone = (string)reader["Phone"];
                                PasswordHash = (string)reader["PasswordHash"];
                                Permissions = (int)reader["Permissions"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: سجل الخطأ هنا
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        public static bool GetUserInfoByEmail(string Email, ref int UserID, ref string UserName,
            ref string Phone, ref string PasswordHash, ref int Permissions)
        {
            bool isFound = false;
            string query = "SELECT * FROM Users WHERE Email = @Email";

            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", Email);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                UserID = (int)reader["UserID"];
                                UserName = (string)reader["UserName"];
                                Phone = (string)reader["Phone"];
                                PasswordHash = (string)reader["PasswordHash"];
                                Permissions = (int)reader["Permissions"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: سجل الخطأ هنا
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        public static int AddNewUser(string UserName, string Email, string Phone,
            string PasswordHash, int Permissions)
        {
            int UserID = -1;
            string query = @"
                INSERT INTO [dbo].[Users] ([UserName], [Email], [Phone], [PasswordHash], [Permissions])
                VALUES (@UserName, @Email, @Phone, @PasswordHash, @Permissions);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                    command.Parameters.AddWithValue("@Permissions", Permissions);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            UserID = insertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: سجل الخطأ هنا
                        UserID = -1;
                    }
                }
            }
            return UserID;
        }

        public static bool UpdateUser(int UserID, string UserName, string Email,
            string Phone, string PasswordHash, int Permissions)
        {
            int rowsAffected = 0;
            string query = @"
                UPDATE [dbo].[Users]
                SET [UserName] = @UserName, [Email] = @Email, [Phone] = @Phone,
                    [PasswordHash] = @PasswordHash, [Permissions] = @Permissions
                WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                    command.Parameters.AddWithValue("@Permissions", Permissions);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // TODO: سجل الخطأ هنا
                        return false;
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static bool DeleteUser(int UserID)
        {
            string query = "Delete Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    try
                    {
                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        // سجل الخطأ
                        return false;
                    }
                }
            }
        }

        public static DataTable GetAllUsers()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "Select * From Users";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            return (dt != null) ? dt : null;
        }

        public static bool IsUserExist(int UserID)
        {
            string query = "SELECT 1 FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        return (result != null);
                    }
                    catch (Exception ex)
                    {
                        // سجل الخطأ
                        return false;
                    }
                }
            }
        }

        public static bool IsEmailExists(string Email)
        {
            string query = "SELECT 1 FROM Users WHERE Email = @Email";

            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", Email);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        return (result != null);
                    }
                    catch (Exception ex)
                    {
                        // سجل الخطأ
                        return false;
                    }
                }
            }
        }
    }
}
