using AlaMashi.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlaMashi.BLL
{
    public class AddressBLL
    {
            public enum enMode { AddNew = 0, Update = 1 };
            public enum enAddressType { home = 0, Work = 1 , Another = 2 };


            public int AddressID { get; private set; }
            public int UserID { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string AddressDetails { get; set; } 
            public enAddressType AddressType { get; set; }
            public enMode Mode { get; private set; }

            public AddressBLL()
            {
                AddressID = -1;
                UserID = -1;
                Street = "";
                City = "";
                AddressDetails = "";
                AddressType = enAddressType.Another;
                Mode = enMode.AddNew;
            }

            private AddressBLL(int AddressID,int UserID, string Street, string City, string AddressDetails, enAddressType AddressType)
            {
                this.AddressID = AddressID;
                this.UserID = UserID;
                this.Street = Street;
                this.City = City;
                this.AddressDetails = AddressDetails;
                this.AddressType = AddressType;
                this.Mode = enMode.Update;
            }

            private bool _AddNewAddress()
            {
                this.AddressID =AddressDAL.AddNewAddress(UserID, Street, City, AddressDetails, (int)AddressType);

                return (this.AddressID > 0);
            }

            private bool _UpdateAddress()
            {
                return AddressDAL.UpdateAddress(AddressID, Street, City, AddressDetails, (int)AddressType);
            }

            public static AddressBLL GetAddressByAddressID(int AddressID)
            {
                string Street = "", City = "", AddressDetails = "";
                int UserID = 0 , AddressType = 0;

                bool IsFound = AddressDAL.GetAddressInfoByID(AddressID, ref UserID, ref Street, ref City, ref AddressDetails, ref AddressType);

                if (IsFound)
                {
                    return new AddressBLL(AddressID, UserID, Street, City, AddressDetails, (enAddressType)AddressType);
                }

                return null;
            }

            public bool Save()
            {
                // Validation
                if (string.IsNullOrEmpty(Street) || string.IsNullOrEmpty(City))
                {
                    throw new ArgumentException("All required fields must be filled.");
                }

                if(UserBLL.isUserExist(UserID) == false)
                {
                    throw new ArgumentException("User does not exist.");
                }


                if (Mode == enMode.AddNew)
                {
                    if (_AddNewAddress())
                    {
                        Mode = enMode.Update;
                        return true;
                    }

                    throw new ArgumentException("Failed To Save");
                }
                else // Mode == enMode.Update
                {
                    return _UpdateAddress();
                }
            }

            public bool Delete()
            {
                return AddressDAL.DeleteAddress(this.AddressID);
            }

            public static bool DeleteAddress(int AddressID)
            {
                return AddressDAL.DeleteAddress(AddressID);
            }

            public static List<AddressBLL> _ConvertDataTableToList(DataTable dt)
            {
                List<AddressBLL> Address = new List<AddressBLL>();

                foreach (DataRow row in dt.Rows)
                {
                   AddressBLL Addressbll = new AddressBLL()
                  {
                    AddressID = Convert.ToInt32(row["AddressID"]),
                    UserID = Convert.ToInt32(row["UserID"]),
                    Street = row["Street"].ToString(),
                    City = row["City"].ToString(),
                    AddressDetails = row["AddressDetails"] == DBNull.Value ? string.Empty : row["AddressDetails"].ToString(),
                    AddressType = (enAddressType)Convert.ToInt32(row["AddressType"])
                  };
                   Address.Add(Addressbll);
                }

                return Address;
            }

            public static List<AddressBLL> GetAllAddress()
            {
                DataTable dt =AddressDAL.GetAllAddress();

                return _ConvertDataTableToList(dt);
            }

            public static List<AddressBLL> GetAllAddressByUserID(int UserID)
            {
                DataTable dt = AddressDAL.GetAllAddressByUserID(UserID);
            
                return _ConvertDataTableToList(dt);
            }

            public static bool isAddressExist(int AddressID)
            {
                return AddressDAL.IsAddressExist(AddressID);
            }
    }
    
}
