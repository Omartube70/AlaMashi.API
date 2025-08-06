using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlaMashi.DAL
{
    public class ResetCodeDAL
    {
        public static bool GetResetCodeInfoByID(int ResetCodeID, ref int UserID,
        ref string Code, ref DateTime ExpiresAt, ref bool IsUsed)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT * FROM ResetCodes WHERE ResetCodeID = @ResetCodeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ResetCodeID", ResetCodeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    Code = (string)reader["Code"];
                    ExpiresAt = (DateTime)reader["ExpiresAt"];
                    IsUsed = (bool)reader["PasswordHash"];


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

        public static int AddNewResetCode(int UserID,
         string Code, DateTime ExpiresAt, bool IsUsed)
        {
            int ResetCodeID = -1;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string Quere = @"INSERT INTO [dbo].[ResetCodes]
           ([UserID]
           ,[Code]
           ,[ExpiresAt]
           ,[IsUsed])
     VALUES (@UserID,@Code,@ExpiresAt,@IsUsed)
Select SCOPE_IDENTITY();";


            SqlCommand command = new SqlCommand(Quere, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@Code", Code);
            command.Parameters.AddWithValue("@ExpiresAt", ExpiresAt);
            command.Parameters.AddWithValue("@IsUsed", IsUsed);




            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    ResetCodeID = InsertedID;
                }
                else
                {
                    ResetCodeID = -1;
                }
            }
            catch (Exception ex)
            {
                ResetCodeID = -1;
            }
            finally
            {
                connection.Close();
            }
            return ResetCodeID;
        }

        public static DataTable GetAllResetCodes()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "Select * From ResetCodes";

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

        public static bool UpdateResetCode(int ResetCodeID,int UserID, string Code,
        DateTime ExpiresAt, bool IsUsed)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"
UPDATE [dbo].[ResetCodes]
   SET [UserID] = @UserID,
       [Code] = @Code,
       [ExpiresAt] = @ExpiresAt,
       [IsUsed] = @IsUsed
 WHERE ResetCodeID = @ResetCodeID";

            SqlCommand command = new SqlCommand(query, connection);

            // ضيف البراميترز مرة واحدة بس
            command.Parameters.AddWithValue("@ResetCodeID", ResetCodeID);
            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@Email", Code);
            command.Parameters.AddWithValue("@ExpiresAt", ExpiresAt);
            command.Parameters.AddWithValue("@IsUsed", IsUsed);



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


    }
}
