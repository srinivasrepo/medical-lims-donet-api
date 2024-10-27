using MedicalLIMSApi.Core.Interface.RinsingSolutions;
using MedicalLIMSApi.Infrastructure.Context;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.RinsingSolutions;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.RinsingSolutions
{
    public class RinsingSolutionsRepository : IRinsingSolutions
    {
        TrainingContext context = new TrainingContext();
        DBHelper ctx = new DBHelper();

        public GetActionAndRptData ManageRinsingSolutions(ManageRinsingSolutions obj, TransResults tr)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspRSManageRinsingSolutions");
            if (obj.SolutionID > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionID", obj.SolutionID);
            ctx.AddInParameter<int>(cmd, "@UsageTypeID", obj.UsageTypeID);
            ctx.AddInParameter<string>(cmd, "@SolutionName", obj.SolutionName);
            ctx.AddInParameter<string>(cmd, "@StpNumber", obj.StpNumber);
            if (!string.IsNullOrEmpty(obj.BriefDescription))
                ctx.AddInParameter<string>(cmd, "@BriefDescription", obj.BriefDescription);
            if (!string.IsNullOrEmpty(obj.PreparationRemarks))
                ctx.AddInParameter<string>(cmd, "@PreparationRemarks", obj.PreparationRemarks);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            if (!string.IsNullOrEmpty(obj.UploadedIDXMl))
                ctx.AddInParameter<string>(cmd, "@UploadedIDXMl", obj.UploadedIDXMl);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<string>(cmd, "Type", obj.Type);

            using (var reader = cmd.ExecuteReader())
            {
                var read = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in read)
                    retObj.Act = rr;

                reader.NextResult();

                retObj.RptList = new UploadReportList();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadReports>(reader);
                foreach (var rr in rrLst)
                    retObj.RptList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return retObj;
        }

        public GetRinsingSolutionsDetails GetRinsingSolutionsDetails(int solutionID, int userRoleID)
        {
            GetRinsingSolutionsDetails lst = new GetRinsingSolutionsDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspRSGetRinsingSolutionsDetailsByID");
            ctx.AddInParameter<int>(cmd, "@SolutionID", solutionID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", userRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var read = ((IObjectContextAdapter)context).ObjectContext.Translate<GetRinsingSolutionsDetails>(reader);
                foreach (var rr in read)
                    lst = rr;

                reader.NextResult();
                lst.SolventsList = new SolventsList();
                var solLst = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);
                foreach (var rr in solLst)
                    lst.SolventsList.Add(rr);

                reader.NextResult();
                lst.RecordActions = new RecordActionDetails();
                var reAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in reAct)
                    lst.RecordActions = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public RecordActionDetails ManageRinsingSolutionsOutputDetails(ManageRinsingSolutionsOutputDetails obj, TransResults tr)
        {
            RecordActionDetails reAct = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspRSManageRinsingSolutionsOutputDetails");
            ctx.AddInParameter<int>(cmd, "@SolutionID", obj.SolutionID);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<decimal>(cmd, "@FinalVolume", obj.FinalVolume);
            ctx.AddInParameter<int>(cmd, "@ValidityPeriodID", obj.ValidityPeriodID);
            ctx.AddInParameter<DateTime>(cmd, "@UseBeforeDate", obj.UseBeforeDate);
            if (!string.IsNullOrEmpty(obj.OutputRemarks))
                ctx.AddInParameter<string>(cmd, "@OutputRemarks", obj.OutputRemarks);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var read = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in read)
                    reAct = rr;

                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return reAct;
        }

        public SearchResults<GetRinsingSolutions> SearchRinsingSolutionsData(SearchRinsingSolutionsData obj, TransResults tr)
        {
            var list = new SearchResults<GetRinsingSolutions>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspRSSearchRinsingSolutionsData");
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            if (obj.UsageType > default(int))
                ctx.AddInParameter<int>(cmd, "@UsageType", obj.UsageType);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.BatchNumberID > default(int))
                ctx.AddInParameter<int>(cmd, "@BatchNumberID", obj.BatchNumberID);
            if (obj.ValidityFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ValidityFrom", obj.ValidityFrom);
            if (obj.ValidityTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ValidityTo", obj.ValidityTo);
            if (obj.SolutionIDFrom > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionIDFrom", obj.SolutionIDFrom);
            if (obj.SolutionIDTo > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionIDTo", obj.SolutionIDTo);
            if (!string.IsNullOrEmpty(obj.NameOfTheSolution))
                ctx.AddInParameter<string>(cmd, "@NameOfTheSolution", obj.NameOfTheSolution);
            if (!string.IsNullOrEmpty(obj.StpNumber))
                ctx.AddInParameter<string>(cmd, "@StpNumber", obj.StpNumber);
            if (obj.InitiatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InitiatedOn",obj.InitiatedOn);
            if (obj.InitiatedBy > default(int))
                ctx.AddInParameter<int>(cmd, "@InitiatedBy",obj.InitiatedBy);
            if (obj.SolutionID > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionID", obj.SolutionID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var totalRecords = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var tnr in totalRecords)
                    list.TotalNumberOfRows = tnr;

                reader.NextResult();

                List<GetRinsingSolutions> gsts = new List<GetRinsingSolutions>();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetRinsingSolutions>(reader);
                foreach (var rr in rrRes)
                    gsts.Add(rr);

                list.SearchList = gsts;
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return list;
        }
    }
}
