using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AlaMashi.DAL;

namespace AlaMashi.BLL
{
    public class  ResetCodeBLL
    {
            public enum enMode { AddNew = 0, Update = 1 };
            public enMode Mode = enMode.AddNew;


            public int ResetCodeID { get; set; }
            public int UserID { get; set; }
            public string Code { get; set; }
            public DateTime ExpiresAt { get; set; }
            public bool IsUsed { get; set; }




            public ResetCodeBLL()
            {
                this.ResetCodeID = -1;
                this.UserID = -1;
                this.Code = "";
                this.ExpiresAt = default;
                this.IsUsed = false;
                Mode = enMode.AddNew;
            }

            private ResetCodeBLL(int ResetCodeID, int UserID, string Code, DateTime ExpiresAt, bool IsUsed)
            {
                this.ResetCodeID = ResetCodeID;
                this.UserID = UserID;
                this.Code = Code;
                this.ExpiresAt = ExpiresAt;
                this.IsUsed = IsUsed;
                Mode = enMode.Update;
            }

            private bool _AddNewResetCode()
            {
                //call DataAccess Layer 

                this. ResetCodeID = ResetCodeDAL.AddNewResetCode(this.UserID, this.Code, this.ExpiresAt, this.IsUsed);

                return (this. ResetCodeID != -1);
            }

             private bool _UpdateResetCode()
             {
               return ResetCodeDAL.UpdateResetCode(this.ResetCodeID, this.UserID, this.Code, this.ExpiresAt, this.IsUsed);
             }

        public static  ResetCodeBLL FindByResetCodeID(int ResetCodeID)
            {
                 string Code = "";
                 int UserID = -1;
                 DateTime ExpiresAt = default;
                 bool IsUsed = false;


            if ( ResetCodeDAL.GetResetCodeInfoByID( ResetCodeID, ref UserID, ref Code, ref ExpiresAt, ref IsUsed))
                    return new  ResetCodeBLL(ResetCodeID,UserID ,Code , ExpiresAt , IsUsed);
                else
                    return null;
            }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewResetCode())
                    {
                        Mode = enMode.Update;
                    }
                    return this.UserID != -1;


                case enMode.Update:

                    return _UpdateResetCode();

            }

            return false;
        }

        public static List< ResetCodeBLL> ConvertDataTableToResetCodeList(DataTable dt)
            {
                List< ResetCodeBLL>  ResetCodes = new List< ResetCodeBLL>();

                foreach (DataRow row in dt.Rows)
                {
                ResetCodeBLL ResetCode = new ResetCodeBLL
                {
                    ResetCodeID = Convert.ToInt32(row["ResetCodeID"]),
                    UserID = Convert.ToInt32(row["UserID"]),
                    Code = row["Code"].ToString(),
                    ExpiresAt = Convert.ToDateTime(row["ExpiresAt"]),
                    IsUsed = Convert.ToBoolean(row["IsUsed"])
                };

                     ResetCodes.Add( ResetCode);
                }

                return  ResetCodes;
            }

         public static List< ResetCodeBLL> GetAllResetCodes()
            {
                return ConvertDataTableToResetCodeList( ResetCodeDAL.GetAllResetCodes());
            }
    }
}


