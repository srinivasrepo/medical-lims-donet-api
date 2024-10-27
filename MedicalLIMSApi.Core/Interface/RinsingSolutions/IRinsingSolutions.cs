using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.RinsingSolutions;

namespace MedicalLIMSApi.Core.Interface.RinsingSolutions
{
    public interface IRinsingSolutions
    {
        GetActionAndRptData ManageRinsingSolutions(ManageRinsingSolutions obj , TransResults tr);

        GetRinsingSolutionsDetails GetRinsingSolutionsDetails(int solutionID, int userRoleID);

        RecordActionDetails ManageRinsingSolutionsOutputDetails(ManageRinsingSolutionsOutputDetails obj,TransResults tr);

        SearchResults<GetRinsingSolutions> SearchRinsingSolutionsData(SearchRinsingSolutionsData obj, TransResults tr);
    }
}
