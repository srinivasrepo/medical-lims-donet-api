using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.QCInventory;

namespace MedicalLIMSApi.Core.Interface.QCInventory
{
    public interface IQCInventory
    {
        SearchResults<GetQCInventoryItems> getQCInventoryList(SearchQCInventory obj, TransResults tr);

        QCInventoryBatchDetails GetQCInventoryDetails(int invSourceID, int UserRoleID);

        PackDetails GetQCInvPackDetails(int invID);

        string ManageQCInvPackDetails(ManagePacks obj, TransResults tr);

        string ManageQCBatchDetails(ManageBatches obj, TransResults tr);

        string ManageQCInventory(ManageQCInventory manageObj, TransResults transObj);

        ViewInvtDetails ViewInvtDetailsByInvID(GetPackInvDetails obj);

        GetActionAndRptData openPack(OpenPack obj, TransResults tr);

        GetMiscConsumptionDetails GetMiscConsumptionDetails(int packInvID);

        string ManageMiscConsumptionData(MiscConsumption obj, TransResults tr);

        PackInvDetailsList GetPackInventoryReservationsDetails(int PackInvID);

        GetQCInventorySourcesList GetQCInventorySources();

        GetBatchSplitDetailsBO GenerateNewBatchSplit(QCInvBatchSplitBO obj, TransResults trn);

        GetBatchSplitDetailsBO DeleteNewBatchSplit(int invID, TransResults trn);

        string SendBatchForSample(int invID, TransResults trn);
    }
}
