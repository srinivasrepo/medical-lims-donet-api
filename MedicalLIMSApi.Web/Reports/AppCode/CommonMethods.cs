using MedicalLIMSApi.Core.CommonMethods;
using System;

namespace MedicalLIMSApi.Web.Reports.AppCode
{
    public class CommonMethods : System.Web.UI.Page
    {
        protected T ReadQS<T>(string key)
        {
            CommonStaticMethods objEncrypt = new CommonStaticMethods();

            T target = default(T);
            try
            {
                target = (T)Convert.ChangeType(objEncrypt.decryptQueryString(Request.QueryString[key]), typeof(T));
            }
            catch { }

            return target;
        }

        public int ConvertToInt(object val)
        {
            int value = default(int);
            int.TryParse(Convert.ToString(val), out value);
            return value;
        }
        public decimal ConvertToDecimal(object val)
        {
            decimal value = default(decimal);
            decimal.TryParse(Convert.ToString(val), out value);
            return value;
        }
        public bool ConvertToBoolean(object val)
        {
            bool value = default(bool);
            bool.TryParse(Convert.ToString(val), out value);
            return value;

        }

        public DateTime ConvertTodateTime(object val)
        {
            DateTime value = default(DateTime);
            DateTime.TryParse(Convert.ToString(val), out value);
            return value;

        }

    }
}