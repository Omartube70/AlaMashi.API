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
        ref string Email, ref string Phone , ref string PasswordHash,ref int Permissions)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT * FROM Users WHERE UserID = @UserID";

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
                    Permissions = (int)reader["Permissions"];



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

        public static bool GetUserInfoByUserName(string UserName, ref int UserID ,
        ref string Email, ref string Phone, ref string PasswordHash, ref int Permissions)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT * FROM Users WHERE UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    Email = (string)reader["Email"];
                    Phone = (string)reader["Phone"];
                    PasswordHash = (string)reader["PasswordHash"];
                    Permissions = (int)reader["Permissions"];



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

        public static bool GetUserInfoByEmail(string Email , ref int UserID, ref string UserName, ref string Phone, ref string PasswordHash, ref int Permissions)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT * FROM Users WHERE Email = @Email";

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
                    Permissions = (int)reader["Permissions"];



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


        public static bool GetUserInfoByUserNameAndPassword(string UserName,
            string PasswordHash, ref int UserID, ref string Email, ref string Phone, ref int Permissions)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "Select * From Users where UserName = @UserName And PasswordHash = @PasswordHash";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@PasswordHash", PasswordHash);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    Email = (string)reader["Email"];
                    Phone = (string)reader["Phone"];
                    Permissions = (int)reader["Permissions"];


                }
                else
                {
                    // The record was not found
                    isFound = false;
                }


            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }


            return isFound;
        }


        public static int AddNewUser(string UserName,
         string Email,  string Phone, string PasswordHash, int Permissions)

        {
            int UserID = -1;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string Quere = @"INSERT INTO [dbo].[Users]
           ([UserName]
           ,[Email]
           ,[Phone]
           ,[PasswordHash]
           ,[Permissions])
     VALUES (@UserName,@Email,@Phone,@PasswordHash,@Permissions)
Select SCOPE_IDENTITY();";


            SqlCommand command = new SqlCommand(Quere, connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
            command.Parameters.AddWithValue("@Permissions", Permissions);




            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    UserID = InsertedID;
                }
                else
                {
                    UserID = -1;
                }
            }
            catch (Exception ex)
            {
                UserID = -1;
            }
            finally
            {
                connection.Close();
            }
            return UserID;
        }


        public static bool UpdateUser(int UserID, string UserName,
         string Email, string Phone, string PasswordHash, int Permissions)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"
UPDATE [dbo].[Users]
   SET [UserName] = @UserName,
       [Email] = @Email,
       [Phone] = @Phone,
       [PasswordHash] = @PasswordHash,
       [Permissions] = @Permissions
 WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            // ضيف البراميترز مرة واحدة بس
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
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


        public static bool DeleteUser(int UserID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"Delete Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

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
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT Found=1 FROM Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

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
        
        public static bool IsUserNameExists(string UserName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "Select Exits = 'yes' From Users Where Users.UserName = @UserName;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

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

    }
}
