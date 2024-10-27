using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.VolumetricSolution;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Interface.VolumetricSolution
{
    public interface IVolumetricSolution
    {
        VolumetricSolIndexData GetVolumetricSolIndex(VolumetricSolIndexFilter obj, TransResults trn);

        GetVolumetricSolIndex GetVolumetricSolIndexByID(short indexID);

        GetActionAndRptData ManageVolumetricSol(AddSolution obj, TransResults trn);

        GetVolumetricSol GetVolumetricSolutionByID(int solutionID, int userRoleID);

        GetStandardizationInfo GetVolumetricStandardByID(int StandardizationID, TransResults trn);

        RecordActionDetails ManageVolumetricSolStandard(ManageVolStdDetails obj, TransResults trn);

        SearchResults<SearchVolumetricSOl> SearchVolumetricSol(SearchVolumetricSolFilter obj, TransResults trn);

        RecordActionDetails ReStandardization(ReStandardization obj, TransResults trn);
        string GetRSDValue(string xml);
        TransResultApprovals InvalidateRequest(ReStandardization obj, TransResults transResults);

        ProcedureUpdate ManageStandProcedures(ProcedureUpdate obj, TransResults trn);

        List<GetSolutionFormulae> getSolutionFormulae(short indexID);

        string manageSolutionFormula(ManageSolutionFormula obj, TransResults tr);

        List<GetSolutionFormulasByIndexID> getSolutionFormulasByIndexID(int indexID);

        GetVOLViewSolIndexDetailsByIDResp GetVOLViewSolIndexDetailsByID(int IndexID);
    }
}
