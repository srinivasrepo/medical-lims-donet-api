using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.AuditTrail;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Interface.AuditTrail;
using MedicalLIMSApi.Web.App_Start;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.AuditTrail
{
    [LIMSAuthorization]
    public class AuditTrailController : ApiController
    {
        IAuditTrail db;

        public AuditTrailController(IAuditTrail db)
        {
            this.db = db;
        }


        [HttpPost]
        [Route("GetAuditTrailLogDetails")]
        public SearchResults<AuditData> GetAuditTrailLogDetails(SearchAuditData obj)
        {
            obj.PlantID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID;
            return db.GetAuditTrailLogDetails(obj);
        }

        [HttpGet]
        [Route("GetAuditTableByAuditID")]
        public AuditDataTitleList GetAuditTableByAuditID(string encAuditID)
        {
            long auditID = default(long);

            auditID = CommonStaticMethods.Decrypt<long>(encAuditID);
            return db.GetAuditTableByAuditID(auditID);
        }

        [HttpGet]
        [Route("GetAuditColumnsByTableID")]
        public AuditDataByIDList GetAuditColumnsByTableID(string encTableID)
        {
            long auditTableID = default(long);
            auditTableID = CommonStaticMethods.Decrypt<long>(encTableID);
            return db.GetAuditColumnsByTableID(auditTableID);
        }

        [HttpGet]
        [Route("GetAllTables")]
        public AudObjectList GetDBObjects(int? tabID)
        {
            return db.GetDBObjects(tabID);
        }

        [HttpPost]
        [Route("ManageDBObjects")]
        public string ManageDBObjects(ManageAudObj obj)
        {
            return db.ManageDBObjects(obj);
        }

    }
}
