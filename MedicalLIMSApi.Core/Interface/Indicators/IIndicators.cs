using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Indicators;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.VolumetricSolution;

namespace MedicalLIMSApi.Core.Interface.Indicators
{
    public interface IIndicators
    {
        SearchResults<SearchIndicatorData> SearchIndicators(SearchIndicatorsBO obj);

        GetIndicatorsInfo GetIndicatorsInfo(int indicatorID, int userRoleID);

        GetActionAndRptData ManageIndicatorsInfo(ManageIndicatorsInfo obj, TransResults trn);

        ViewIndicatorInfo ViewIndicatorInfo(int indicatorID);

        SolventsList GetPreparationDetails(SolventPreparation obj);

        GetINDMasterData ManageIndicatorMasterData(ManageMasterData obj, TransResults tr);

        VolumetricSolIndexData ManageTestSolutionIndex(VolumetricSolIndexFilter obj, TransResults tr);

        GetVolumetricSolIndex GetTestSolutionIndexByID(short indexID);
    }
}
