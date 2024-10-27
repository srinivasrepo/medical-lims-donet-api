using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Reports;
using MedicalLIMSApi.Core.Interface.Report;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.Report
{
    public class ReportRepository : IReport
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public EntityReportsList GetRPTReportInfo(TransResults tran, string entityType)
        {
            var lst = new EntityReportsList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspRPTGetReportInitInfo");
            ctx.AddInParameter<string>(cmd, "@ApplicationType", tran.ApplicationType);
            ctx.AddInParameter<string>(cmd, "@EntityType", entityType);

            using (var reader = cmd.ExecuteReader())
            {
                
                var entRpts = ((IObjectContextAdapter)context).ObjectContext.Translate<EntityReports>(reader);

                foreach (var rpt in entRpts)
                    lst.Add(rpt);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SearchResults<ReportSearchResult> SearchRptResult(ReportSearchBO search)
        {
            var lst = new SearchResults<ReportSearchResult>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspRPTSearchReportList");
            ctx.AddInParameter<int>(cmd, "@PageIndex", search.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", search.PageSize);
            ctx.AddInParameter<string>(cmd, "@ReportCode", search.ReportCode);
            ctx.AddInParameter<short>(cmd, "@PlantID", search.PlantID);


            if (search.DateFrom != default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@FromDate", search.DateFrom);
            if (search.DateTo != default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ToDate", search.DateTo);

            if (!string.IsNullOrEmpty(search.RefCode))
                ctx.AddInParameter<string>(cmd, "@RefCode", search.RefCode);
            if (search.MatID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatID", search.MatID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrCount = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var rr in rrCount)
                    lst.TotalNumberOfRows = rr;

                reader.NextResult();

                var inudution = ((IObjectContextAdapter)context).ObjectContext.Translate<ReportSearchResult>(reader);

                List<ReportSearchResult> SearchList = new List<ReportSearchResult>();
                foreach (var item in inudution)
                    SearchList.Add(item);

                lst.SearchList = SearchList;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

    }
}
