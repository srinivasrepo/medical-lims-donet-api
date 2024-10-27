using System.Collections.Generic;
using System.Linq;

namespace MedicalLIMSApi.Web.Utilities
{
    public class SingleLoginApplicationCache
    {
        public static Dictionary<string, string> SingleUsrList = new Dictionary<string, string>();

        public static void setLoginData(Dictionary<string, string> obj)
        {
            SingleUsrList = SingleUsrList.Concat(obj)
                       .ToDictionary(x => x.Key, x => x.Value);
        }

        public static Dictionary<string, string> GetLoginData()
        {
            return SingleUsrList;
        }
    }
}