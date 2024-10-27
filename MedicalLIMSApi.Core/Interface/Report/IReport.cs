using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Reports;

namespace MedicalLIMSApi.Core.Interface.Report
{
    public interface IReport
    {
        EntityReportsList GetRPTReportInfo(TransResults transResults, string entityType);

        SearchResults<ReportSearchResult> SearchRptResult(ReportSearchBO search);
    }
}
