using System;
using System.IO;
using System.Web;
using MedicalLIMSApi.Core.Entities.Common;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Text;
using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Infrastructure.ReportDAL;
using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.UtilUploads;

namespace MedicalLIMSApi.Web.Utilities
{
    public class FileUploadUtility
    {
        UploadDetails details;
        HttpPostedFile fileToUpload;
        string basePath;
        string fileNameToUpload;
        string fileWithFullPath;
        public string browsePath;

        public FileUploadUtility(string source)
        {
            details = GetUploadDetails(source);

            if (details == null)
                throw new MedicalLIMSInvalidSourceException("Invalid Source");
        }

        public FileUploadUtility(string source, HttpPostedFile file, string bsPath)
        {
            details = GetUploadDetails(source);

            if (details == null)
                throw new MedicalLIMSInvalidSourceException("Invalid Source");

            fileToUpload = file;

            basePath = bsPath;

            ValidateUpload();

            PrepareFileName();

            UploadeFile();
        }

        void UploadeFile()
        {
            try
            {
                ImpersonationHelper.Impersonate(delegate
                {
                    if (!System.IO.Directory.Exists(basePath))
                        System.IO.Directory.CreateDirectory(basePath);

                    fileWithFullPath = Path.Combine(basePath, fileNameToUpload);

                    browsePath = string.Format("{0}/{1}", details.FilePath, fileNameToUpload);

                    fileToUpload.SaveAs(fileWithFullPath);
                });
            }
            catch
            {
                throw new MedicalLIMSInvalidSourceException("Unable to Upload File");
            }
        }

        void ValidateUpload()
        {
            //get uploaded file extension
            string extn = Path.GetExtension(fileToUpload.FileName).ToLower();

            // validate against eligible extensions
            string[] strFileNames = details.Extension.Split(',');

            bool isValidExt = Array.IndexOf(strFileNames, extn) >= 0;

            if (!isValidExt)
                throw new MedicalLIMSInvalidSourceException("Invalid File Type; allows only " + details.Extension);
        }

        void PrepareFileName()
        {
            fileNameToUpload = System.IO.Path.GetFileName(fileToUpload.FileName);

            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                fileNameToUpload = fileNameToUpload.Replace(c.ToString(), "");
            }

            fileNameToUpload = Regex.Replace(fileNameToUpload, "^[-_.A-Za-z0-9]$", "_");
            fileNameToUpload = fileNameToUpload.Replace(" ", "_");
            basePath = Path.Combine(basePath, details.FilePath);
            string originalName = fileNameToUpload;
            int srNO = 1;
            ImpersonationHelper.Impersonate(delegate
            {
                while (true)
                {
                    if (!File.Exists(Path.Combine(basePath, fileNameToUpload)))
                        break;
                    else
                    {
                        fileNameToUpload = string.Format("{0}_{1}", srNO, originalName);
                        srNO++;
                    }

                }
            });
            if (fileNameToUpload.Length > 250)
                throw new MedicalLIMSInvalidSourceException("File Name cannot be greater than 250 characters");
        }

        UploadDetails GetUploadDetails(string entityCode)
        {
            UploadDetails Details = new UploadDetails();

            Details.FileSize = ConfigurationManager.AppSettings["UploadFileSize"].ToString();

            entityCode = entityCode.ToUpper();

            switch (entityCode)
            {

                default:
                    //Details = null;
                    Details.Type = FileTypes.BOTH;
                    Details.FilePath = entityCode;
                    break;
            }

            return Details;
        }

        enum FileTypes
        {
            // Only Images
            IMAGE,

            //Only Files
            FILE,

            // Only PDF files 
            PDF_ONLY,

            // Both image and file types
            BOTH
        }

        public enum UploadMode
        {
            MANAGE,

            VIEW
        }

        public static string PrepareDocumentXML(string doc)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(doc))
            {
                sb.Append("<RT>");

                string[] str = doc.Split(',');

                foreach (string s in str)
                {
                    if (!string.IsNullOrEmpty(s))
                        sb.AppendFormat("<DOC><DOCID>{0}</DOCID></DOC>", s);
                }

                sb.Append("</RT>");
            }

            return sb.ToString();
        }

        class UploadDetails
        {
            public string Extension
            {
                get
                {
                    return GetExtensionsByType();
                }
            }

            public string FilePath { get; set; }

            public string FileSize { get; set; }

            public FileTypes Type { get; set; }

            public string Success { get; set; }

            string GetExtensionsByType()
            {
                string type = string.Empty;

                switch (Type)
                {
                    case FileTypes.FILE:
                        type = ConfigurationManager.AppSettings["FILE_EXTENSIONS"];
                        break;
                    case FileTypes.IMAGE:
                        type = ConfigurationManager.AppSettings["IMAGE_EXTENSIONS"];
                        break;
                    case FileTypes.PDF_ONLY:
                        type = ".pdf";
                        break;
                    case FileTypes.BOTH:
                        type = string.Format("{0},{1}", ConfigurationManager.AppSettings["FILE_EXTENSIONS"], ConfigurationManager.AppSettings["IMAGE_EXTENSIONS"]);
                        break;

                    default:
                        break;
                }

                return type;
            }
        }

        public static string UploadReportInfoToDMS(ReportUploadDMS obj)
        {
            RdlcUploadedBO retObj = new RdlcUploadedBO();
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            obj.AppCode = tr.ApplicationType;
            obj.DeptCode = tr.DeptCode;
            obj.PlantCode = tr.PlantCode;
            obj.LoginID = tr.LoginID;
            obj.RoleName = tr.RoleName;
            retObj = CommonStaticMethods.PostApiConnectionData<RdlcUploadedBO>("UploadReportsInfo", obj, "DMS_URL");
            if(retObj.ReturnFlag == "OK" && retObj.Lst != null && retObj.Lst.Count > 0)
                retObj.ReturnFlag = ReportDAL.UploadReportDMSIDs(CommonStaticMethods.Serialize<UploadedRdlcList>(retObj.Lst), tr);
            return retObj.ReturnFlag;
        }

        public static string SynReportInfoToDMS(ReportUploadDMS obj)
        {
            RdlcUploadedBO retObj = new RdlcUploadedBO();
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            obj.AppCode = tr.ApplicationType;
            obj.DeptCode = tr.DeptCode;
            obj.PlantCode = tr.PlantCode;
            obj.LoginID = tr.LoginID;
            obj.RoleName = tr.RoleName;
            retObj = CommonStaticMethods.PostApiConnectionData<RdlcUploadedBO>("SyncReportsInfo", obj, "DMS_URL");
            if (retObj.ReturnFlag == "OK" && retObj.Lst != null && retObj.Lst.Count > 0)
                retObj.ReturnFlag = ReportDAL.UploadReportDMSIDs(CommonStaticMethods.Serialize<UploadedRdlcList>(retObj.Lst), tr);
            return retObj.ReturnFlag;
        }

        public static string UploadReportToDMS(ReportUploadDMS obj)
        {
            RdlcUploadedBO retObj = new RdlcUploadedBO();
            retObj = CommonStaticMethods.PostApiConnectionData<RdlcUploadedBO>("UploadRDLCReport", obj, "DMS_URL");
            return retObj.ReturnFlag;
        }

        public static ReportDetails GetCalibrationReportDetails(ArdsBO obj, TransResults tr)
        {
            return ReportDAL.GetCalibrationReportDetails(obj, tr);
        }

        public static string UpdateArdsPlaceholders(UpdateARDSPlaceholder obj, TransResults tr)
        {
            return ReportDAL.UpdateArdsPlaceholders(obj, tr);
        }
    }
}