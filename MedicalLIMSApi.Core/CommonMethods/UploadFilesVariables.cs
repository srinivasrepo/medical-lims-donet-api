using System.Configuration;
using System.Web;

namespace MedicalLIMSApi.Core.CommonMethods
{
    public static class UploadFilesVariables
    {
        public static string ImpersonationLoginName
        {
            get
            {
                return ConfigurationManager.AppSettings["login"];
            }
        }

        public static string ImpersonationPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["pass"];
            }
        }

        public static string ImpersonationDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["domain"];
            }
        }

        public static string RootPath
        {
            get
            {
                return (IsImpersonationEnable) ? ConfigurationManager.AppSettings["ImpersonationPath"] :
                       HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["localpath"]);
            }
        }

        public static string DMSdocPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DMSpath"];
            }
        }




        public static string PDFViewer
        {
            get
            {
                return (IsImpersonationEnable) ? ConfigurationManager.AppSettings["PDFRemoteViewer"] :
                       ConfigurationManager.AppSettings["PDFLocalViewer"];
            }
        }


        public static bool IsImpersonationEnable
        {
            get
            {
                return ConfigurationManager.AppSettings["ImpEnable"] == "1";
            }
        }

    }
}
