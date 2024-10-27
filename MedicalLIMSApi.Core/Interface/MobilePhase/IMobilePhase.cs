using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;

namespace MedicalLIMSApi.Core.Interface.MobilePhase
{
    public interface IMobilePhase
    {
        GetActionAndRptData ManageMobilePhase(MobilePhaseBO obj, TransResults tr);

        MobilePhaseData GetMobilePhaseData(int mobilePhaseID, TransResults trn);

        UOMList GetConvertableUOMByMatID(int? materialID, int sioID);

        Preparations ManageMobilePhaseSolventsPreparation(SolventPreparation obj, TransResults tr);

        TransResultApprovals ManageMobilePhasePrepComments(MobilePhasePrepComments obj, TransResults trn);

        SingleUsrBasicList GetValidityPeriods(string entityCode);

        ManageMobilePreparationOutput ManagePhaseOutput(MobilePhaseOutput obj, TransResults trn);

        SearchMobilePhaseData GetSearchMobilePhaseData(SearchMPBO obj);

        viewMobilePhase ViewMobilePhaseData(int phaseID);

        GetPreparationDetailsList GetPreparationDetails(int phaseID);

        ParameterTypeList GetCalibrationParameters();

        GetMasterData ManagePreparationMasterData(MasterData obj, TransResults tr);

        string DiscardPreparationBatch(DiscardPreparationBatch obj, TransResults tr);

        GetProductStageDetails GetMaterialDetailsBySpecID(int specID);
    }
}
