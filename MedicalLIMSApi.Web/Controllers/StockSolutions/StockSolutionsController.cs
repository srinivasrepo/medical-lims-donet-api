using MedicalLIMSApi.Core.Interface.StockSolutions;
using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.StockSolutions;
using System.Web.Http;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Web.Utilities;

namespace MedicalLIMSApi.Web.Controllers.StockSolutions
{
    public class StockSolutionsController : ApiController
    {
        IStockSolutions db;
        
        public StockSolutionsController(IStockSolutions db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("STOCKManageStockSolutionsRequest")]
        public RecordActionDetails StockManageStockSolutionsRequest(StockManageStockSolutionsRequest obj)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            retObj = db.StockManageStockSolutionsRequest(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
            if (retObj.Act.ReturnFlag == "SUCCESS" && retObj.RptList != null && retObj.RptList.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = retObj.Act.TransKey;
                dmsObj.EntityCode = "STOCK_SOL";
                dmsObj.ReferenceNumber = retObj.Act.ReferenceNumber;
                dmsObj.List = retObj.RptList;
                string retCode = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
                if (retCode != "OK" && retCode != "SUCCESS")
                    retObj.Act.ReturnFlag = retCode;
            }
            return retObj.Act;
        }

        [HttpGet]
        [Route("GetSTOCKStockSolutionsDetailsByID")]
        public GetSTOCKStockSolutionsDetails GetSTOCKStockSolutionsDetailsByID (string encStockSolutionID )
        {
            return db.GetSTOCKStockSolutionsDetailsByID(CommonStaticMethods.Decrypt<int>(encStockSolutionID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID);
        }
        
        [HttpPost]
        [Route("STOCKManageStockSolutionsPreparation")]
        public RecordActionDetails StockManageStockSolutionsPreparation (STOCKManageStockSolutionsPreparation obj)
        {
            return db.StockManageStockSolutionsPreparation(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("STOCKManageStockSolutionsOutput")]
        public RecordActionDetails StockManageStockSolutionsOutput(STOCKManageStockSolutionsOutput obj)
        {
            return db.StockManageStockSolutionsOutput(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("STOCKSearchStockSolutions")]
        public SearchResults<GetSTOCKSearchStockSolutions> StockSearchStockSolutions(STOCKSearchStockSolutions obj)
        {
            return db.StockSearchStockSolutions(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID);
        }
    }
}
