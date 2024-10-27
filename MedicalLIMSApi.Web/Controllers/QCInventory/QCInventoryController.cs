using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.QCInventory;
using MedicalLIMSApi.Core.Interface.QCInventory;
using MedicalLIMSApi.Web.Utilities;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.QCInventory
{
    public class QCInventoryController : ApiController
    {
        IQCInventory db;

        public QCInventoryController(IQCInventory db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("SearchQCInventory")]
        public SearchResults<GetQCInventoryItems> getQCInventoryList(SearchQCInventory obj)
        {
            return db.getQCInventoryList(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetQCInventoryDetails")]
        public QCInventoryBatchDetails GetQCInventoryDetails(string encInvSourceID)
        {
            return db.GetQCInventoryDetails(CommonStaticMethods.Decrypt<int>(encInvSourceID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID);
        }

        [HttpGet]
        [Route("GetQCInvPackDetails")]
        public PackDetails GetQCInvPackDetails(int invID)
        {
            return db.GetQCInvPackDetails(invID);
        }

        [HttpPost]
        [Route("ManageQCInvPackDetails")]
        public string ManageQCInvPackDetails(ManagePacks obj)
        {
            return db.ManageQCInvPackDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageQCBatchDetails")]
        public string ManageQCBatchDetails(ManageBatches obj)
        {
            return db.ManageQCBatchDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageQCInventory")]
        public string ManageQCInventory(ManageQCInventory manageObj)
        {
            return db.ManageQCInventory(manageObj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ViewInvtDetailsByInvID")]
        public ViewInvtDetails ViewInvtDetailsByInvID(GetPackInvDetails obj)
        {
            return db.ViewInvtDetailsByInvID(obj);
        }

        [HttpPost]
        [Route("ManageOpenPack")]
        public string openPack(OpenPack obj)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            retObj = db.openPack(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            if (retObj.ReturnFlag == "OK" && retObj.RptList != null && retObj.RptList.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = obj.PackInvID;
                dmsObj.EntityCode = "QC_INV_PACK";
                dmsObj.ReferenceNumber = retObj.refCode;
                dmsObj.List = retObj.RptList;
                string retCode = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
                if (retCode != "OK" && retCode != "SUCCESS")
                    retObj.ReturnFlag = retCode;
            }
            return retObj.ReturnFlag;
        }

        [HttpGet]
        [Route("GetMiscConsumptionDetails")]
        public GetMiscConsumptionDetails GetMiscConsumptionDetails(int packInvID)
        {
            return db.GetMiscConsumptionDetails(packInvID);
        }

        [HttpPost]
        [Route("ManageMiscConsumptionData")]
        public string ManageMiscConsumptionData(MiscConsumption obj)
        {
            return db.ManageMiscConsumptionData(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetPackInventoryReservationsDetails")]
        public PackInvDetailsList GetPackInventoryReservationsDetails(string EncPackInvID)
        {
            return db.GetPackInventoryReservationsDetails(CommonStaticMethods.Decrypt<int>(EncPackInvID));
        }

        [HttpGet]
        [Route("GetQCInventorySources")]
        public GetQCInventorySourcesList GetQCInventorySources()
        {
            return db.GetQCInventorySources();
        }

        [HttpPost]
        [Route("GenerateNewBatchSplit")]
        public GetBatchSplitDetailsBO GenerateNewBatchSplit(QCInvBatchSplitBO obj)
        {
            return db.GenerateNewBatchSplit(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("DeleteNewBatchSplit")]
        public GetBatchSplitDetailsBO DeleteNewBatchSplit(string encInvID)
        {
            return db.DeleteNewBatchSplit(CommonStaticMethods.Decrypt<int>(encInvID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("SendBatchForSample")]
        public string SendBatchForSample (int invID)
        {
            return db.SendBatchForSample(invID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }
    }
}
