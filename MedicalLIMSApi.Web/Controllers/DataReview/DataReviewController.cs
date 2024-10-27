using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.DataReview;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Interface.DataReview;
using MedicalLIMSApi.Web.App_Start;
using MedicalLIMSApi.Web.Utilities;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.DataReview
{
    [LIMSAuthorization]
    public class DataReviewController : ApiController
    {

        IDataReview db;

        public DataReviewController(IDataReview db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("GetTestForReview")]
        public DataReviewTestList GetTestForReview(GetReviewTests obj)
        {
            return db.GetTestForReview(obj);
        }

        [HttpPost]
        [Route("ManageDataReviewData")]
        public RecordActionDetails ManageDataReviewData(DataReviewData obj)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            retObj = db.ManageDataReviewData(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            if (retObj.Act.ReturnFlag == "SUCCESS" && retObj.RptList != null && retObj.RptList.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = retObj.Act.TransKey;
                dmsObj.EntityCode = obj.EntityCode;
                dmsObj.ReferenceNumber = retObj.Act.ReferenceNumber;
                dmsObj.List = retObj.RptList;
                string retCode = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
                if (retCode != "OK" && retCode != "SUCCESS")
                    retObj.Act.ReturnFlag = retCode;
            }

            return retObj.Act;
        }

        [HttpGet]
        [Route("GetDataReviewData")]
        public GetDataReviewDetails GetDataReviewData(string encReviewID)
        {
            return db.GetDataReviewData(CommonStaticMethods.Decrypt<int>(encReviewID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SearchDataReviewDetails")]
        public SearchResults<GetDataReviewDetailsBySearch> GetDataReviewDetailsBySearchId(SearchDataReview obj)
        {
            return db.GetDataReviewDetailsBySearchId(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID);
        }
    }
}
