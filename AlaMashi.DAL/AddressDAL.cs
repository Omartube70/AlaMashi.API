using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlaMashi.DAL
{
    public class AddressDAL
    {
        public static bool GetAddressInfoByID(int AddressID, ref int UserID, ref string Street, ref string City, ref string AddressDetails, ref int AddressType)
        {
            string query = "SELECT * FROM Address WHERE AddressID = @AddressID";
            var parameter = new SqlParameter("@AddressID", SqlDbType.Int) { Value = AddressID };

            // الخطوة 1: تعريف متغيرات وسيطة (محلية)
            int tempUserID = 0;
            string tempStreet = "";
            string tempCity = "";
            string tempAddressDetails = "";
            int tempAddressType = 0;

            // استدعِ الدالة وخزن نتيجتها (true/false)
            bool isFound = Execute.ExecuteReader(query, new[] { parameter }, (reader) =>
            {
                // الخطوة 2: استخدام المتغيرات الوسيطة داخل اللامدا
                // هذا الكود فقط هو ما يتم تشغيله إذا تم العثور على سجل
                tempUserID = (int)reader["UserID"];
                tempStreet = reader["Street"].ToString();
                tempCity = reader["City"].ToString();
                tempAddressDetails = reader["AddressDetails"] == DBNull.Value ? string.Empty : reader["AddressDetails"].ToString();
                tempAddressType = (int)reader["AddressType"];
            });

            // الخطوة 3: تحديث متغيرات ref بعد التنفيذ
            if (isFound)
            {
                UserID = tempUserID;
                Street = tempStreet;
                City = tempCity;
                AddressDetails = tempAddressDetails;
                AddressType = tempAddressType;
            }

            return isFound;
        }

        public static int AddNewAddress(int UserID, string Street, string City, string AddressDetails, int AddressType)
        {
            string query = @"
                INSERT INTO Address (UserID, Street, City, AddressDetails, AddressType)
                VALUES (@UserID, @Street, @City, @AddressDetails, @AddressType);
                SELECT SCOPE_IDENTITY();";

            var parameters = new[]
            {
                new SqlParameter("@UserID", SqlDbType.Int) { Value = UserID },
                new SqlParameter("@Street", SqlDbType.NVarChar, 255) { Value = Street },
                new SqlParameter("@City", SqlDbType.NVarChar, 100) { Value = City },
                new SqlParameter("@AddressDetails", SqlDbType.NVarChar, 255) { Value = string.IsNullOrEmpty(AddressDetails) ? DBNull.Value : AddressDetails },         
                new SqlParameter("@AddressType", SqlDbType.Int) { Value = AddressType }
            };

            return Execute.ExecuteScalar<int>(query, parameters);
        }

        public static bool UpdateAddress(int addressID, string street, string city, string addressDetails, int addressType)
        {
            string query = @"
        UPDATE Addresses
        SET Street = @Street, 
            City = @City,
            AddressDetails = @AddressDetails, 
            AddressType = @AddressType
        WHERE AddressID = @AddressID"; 

            var parameters = new[]
            {
        new SqlParameter("@AddressID", addressID),
        new SqlParameter("@Street", street),
        new SqlParameter("@City", city),
        new SqlParameter("@AddressDetails", addressDetails),
        new SqlParameter("@AddressType", addressType)
        // تم حذف parameter الـ UserID
          };

            return Execute.ExecuteNonQuery(query, parameters) > 0;
        }

        public static bool DeleteAddress(int AddressID)
        {
            string query = "DELETE Address WHERE AddressID = @AddressID";
            var parameter = new SqlParameter("@AddressID", SqlDbType.Int) { Value = AddressID };

            return Execute.ExecuteNonQuery(query, new[] { parameter }) > 0;
        }

        public static DataTable GetAllAddress()
        {
            string query = "SELECT * FROM Address";

            return Execute.GetDataTable(query);
        }

        public static DataTable GetAllAddressByUserID(int UserID)
        {
            string query = "SELECT * FROM Address WHERE UserID = @UserID";
            var parameter = new SqlParameter("@UserID", SqlDbType.Int) { Value = UserID };

            // افترض أن لديك دالة جاهزة في Execute تقوم بالعملية كلها
            return Execute.GetDataTable(query, new[] { parameter });
        }

        public static bool IsAddressExist(int AddressID)
        {
            string query = "SELECT COUNT(1) FROM Address WHERE AddressID = @AddressID";
            var parameter = new SqlParameter("@AddressID", SqlDbType.Int) { Value = AddressID };

            return Execute.ExecuteScalar<int>(query, new[] { parameter }) > 0;
        }

    }
}
