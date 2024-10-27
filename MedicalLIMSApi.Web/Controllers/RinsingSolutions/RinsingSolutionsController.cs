using System.Web.Http;
using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Interface.RinsingSolutions;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.RinsingSolutions;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Web.Utilities;

namespace MedicalLIMSApi.Web.Controllers.RinsingSolutions
{
    public class RinsingSolutionsController : ApiController
    {
        IRinsingSolutions db;

        public RinsingSolutionsController(IRinsingSolutions db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("ManageRinsingSolutions")]
        public RecordActionDetails ManageRinsingSolutions(ManageRinsingSolutions obj)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();
            retObj = db.ManageRinsingSolutions(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            if (retObj.Act.ReturnFlag == "SUCCESS" && retObj.RptList != null && retObj.RptList.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = retObj.Act.TransKey;
                dmsObj.EntityCode = "RINSING_SOL";
                dmsObj.ReferenceNumber = retObj.Act.ReferenceNumber;
                dmsObj.List = retObj.RptList;
                string retCode = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
                if (retCode != "OK" && retCode != "SUCCESS")
                    retObj.Act.ReturnFlag = retCode;
            }
            return retObj.Act;
        }

        [HttpGet]
        [Route("GetRinsingSolutionsDetailsByID")]
        public GetRinsingSolutionsDetails GetRinsingSolutionsDetailsByID(string encSolutionID)
        {
            return db.GetRinsingSolutionsDetails(CommonStaticMethods.Decrypt<int>(encSolutionID),MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID);
        }

        [HttpPost]
        [Route("ManageRinsingSolutionsOutputDetails")]
        public RecordActionDetails ManageRinsingSolutionsOutputDetails (ManageRinsingSolutionsOutputDetails obj)
        {
            return db.ManageRinsingSolutionsOutputDetails(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SearchRinsingSolutionsData")]
        public SearchResults<GetRinsingSolutions> SearchRinsingSolutionsData(SearchRinsingSolutionsData obj)
        {
            return db.SearchRinsingSolutionsData(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }
    }
}
