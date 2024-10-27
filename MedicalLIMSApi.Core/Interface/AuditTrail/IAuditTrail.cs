using MedicalLIMSApi.Core.Entities.AuditTrail;
using MedicalLIMSApi.Core.Entities.Common;

namespace MedicalLIMSApi.Core.Interface.AuditTrail
{
    public interface IAuditTrail
    {
        SearchResults<AuditData> GetAuditTrailLogDetails(SearchAuditData obj);

        AuditDataTitleList GetAuditTableByAuditID(long auditID);

        AuditDataByIDList GetAuditColumnsByTableID(long auditTableID);

        AudObjectList GetDBObjects(int? tabID);

        string ManageDBObjects(ManageAudObj obj);
    }
}
