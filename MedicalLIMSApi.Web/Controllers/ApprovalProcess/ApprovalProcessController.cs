using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.ApprovalProcess;
using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SampleAnalysis;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using MedicalLIMSApi.Core.Interface.ApprovalProcess;
using MedicalLIMSApi.Web.App_Start;
using MedicalLIMSApi.Web.Utilities;
using System;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.ApprovalProcess
{
    [LIMSAuthorization]
    public class ApprovalProcessController : ApiController
    {
        IApprovalProcess db;

        public ApprovalProcessController(IApprovalProcess db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("GetUserEntityApproveDetails")]
        public ApprovalActions GetUserEntityApproveDetails(int detailID, string entActualID)
        {
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();

            ApprovalDetails obj = new ApprovalDetails();
            obj.DetailID = detailID;
            obj.UserID = trn.UserID;
            obj.RoleID = trn.RoleID;
            obj.EntActualID = CommonStaticMethods.Decrypt<int>(entActualID);

            return db.GetUserEntityApplicableActions(obj);
        }

        [HttpPost]
        [Route("ConfirmAction")]
        public string ConfirmAction([FromBody]ApprovalConfirmDetails app)
        {
            string retVal = string.Empty;
            SupplierLogin obj = new SupplierLogin();
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            retVal = db.ConfirmAction(app, trn);
            if (app.EntityCode == "ANALYST_QUALIFICATION" && retVal == "FINAL")
                retVal = AnalystQualificationFinalConfirm(app.EntActID);
            else if (app.AppLevel == 1 && app.EntityCode == "INVALIDATIONS" && retVal == "OK")
                GenerateDocument(app.EncryptedKey);
            else if (app.EntityCode == "INVALIDATIONS" && retVal == "FINAL")
                retVal = GenerateCumulativeInvalidationReport(app.EntActID, app.EntityCode);
            else if (retVal == "FINAL" && (app.EntityCode == "QCSAMPASYS" || app.EntityCode == "CALIB_VALIDATION" || app.EntityCode == "SPEC_VALID" || app.EntityCode == "ENGGMNT" || app.EntityCode == "OOSPROC"))
            {
                retVal = GenerateMergedARDSReport(app.EntActID, app.EntityCode);
                retVal = "OK";
            }
            return retVal;
        }

        string AnalystQualificationFinalConfirm(int qualificationID)
        {
            string retCode = string.Empty;
            ArdsReportInvalidBO obj = new ArdsReportInvalidBO();
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();

            obj = db.GetArdsInvalidDocument(qualificationID);
            obj.PlantCode = tr.PlantCode;
            obj.DeptCode = tr.DeptCode;
            obj.AppCode = tr.ApplicationType;
            obj.WaterMarkerText = obj.ReturnFlag;
            if (obj.ReturnFlag != "OK" && obj.List != null && obj.List.Count > 0)
                retCode = CommonStaticMethods.PostApiConnectionData<string>("UpdateArdsWaterMarker", obj, "DMS_URL");
            else
                retCode = "OK";
            return retCode;
        }

        void GenerateDocument(string encInvalidationID)
        {
            GetReportData rpt = new GetReportData();

            ARDSInvalidation invalObj = new ARDSInvalidation();
            invalObj = db.GetInvalidationInfoForArds(CommonStaticMethods.Decrypt<int>(encInvalidationID));

            if (invalObj.ReturnFlag == "OK")
            {
                ReportDetails rptObj = new ReportDetails();
                ArdsBO obj = new ArdsBO();
                obj.InvalidationID = CommonStaticMethods.Decrypt<int>(encInvalidationID);
                obj.ArdsExecID = Convert.ToInt32(invalObj.EntityActID);
                obj.EntityCode = invalObj.EntityCode;

                rptObj = FileUploadUtility.GetCalibrationReportDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
                rptObj.LoginID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().LoginID;

                CommonStaticMethods.GetPlaceHolder(obj.EntityCode, rptObj.PlaceHolders);

                rpt.StatusCode = UpdateArdsPlaceholders(Convert.ToString(obj.ArdsExecID), obj.EntityCode, rptObj.PlaceHolders.PlaceholderList);

                if (rpt.StatusCode != "SUCCESS")
                    return;
                try
                {
                    rptObj.PlaceholderList = rptObj.PlaceHolders.PlaceholderList;
                    rptObj.InvalidationCode = invalObj.InvalidationNumber.Replace('/', '_');
                    rptObj.WaterMarker = "INVALID";
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
            }
        }

        string UpdateArdsPlaceholders(string encEntActID, string entityCode, PlaceholderList lst)
        {
            UpdateARDSPlaceholder obj = new UpdateARDSPlaceholder();
            obj.EncEntActID = encEntActID;
            obj.EntityCode = entityCode;
            obj.Lst = lst;
            return FileUploadUtility.UpdateArdsPlaceholders(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        string GenerateCumulativeInvalidationReport(int invalidationID, string entityCode)
        {
            string retFlag = string.Empty;
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            CumulativeInvalidationBO obj = new CumulativeInvalidationBO();
            obj = db.GetCumulativeInvalidationInfo(invalidationID);

            ReportUploadDMS rptObj = new ReportUploadDMS();
            rptObj.AppCode = tr.ApplicationType;
            rptObj.DeptCode = tr.DeptCode;
            rptObj.PlantCode = tr.PlantCode;
            rptObj.LoginID = tr.LoginID;
            rptObj.RoleName = tr.RoleName;
            rptObj.EntityCode = entityCode;
            rptObj.EntActID = obj.InvalidationID;
            rptObj.ReferenceNumber = obj.InvalidationCode;
            rptObj.Type = obj.ImpactType;
            rptObj.TypeCode = obj.ImpactTypeCode;
            retFlag = CommonStaticMethods.PostApiConnectionData<string>("UploadCumulativeReportsInfo", rptObj, "DMS_URL");

            return retFlag;
        }

        string GenerateMergedARDSReport(int entActID, string entityCode)
        {
            string retFlag = string.Empty;
            TransResults tr = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            ARDSMergedReport obj = db.GetArdsMergedReportInfo(entActID, entityCode);
            UploadMergeFile updObj = new UploadMergeFile();
            updObj.EntActID = entActID;
            updObj.EntityCode = entityCode;
            updObj.PlantCode = tr.PlantCode;
            updObj.SectionCode = "DMS_REPORT";
            updObj.DeptCode = tr.DeptCode;
            updObj.LoginID = tr.LoginID;
            updObj.Role = tr.RoleName;
            updObj.AppCode = tr.ApplicationType;
            updObj.List = obj.List;
            updObj.InsertSection = "REPORT";
            updObj.ReferenceNumber = obj.ReferenceNumber;
            updObj.TypeCode = "DMSID";
            updObj.FileName = string.Format("{0}_ARDS_MergedReport", obj.ReferenceNumber.Replace('/','_'));

            if (obj.List != null && obj.List.Count > 0)
            {
                retFlag = CommonStaticMethods.PostApiConnectionData<string>("MergeSDMSFiles", updObj, "DMS_URL");
                if (retFlag == "Success")
                    retFlag = "OK";
            }
            else
                retFlag = "NO_ARDS_RPT";
            return retFlag;
        }

        [HttpGet]
        [Route("GenerateCumulativeArdsReport")]
        public string GenerateCumulativeArdsReport(string encEntActID, string entityCode)
        {
            return GenerateMergedARDSReport(CommonStaticMethods.Decrypt<int>(encEntActID), entityCode);
        }
    }
}
