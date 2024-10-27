
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Web.App_Start;
using System;
using System.Configuration;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Web.Utilities
{
    public class Common
    {
        public static byte plantID;

        public static string deptCode;

        public static int userID;

        public static string Serialize<T>(T t)
        {
            string XMLString = string.Empty;

            if (t == null)
                return XMLString;

            XmlSerializer x = null;

            try
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    x = new XmlSerializer(typeof(T));

                    x.Serialize(stringwriter, t);

                    XMLString = stringwriter.ToString();
                }
            }
            catch
            {
                XMLString = string.Empty;
            }
            finally
            {
                x = null;
            }

            return XMLString;
        }

        public static int GetPageSize()
        {
            int size = default(int);
            int.TryParse(ConfigurationManager.AppSettings["PAGE_SIZE"], out size);
            return size;
        }

        public static TransResults GetUserDetails()
        {
            TransResults trn = new TransResults();
            trn.UserID = APIContext.Current.userDetails.UserID;
            trn.RoleID = APIContext.Current.userDetails.RoleID;
            trn.DeptCode = APIContext.Current.userDetails.DeptCode;
            trn.PlantID = APIContext.Current.userDetails.PlantID;
            trn.UserRoleID = APIContext.Current.userDetails.UserRoleID;
            trn.DeptID = APIContext.Current.userDetails.DeptID;
            trn.PlantCode = APIContext.Current.userDetails.PlantCode;
            trn.RoleType = APIContext.Current.userDetails.RoleType;
            trn.SupplierID = APIContext.Current.userDetails.SupplierID;
            trn.LoginID = APIContext.Current.userDetails.LoginID;
            trn.RoleName = APIContext.Current.userDetails.RoleName;
            trn.UserToken = APIContext.Current.userDetails.ExistingUserToken;
            return trn;
        }

        public static int ParseInt(object val)
        {
            int number = default(int);
            int.TryParse(Convert.ToString(val), out number);
            return number;
        }
    }
}