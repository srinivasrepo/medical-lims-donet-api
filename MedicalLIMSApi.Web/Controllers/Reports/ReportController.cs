using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Reports;
using MedicalLIMSApi.Core.Interface.Report;
using MedicalLIMSApi.Web.App_Start;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.Reports
{
    [LIMSAuthorization]
    public class ReportController : ApiController
    {
        IReport db;
        public ReportController(IReport db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("GetRPTReportInfo")]
        public EntityReportsList GetRPTReportInfo(string entityType)
        {
            return db.GetRPTReportInfo(Web.Utilities.Common.GetUserDetails(), entityType);
        }

        [HttpPost]
        [Route("SearchRptResult")]
        public SearchResults<ReportSearchResult> SearchRptResult(ReportSearchBO search)
        {
            search.PlantID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID;

            return db.SearchRptResult(search);
        }
    }
}
