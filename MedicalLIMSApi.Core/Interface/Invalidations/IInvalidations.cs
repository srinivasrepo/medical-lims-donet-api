using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Invalidations;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Interface.Invalidations
{
    public interface IInvalidations
    {
        SearchResults<SearchInvalidations> SearchInvalidations(InvSearchBO obj);

        InvalidationInfo GetInvalidationData(int invalidationID, TransResults tr);

        GetActionAndRptData ManageInvalidationInfo(ManageInvalidationBO obj, TransResults tr);

        SearchInvalidationList GetInvalidationTypesInstrumentTypes();

        List<GetPreviousInvalidations> GetPreviousInvalidations(int invalidationID);

        RecordActionDetails ManageInvalidationManualInfo(ManageInvalidationManualInfo obj, TransResults trn);

    }
}
