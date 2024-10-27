using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.OOS;
using MedicalLIMSApi.Core.Interface.OOS;
using MedicalLIMSApi.Web.App_Start;
using System.Collections.Generic;
using System.Web.Http;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Web.Utilities;

namespace MedicalLIMSApi.Web.Controllers.OOS
{
    [LIMSAuthorization]
    public class OosController : ApiController
    {

        IOos db;
        public OosController(IOos _db)
        {
            this.db = _db;
        }

        [HttpPost]
        [Route("OOSSearchOOSTestDetails")]
        public SearchResults<GetSearchOOSTestDetails> OOSSearchOOSTestDetails(SearchOOSTestDetails obj)
        {
            return db.OOSSearchOOSTestDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetTestInfo")]
        public TestInfo GetTestInfo(OOSGetHypoTestingInfo obj)
        {
            return db.GetTestInfo(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("UpdateOOSSummary")]
        public RecordActionDetails UpdateOOSSummary(UpdateSummary obj)
        {
            return db.UpdateOOSSummary(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("OOSGetPhase1CheckList")]
        public GetOOSPhase1CheckList GetOOSPhase1CheckList(string encOosDetailID)
        {
            return db.GetOOSPhase1CheckList(CommonStaticMethods.Decrypt<int>(encOosDetailID));
        }

        [HttpPost]
        [Route("OOSProcessItem")]
        public string OOSProcessItem(OOSProcessItem obj)
        {
            return db.OOSProcessItem(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("OOSGetHypoTestingInfo")]
        public GetOOSHypoTestingResult GetOOSHypoTestingResult(OOSGetHypoTestingInfo obj)
        {
            return db.GetOOSHypoTestingResult(obj);
        }

        [HttpPost]
        [Route("OOSManageHypoTestingPhases")]
        public OOSManageHypoResults OOSManageHypoTestingPhases(OOSManageHypoTestingPhases obj)
        {
            return db.OOSManageHypoTestingPhases(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("OOSTestingOfNewPortionOfSameSampleResult")]
        public TestingSameSample OOSTestingOfNewPortionOfSameSampleResult(OOSGetHypoTestingInfo obj)
        {
            return db.OOSTestingOfNewPortionOfSameSampleResult(obj);
        }


        [HttpGet]
        [Route("OOSGetSingleAndCatBDetails")]
        public GetOOSSingleAndCatBDetails GetOOSSingleAndCatBDetails(string encOOSTestDetailID)
        {
            return db.GetOOSSingleAndCatBDetails(CommonStaticMethods.Decrypt<int>(encOOSTestDetailID));
        }

        [HttpGet]
        [Route("GetDeptReviewDetails")]
        public GetDeptReviewData GetDeptReviewDetails (string encOOSTestDetID)
        {
            return db.GetDeptReviewDetails(CommonStaticMethods.Decrypt<int>(encOOSTestDetID));
        }

        [HttpPost]
        [Route("ManageOOSDeptReview")]
        public RecordActionDetails ManageOOSDeptReview(OOSDeptReview obj)
        {
            return db.ManageOOSDeptReview(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetDepartmentWiseReview")]
        public List<GetDepartmentWiseReviews> GetDepartmentWiseReview(string encOOSTestDetailID)
        {
            return db.GetDepartmentWiseReview(CommonStaticMethods.Decrypt<int>(encOOSTestDetailID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().DeptID);
        }

        [HttpPost]
        [Route("ManageDepartmentComments")]
        public RecordActionDetails ManageDepartmentComments(ManageDeptCommets obj)
        {
            return db.ManageDepartmentComments(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetManufactureChecklist")]
        public List<ManufactureChecklist> GetManufactureChecklist (string category)
        {
            return db.GetManufactureChecklist(category);
        }

        [HttpGet]
        [Route("GetManufactureCheckPoints")]
        public ManufactureCheckPoints GetManufactureCheckPoints(string encOosTestDetID, int phaseID)
        {
            return db.GetManufactureCheckPoints(CommonStaticMethods.Decrypt<int>(encOosTestDetID), phaseID);
        }

        [HttpGet]
        [Route("ManufactureInvestigationDetails")]
        public MfgInvestigationDetails ManufactureInvestigationDetails(string encOosTestDetID)
        {
            return db.ManufactureInvestigationDetails(CommonStaticMethods.Decrypt<int>(encOosTestDetID));
        }

        [HttpPost]
        [Route("ManageQASummaryInfo")]
        public RecordActionDetails ManageQASummaryInfo(ManageQASummaryInfo obj)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            retObj = db.ManageQASummaryInfo(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            if (retObj.Act.ReturnFlag == "SUCCESS" && retObj.RptList != null && retObj.RptList.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = retObj.Act.TransKey;
                dmsObj.EntityCode = "OOSPROC";
                dmsObj.ReferenceNumber = retObj.Act.ReferenceNumber;
                dmsObj.List = retObj.RptList;
                string retCode = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
                if (retCode != "OK" && retCode != "SUCCESS")
                    retObj.Act.ReturnFlag = retCode;
            }
            return retObj.Act;
        }

        [HttpGet]
        [Route("GetQASummaryInfo")]
        public GetQASummaryInfo GetQASummaryInfo(string encOOSTestID)
        {
            return db.GetQASummaryInfo(CommonStaticMethods.Decrypt<int>(encOOSTestID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID);
        }

        [HttpPost]
        [Route("ManageOOSSummaryInfo")]
        public RecordActionDetails ManageOOSSummaryInfo(ManageOOSSummaryInfo obj)
        {
            return db.ManageOOSSummaryInfo(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetOOSSummaryInfo")]
        public ManageOOSSummaryInfo GetOOSSummaryInfo(string encOOSTestID)
        {
            return db.GetOOSSummaryInfo(CommonStaticMethods.Decrypt<int>(encOOSTestID));
        }

        [HttpGet]
        [Route("GenerateDeviationReq")]
        public NewDeviationReg GenerateDeviationReq(string encOOSTestID)
        {
            return db.GenerateDeviationReq(CommonStaticMethods.Decrypt<int>(encOOSTestID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetNewSampleRequests")]
        public List<NewSampleRequest> GetNewSampleRequests(string encOOSTestID)
        {
            return db.GetNewSampleRequests(CommonStaticMethods.Decrypt<int>(encOOSTestID));
        }
    }
}
