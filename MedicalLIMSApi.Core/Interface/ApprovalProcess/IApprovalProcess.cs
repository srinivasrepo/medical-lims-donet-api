using MedicalLIMSApi.Core.Entities.ApprovalProcess;
using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.UtilUploads;

namespace MedicalLIMSApi.Core.Interface.ApprovalProcess
{
    public interface IApprovalProcess
    {
        ApprovalActions GetUserEntityApplicableActions(ApprovalDetails appProcess);

        string ConfirmAction(ApprovalConfirmDetails app, TransResults trn);

        ArdsReportInvalidBO GetArdsInvalidDocument(int qualificationID);


        string UpdateReportID(ArdsBO obj, TransResults tr);


        ARDSInvalidation GetInvalidationInfoForArds(int invalidationID);

        CumulativeInvalidationBO GetCumulativeInvalidationInfo(int invalidationID);

        ARDSMergedReport GetArdsMergedReportInfo(int entActID, string entityCode);
    }
}
