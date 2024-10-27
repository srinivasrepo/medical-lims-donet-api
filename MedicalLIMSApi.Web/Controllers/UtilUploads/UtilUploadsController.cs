using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SampleAnalysis;
using MedicalLIMSApi.Core.Entities.Utilities;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using MedicalLIMSApi.Core.Interface.UtilUploads;
using MedicalLIMSApi.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.UtilUploads
{
    public class UtilUploadsController : ApiController
    {
        IUtilUploads db;
        string rootPath = UploadFilesVariables.RootPath;
        StringBuilder sbError = null;

        public UtilUploadsController(IUtilUploads db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("DownloadDoc")]
        public void DownloadDoc(DownloadDoc obj)
        {
            String downloadFileName = obj.Path.Substring(obj.Path.LastIndexOf("/"));

            try
            {
                ImpersonationHelper.Impersonate(delegate
                {
                    if (File.Exists(obj.Path))
                    {
                        byte[] file = File.ReadAllBytes(obj.Path);
                        using (Stream stream = new MemoryStream(file))
                        {
                            using (BinaryReader br = new BinaryReader(stream))
                            {
                                file = br.ReadBytes((int)file.Length);
                            }
                        }
                        System.Web.UI.Page P = new System.Web.UI.Page();
                        P.Response.Clear();
                        P.Page.Response.ClearContent();
                        P.Page.Response.ClearHeaders();
                        P.Page.Response.AddHeader("content-disposition", "attachment; filename= \"" + downloadFileName + "\"");
                        P.Page.Response.ContentType = "APPLICATION/OCTET-STREAM";
                        P.Page.Response.BinaryWrite(file);
                        P.Page.Response.Flush();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        file = null;
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        [Route("GetUploadDocs")]
        public IEnumerable<UtilUpload> GetUploadDocs(getUplodedFiles obj)
        {
            var lst = new List<UtilUpload>();

            if (obj.Type == "MEDICAL_LIMS")
            {
                TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();

                var baseBO = new BaseDocumentBO();
                baseBO.AppCode = trn.ApplicationType;
                baseBO.DeptCode = trn.DeptCode;
                baseBO.EntityCode = obj.EntityCode;
                baseBO.LoginID = trn.LoginID;
                baseBO.PlantCode = trn.PlantCode;
                baseBO.RefNumber = obj.RefNumber;
                baseBO.SectionCode = obj.Section;
                baseBO.EntActID = CommonStaticMethods.Decrypt<int>(obj.EncryptedKey);
                baseBO.DMSTempIDLst = obj.FileUploadedIDs;

                lst = CommonStaticMethods.PostApiConnectionData<List<UtilUpload>>("GetFiles", baseBO, "DMS_URL");
            }
            else
            {
                obj.EntityActID = EncryptingString.Decrypt<int>(obj.EncryptedKey);
                lst = db.GetDocuments(obj);
            }

            return lst;

        }

        [HttpPost]
        [Route("DeleteDocument")]
        public string DeleteDocument(UploadFileBO obj)
        {
            return DeleteUploadedDocument(obj);
        }

        [HttpPost]
        [Route("UploadFiles")]
        public UploadFileID UploadFiles(string entCode, string entActID, string section, string docType, int dmsID, string referenceNumber, string role, int documentTrackID, string docName)
        {

            int encActID = default(int);
            var upd = new FileUploadData();
            var file = new UploadFileID();
            TransResults tran = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            if (!string.IsNullOrEmpty(entActID) && entActID != "null")
                encActID = EncryptingString.Decrypt<int>(entActID);
            tran.TransKey = encActID;

            var httpRequest = HttpContext.Current.Request;

            if (section == "DMS_REPORT") // Sample Analysis Ards Report
            {
                if (!string.IsNullOrEmpty(referenceNumber))
                {
                    //upd.FileName = Regex.Replace(referenceNumber, "^[-_.A-Za-z0-9]$", "_");
                    //upd.FileName = upd.FileName.Replace(" ", "_") + ".docx";
                }

                if (httpRequest.Files.Count > 0)
                {
                    byte[] buffer = new byte[16 * 1024];

                    upd.ActFileName = httpRequest.Files[0].FileName;


                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = httpRequest.Files[0].InputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        buffer = ms.ToArray();
                    }


                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["DMS_URL"]);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        using (var content = new MultipartFormDataContent())
                        {
                            var fileContent = new ByteArrayContent(buffer);
                            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = upd.ActFileName
                            };
                            content.Add(fileContent);
                            string Url = string.Format("UploadReport?DMSId={0}&LoginID={1}&Role={2}&AppCode={3}", dmsID, tran.LoginID, role, tran.ApplicationType);
                            HttpResponseMessage resp = client.PostAsync(Url, content).Result;
                            file.ReturnFlag = resp.Content.ReadAsAsync<string>().Result;
                            file.ResultFlag = file.ReturnFlag;
                        }
                    }

                }
            }
            else if (documentTrackID > default(int))
            {
                getUplodedFiles obj = new getUplodedFiles();
                obj.AppCode = tran.ApplicationType;
                obj.PlantCode = tran.PlantCode;
                obj.DeptCode = tran.DeptCode;
                obj.LoginID = tran.LoginID;
                obj.EntityCode = entCode;
                obj.SectionCode = section;
                obj.RefNumber = referenceNumber;
                obj.FileName = docName.Replace('/', '_');
                obj.DocumentTrackID = documentTrackID;
                obj.Role = role;
                obj.EntActID = encActID;

                file = CommonStaticMethods.PostApiConnectionData<UploadFileID>("UploadDocument", obj, "DMS_URL");


                file.ResultFlag = file.ReturnFlag;
            }
            else
            {
                string fileName = string.Empty;
                if (httpRequest.Files.Count > 0)
                {

                    upd.ActFileName = httpRequest.Files[0].FileName;

                    byte[] buffer = new byte[16 * 1024];

                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = httpRequest.Files[0].InputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        buffer = ms.ToArray();
                    }

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["DMS_URL"]);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        using (var content = new MultipartFormDataContent())
                        {
                            var fileContent = new ByteArrayContent(buffer);
                            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = upd.ActFileName
                            };
                            content.Add(fileContent);
                            string url = string.Format("UploadFile?AppCode={0}&PlantCode={1}&EntityCode={2}&SectionCode={3}&DepartmentCode={4}&LoginID={5}&EntityActualID={6}&ReferenceNumber={7}&FileName={8}&Role={9}", tran.ApplicationType, tran.PlantCode, entCode, section, tran.DeptCode, tran.LoginID, encActID, referenceNumber, upd.ActFileName, role);

                            HttpResponseMessage resp = client.PostAsync(url, content).Result;

                            file = CommonStaticMethods.ChangeObjectType<UploadFileID>(resp);
                            file.ResultFlag = file.ReturnFlag;
                        }
                    }
                }
                else
                {
                    file.ResultFlag = "NOFILES";
                }
            }

            return file;
        }

        [HttpPost]
        [Route("fileDownload")]
        public string FileDownload(FileDownload obj)
        {
            string base64String = string.Empty;

            if (obj.Type == "MEDICAL_LIMS")
            {
                var viewObj = new UploadFileBO();
                viewObj.DMSID = obj.ReportID;
                viewObj.AppCode = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().ApplicationType;

                base64String = CommonStaticMethods.PostApiConnectionData<string>("DownloadFile", viewObj, "DMS_URL");
            }
            else
            {
                WebClient wc = new WebClient();
                wc.Credentials = new NetworkCredential(UploadFilesVariables.ImpersonationLoginName, UploadFilesVariables.ImpersonationPassword, UploadFilesVariables.ImpersonationDomain);
                byte[] originalArray = null;

                try
                {
                    if (obj.EntityActID > default(int))
                    {
                        obj.Action = "DOWNLOAD";
                        UploadedFileDetails fileObj = db.GetFileDetails(obj);
                        obj.DocumentName = fileObj.DocumentName;
                    }

                    string folderPath = UploadFilesVariables.RootPath + obj.DocumentName;
                    originalArray = wc.DownloadData(folderPath);
                    base64String = Convert.ToBase64String(originalArray);

                }
                catch (Exception ex)
                {
                    LogException(ex);
                    //return sbError.ToString();
                    base64String = sbError.ToString();
                }
                finally
                {
                    wc = null;
                    originalArray = null;
                }

            }

            return base64String;
        }

        [HttpPost]
        [Route("ARDSSelectionPrint")]
        public TransResultApprovals ARDSSelectionPrint(ARDSManageRequest obj)
        {

            TransResultApprovals res = new TransResultApprovals();
            UploadedDocBo upObj = GenerateRequestForPrint(obj);

            // When user going for Print, consider by default as ARDS applicable

            if (upObj.ReturnFlag == "Success")
            {
                obj.DocPath = upObj.DocPath;
                obj.DMSID = upObj.DMSTempID;

                res = db.ARDSManageRequest(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
            }
            else
                res.ReturnFlag = upObj.ReturnFlag;

            return res;
        }

        [HttpPost]
        [Route("ContainerARDSSelectionPrint")]
        public TransResultApprovals ContainerARDSSelectionPrint(SaveContainerArdsDetails obj)
        {

            TransResultApprovals res = new TransResultApprovals();

            ARDSManageRequest ardsBO = new ARDSManageRequest();
            ardsBO.Type = obj.ReqType;
            ardsBO.EncEntityActID = obj.EncEntityActID;
            ardsBO.EntityCode = obj.EntityCode;
            ardsBO.SectionCode = obj.SectionCode;
            ardsBO.Role = obj.Role;
            ardsBO.SpecID = obj.SpecID;
            ardsBO.EncArdsID = obj.EncArdsID;

            UploadedDocBo upObj = GenerateRequestForPrint(ardsBO);

            // When user going for Print, consider by default as ARDS applicable

            if (upObj.ReturnFlag == "Success")
            {
                obj.DocPath = upObj.DocPath;
                obj.DmsID = upObj.DMSTempID;


                res = db.SaveContainerArdsDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
            }
            else if (upObj.ReturnFlag != "ARDS_REFRESH_ACT_DOC")
                res.ReturnFlag = "ERROR";
            else
                res.ReturnFlag = upObj.ReturnFlag;

            return res;
        }

        private UploadedDocBo GenerateRequestForPrint(ARDSManageRequest obj)
        {
            string docPath = string.Empty;
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            obj.PlantID = tr.PlantID;
            ARDSPrintDocHeaderBO docHeaders = db.GetDocumentHeaderData(obj);
            docHeaders.EntityCode = obj.EntityCode;
            UploadedDocBo upObj = new UploadedDocBo();

            if (string.IsNullOrEmpty(docHeaders.InitialDocPath))
            {
                upObj.ReturnFlag = "ARDS_REFRESH_ACT_DOC";
                return upObj;
            }

            CommonStaticMethods.GetPlaceHolder(obj.EntityCode, docHeaders.Placeholders);

            upObj.ReturnFlag = UpdateArdsPlaceholders(obj.EncEntityActID, obj.EntityCode, docHeaders.Placeholders.PlaceholderList);
            if (upObj.ReturnFlag != "SUCCESS")
                return upObj;

            if (docHeaders.EntityCode == "ENGGMNT")
                docPath = GenerateFileName(docHeaders.Placeholders.CalibrationValues.CalibReferenceNumber, docHeaders.Placeholders.CalibrationValues.InstrumentCode, docHeaders.PlantCode, obj.Type, Convert.ToInt32(docHeaders.DocTrackID));
            else
                docPath = GenerateFileName(docHeaders.Placeholders.SampleAnalysisValues.ArNumber, docHeaders.Placeholders.SampleAnalysisValues.BatchNumber, docHeaders.PlantCode, obj.Type, Convert.ToInt32(docHeaders.DocTrackID));

            DocumentRecord docObj = new DocumentRecord();
            docObj.PrintObj = docHeaders;
            docObj.DocumentID = Convert.ToInt32(docHeaders.DocTrackID);
            docObj.FileName = docPath;
            docObj.AppCode = tr.ApplicationType;
            docObj.PlantCode = tr.PlantCode;
            docObj.EntityCode = obj.EntityCode;
            docObj.SectionCode = obj.SectionCode;
            docObj.DeptCode = tr.DeptCode;
            docObj.LoginID = tr.LoginID;
            docObj.EntActID = obj.EntityActID;
            if (docHeaders.EntityCode == "ENGGMNT")
                docObj.EntityReportCode = docHeaders.Placeholders.CalibrationValues.CalibReferenceNumber;
            else
                docObj.EntityReportCode = docHeaders.Placeholders.SampleAnalysisValues.ArNumber;
            docObj.Role = obj.Role;
            docObj.PlaceholderList = docHeaders.Placeholders.PlaceholderList;
            upObj = CommonStaticMethods.PostApiConnectionData<UploadedDocBo>("GenerateARDSPrintDocument", docObj, "DMS_URL");
            return upObj;
        }

        private string GenerateFileName(string arNum, string batchNUM, string plantCode, string type, int doctTrackID)
        {
            string fileName = string.Empty;
            arNum = arNum.Replace("/", "_");

            if (!string.IsNullOrEmpty(batchNUM))
                batchNUM = batchNUM.Replace("/", "_");
            else
                batchNUM = "";

            string finalDocPath = (type == "VIEW") ? ConfigurationManager.AppSettings["ARDSTempPath"] : ConfigurationManager.AppSettings["ARDSFinalPath"] + plantCode + "/";
            string Path = string.Empty;
            fileName = arNum + "_" + batchNUM + "_" + doctTrackID.ToString();
            return fileName;
        }

        private void SaveUploadFile(string originalFilePath, string currentPath)
        {
            Aspose.Words.License wordLicense = new Aspose.Words.License();
            wordLicense.SetLicense("Aspose.Words.lic");

            string docText = string.Empty;

            Aspose.Words.Document document = new Aspose.Words.Document(originalFilePath);
            docText = document.GetText();
            document.AcceptAllRevisions();
            document.Save(currentPath);
        }

        void LogException(Exception ex)
        {
            sbError = new StringBuilder();

            sbError.Append(Environment.NewLine);
            sbError.AppendLine("********************************************************************************");

            PrepareException(ex);

            sbError.Append(Environment.NewLine);

            //File.AppendAllText(fullPath, sbError.ToString());
        }


        void PrepareException(Exception ex)
        {
            sbError.Append(Environment.NewLine);
            sbError.Append(ex.Message);

            if (ex.InnerException != null)
                PrepareException(ex.InnerException);
        }

        [HttpPost]
        [Route("GetCalibrationReportDetails")]
        public GetReportData GetCalibrationReportDetails(ArdsBO obj)
        {
            return GenerateDocument(obj);
        }


        // Generate Report ARDS
        private GetReportData GenerateDocument(ArdsBO obj)
        {
            GetReportData rpt = new GetReportData();
            ReportDetails rptObj = new ReportDetails();
            rptObj = FileUploadUtility.GetCalibrationReportDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            rptObj.LoginID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().LoginID;

            CommonStaticMethods.GetPlaceHolder(obj.EntityCode, rptObj.PlaceHolders);

            rpt.StatusCode = UpdateArdsPlaceholders(Convert.ToString(obj.ArdsExecID), obj.EntityCode, rptObj.PlaceHolders.PlaceholderList);
            if (rpt.StatusCode != "SUCCESS")
                return rpt;
            try
            {
                rptObj.PlaceholderList = rptObj.PlaceHolders.PlaceholderList;
                CommonStaticMethods.PrepareARDSTables(rptObj);
                rpt = CommonStaticMethods.PostApiConnectionData<GetReportData>("GenerateReport", rptObj, "DMS_URL");

                if (rpt.Message == "Success" && rpt.DMSReportID > default(int))
                {
                    obj.ReportID = rpt.DMSReportID;
                    rpt.StatusCode = db.UpdateReportID(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
                }
                else
                    rpt.StatusCode = rpt.Message;
            }
            catch (Exception ex)
            {
                rpt.StatusCode = "ERROR";
            }

            return rpt;
        }

        [HttpPost]
        [Route("UpdatePlaceholderValues")]
        public string UpdatePlaceholderValues(ArdsBO obj)
        {
            string retCode = string.Empty;
            ReportDetails rptObj = new ReportDetails();
            rptObj = FileUploadUtility.GetCalibrationReportDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            rptObj.LoginID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().LoginID;

            CommonStaticMethods.GetPlaceHolder(obj.EntityCode, rptObj.PlaceHolders);

            retCode = UpdateArdsPlaceholders(Convert.ToString(obj.ArdsExecID), obj.EntityCode, rptObj.PlaceHolders.PlaceholderList);
            if (retCode != "SUCCESS")
                return retCode;
            try
            {
                rptObj.PlaceholderList = rptObj.PlaceHolders.PlaceholderList;
                CommonStaticMethods.PrepareARDSTables(rptObj);
                retCode = CommonStaticMethods.PostApiConnectionData<string>("UpdatePlaceHolders", rptObj, "DMS_URL");
            }
            catch (Exception ex)
            {
                retCode = "ERROR";
            }

            return retCode;
        }

        [HttpPost]
        [Route("ManageViewResetReport")]
        public GetReportData ManageViewResetReport(ViewReesetReportDetailsBO obj)
        {
            var lst = new GetReportData();

            if (obj.Type == "RESET") // Reset Report Document
            {
                // Delete Document

                var fileBO = new UploadFileBO();
                fileBO.Section = obj.Section;
                fileBO.Role = obj.Role;

                lst.StatusCode = DeleteUploadedDocument(fileBO);

                if (lst.StatusCode == "Success")
                {
                    var ardsBO = new ArdsBO();

                    ardsBO.ArdsExecID = obj.ArdsExecID;
                    ardsBO.EntityCode = obj.EntityCode;

                    lst = GenerateDocument(ardsBO);
                }
            }
            else
            {
                lst.StatusCode = "SUCCESS";
                var viewObj = new UploadFileBO();
                viewObj.DMSID = obj.ReportID;
                viewObj.AppCode = obj.AppCode;


                lst.Message = CommonStaticMethods.PostApiConnectionData<string>("ViewFile", viewObj, "DMS_URL");
            }

            return lst;
        }

        private Dictionary<string, string> ConvertListToDictinory(List<XmlPlaceHolder> placeHolderList)
        {

            var dictionaryList = new Dictionary<string, string>();

            foreach (var item in placeHolderList)
            {

                if (dictionaryList.Where(x => x.Key == item.name).ToList().Count > 0)
                    continue;

                dictionaryList.Add(item.name, item.value);
            }

            return dictionaryList;
        }

        private string DeleteUploadedDocument(UploadFileBO obj)
        {
            string resultFlag = string.Empty;

            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();

            //if (obj.Section == "ARDS_REPRT")
            {
                obj.AppCode = tr.ApplicationType;
                obj.LoginID = tr.LoginID;

                resultFlag = CommonStaticMethods.PostApiConnectionData<string>("DeleteFile", obj, "DMS_URL");
            }
            return resultFlag;
        }

        [HttpGet]
        [Route("GetFileName")]
        public string GetFilePath(int ReportID, string Type)
        {
            return CommonStaticMethods.GetApiConnectionData<string>(string.Format("GetFileName?ReportID={0}&Type={1}", ReportID, Type), "DMS_URL");
        }

        [HttpPost]
        [Route("ViewFile")]
        public string ViewFile(UploadFileBO obj)
        {
            string path = string.Empty;
            obj.AppCode = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().ApplicationType;
            if (obj.EntityCode == "INVALIDATIONS" && obj.Section == "REPORT" && string.IsNullOrEmpty(obj.Type))
                obj.Lst = db.GetCumulativeInvalidationInfo(obj.EntityActID, "INVALIDATION");
            //obj.Type = "File";
            path = CommonStaticMethods.PostApiConnectionData<string>("ViewFile", obj, "DMS_URL");

            return path;
        }

        string UpdateArdsPlaceholders(string encEntActID, string entityCode, PlaceholderList lst)
        {
            UpdateARDSPlaceholder obj = new UpdateARDSPlaceholder();
            obj.EncEntActID = encEntActID;
            obj.EntityCode = entityCode;
            obj.Lst = lst;
            return FileUploadUtility.UpdateArdsPlaceholders(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("MergeUploadFiles")]
        public string MergeUploadFiles(UploadMergeFile updObj)
        {
            string retFlag = string.Empty;
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            updObj.EntActID = CommonStaticMethods.Decrypt<int>(updObj.EncEntActID);
            updObj.PlantCode = tr.PlantCode;
            updObj.DeptCode = tr.DeptCode;
            updObj.LoginID = tr.LoginID;
            updObj.Role = tr.RoleName;
            updObj.AppCode = tr.ApplicationType;
            updObj.TypeCode = "DMSID";
            if (updObj.List != null && updObj.List.Count > 0)
            {
                string retMsg = CommonStaticMethods.PostApiConnectionData<string>("MergeUploadFiles", updObj, "DMS_URL");
                if (retMsg == "Success")
                    retFlag = "OK";
            }
            else
                retFlag = "OK";
            return retFlag;
        }

        [HttpPost]
        [Route("InvalidUploadFile")]
        public string InvalidUploadFile(BaseDocumentBO obj)
        {
            string retCode = string.Empty;

            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();

            //var baseBO = new BaseDocumentBO();
            obj.AppCode = trn.ApplicationType;
            obj.DeptCode = trn.DeptCode;
            obj.EntityCode = obj.EntityCode;
            obj.LoginID = trn.LoginID;
            obj.PlantCode = trn.PlantCode;
            obj.RefNumber = obj.RefNumber;
            obj.Remarks = obj.Remarks + " - " + trn.LoginID + " @ " + DateTime.Now;
            obj.EntActID = CommonStaticMethods.Decrypt<int>(obj.EntityActID);
            retCode = CommonStaticMethods.PostApiConnectionData<string>("InvalidUploadFile", obj, "DMS_URL");

            return retCode;
        }

    }
}
