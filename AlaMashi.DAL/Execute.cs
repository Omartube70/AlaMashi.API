using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlaMashi.DAL
{
    internal class Execute
    {
        public  static T ExecuteScalar<T>(string query, SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
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
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
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
        public static DataTable GetDataTable(string query, SqlParameter[] parameters = null)
        {
            var dt = new DataTable();
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }
        public static bool ExecuteReader(string query, SqlParameter[] parameters, Action<SqlDataReader> processRow)
        {
            bool isFound = false;
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;
                        processRow(reader); // استدعاء الدالة الممررة لمعالجة السجل
                    }
                }
            }
            return isFound;
        }

    }
}
