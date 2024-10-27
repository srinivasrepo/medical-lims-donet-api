using MedicalLIMSApi.Core.Interface.StockSolutions;
using MedicalLIMSApi.Infrastructure.Context;
using MedicalLIMSApi.Core.Entities.StockSolutions;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using MedicalLIMSApi.Core.Entities.MobilePhase;

namespace MedicalLIMSApi.Infrastructure.Repository.StockSolutions
{
    public class StockSolutionsRepository : IStockSolutions
    {
        TrainingContext context = new TrainingContext();
        DBHelper ctx = new DBHelper();

        public GetActionAndRptData StockManageStockSolutionsRequest(StockManageStockSolutionsRequest obj, TransResults tr)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspSTOCKManageStockSolutionsRequest");
            if (obj.StockSolutionID > default(int))
                ctx.AddInParameter(cmd, "@StockSolutionID", obj.StockSolutionID);
            ctx.AddInParameter(cmd, "@SolutionIndexID", obj.SolutionIndexID);
            if(obj.TestID > default(int))
                ctx.AddInParameter(cmd, "@TestID", obj.TestID);
            ctx.AddInParameter(cmd, "@InstID", obj.InstID);
            if (!string.IsNullOrEmpty(obj.DryingTemp))
                ctx.AddInParameter(cmd, "@DryingTemp", obj.DryingTemp);
            if (!string.IsNullOrEmpty(obj.DryingDuration))
                ctx.AddInParameter(cmd, "@DryingDuration", obj.DryingDuration);
            if (!string.IsNullOrEmpty(obj.CoolingDuration))
                ctx.AddInParameter(cmd, "@CoolingDuration", obj.CoolingDuration);
            if (!string.IsNullOrEmpty(obj.XMLuploadedID))
                ctx.AddInParameter<string>(cmd, "@UploadedIDXMl", obj.XMLuploadedID);
            ctx.AddInParameter(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter(cmd, "@DeptID", tr.DeptID);
            if (!string.IsNullOrEmpty(tr.InitTime))
                ctx.AddInParameter(cmd, "@InitTime", tr.InitTime);
            ctx.AddInParameter<string>(cmd, "@Manual", obj.Manual);
            if (obj.CalibrationReferenceID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibrationReferenceID", obj.CalibrationReferenceID);

            using (var reader = cmd.ExecuteReader())
            {
                var read = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rs in read)
                    retObj.Act = rs;

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

        public GetSTOCKStockSolutionsDetails GetSTOCKStockSolutionsDetailsByID(int stockSolutionID, int userRoleID)
        {
            GetSTOCKStockSolutionsDetails lst = new GetSTOCKStockSolutionsDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspGetSTOCKStockSolutionsDetailsByID");
            ctx.AddInParameter(cmd, "@StockSolutionID", stockSolutionID);
            ctx.AddInParameter(cmd, "@UserRoleID", userRoleID);
            using (var reader = cmd.ExecuteReader())
            {
                lst.STOCKStockSolutionsDetails = new STOCKStockSolutionsDetails();
                var stDet = ((IObjectContextAdapter)context).ObjectContext.Translate<STOCKStockSolutionsDetails>(reader);
                foreach (var rr in stDet)
                    lst.STOCKStockSolutionsDetails = rr;

                reader.NextResult();

                var rrResultSol = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);

                lst.SolventsList = new SolventsList();

                foreach (var rr in rrResultSol)
                    lst.SolventsList.Add(rr);

                reader.NextResult();

                lst.RecordActions = new RecordActionDetails();
                var act = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in act)
                    lst.RecordActions = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public RecordActionDetails StockManageStockSolutionsPreparation(STOCKManageStockSolutionsPreparation obj, TransResults tr)
        {
            RecordActionDetails lst = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspSTOCKManageStockSolutionsPreparation");

            ctx.AddInParameter<int>(cmd, "@StockSolutionID", obj.StockSolutionID);
            ctx.AddInParameter<string>(cmd, "@Description", obj.Description);
            if (!string.IsNullOrEmpty(obj.OtherInfo))
                ctx.AddInParameter<string>(cmd, "@OtherInfo", obj.OtherInfo);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);

            if (obj.SolPH > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@SolPH", obj.SolPH);

            if (obj.Weight > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@Weight", obj.Weight);

            using (var reader = cmd.ExecuteReader())
            {
                var reAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var act in reAct)
                    lst = act;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;

        }

        public RecordActionDetails StockManageStockSolutionsOutput(STOCKManageStockSolutionsOutput obj, TransResults tr)
        {
            RecordActionDetails lst = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspSTOCKManageStockSolutionsOutput");

            ctx.AddInParameter<int>(cmd, "@StockSolutionID", obj.StockSolutionID);
            ctx.AddInParameter<decimal>(cmd, "@FinalVolume", obj.FinalVolume);
            ctx.AddInParameter<int>(cmd, "@Validity", obj.Validity);
            ctx.AddInParameter<DateTime>(cmd, "@UseBefore", obj.UseBefore);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);





            using (var reader = cmd.ExecuteReader())
            {
                var reAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var act in reAct)
                    lst = act;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SearchResults<GetSTOCKSearchStockSolutions> StockSearchStockSolutions(STOCKSearchStockSolutions obj, short plantID)
        {
            var list = new SearchResults<GetSTOCKSearchStockSolutions>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspSTOCKSearchStockSolutions");
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            if (obj.StockSolutionID > default(int))
                ctx.AddInParameter<int>(cmd, "@StockSolutionID", obj.StockSolutionID);
            if (obj.SolutionID > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionID", obj.SolutionID);
            if (obj.InstrumentID > default(int))
                ctx.AddInParameter<int>(cmd, "@InstrumentID", obj.InstrumentID);
            if (obj.InstrumentType > default(int))
                ctx.AddInParameter<int>(cmd, "@InstrumentType", obj.InstrumentType);
            if (obj.ParameterID > default(int))
                ctx.AddInParameter<int>(cmd, "@ParameterID", obj.ParameterID);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.BatchNumberID > default(int))
                ctx.AddInParameter<int>(cmd, "@BatchNumberID", obj.BatchNumberID);
            if (obj.ValidityFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ValidityFrom", obj.ValidityFrom);
            if (obj.ValidityTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ValidityTo", obj.ValidityTo);
            if (obj.StockSolutionIDFrom > default(int))
                ctx.AddInParameter<int>(cmd, "@StockSolutionIDFrom", obj.StockSolutionIDFrom);
            if (obj.StockSolutionIDTo > default(int))
                ctx.AddInParameter<int>(cmd, "@StockSolutionIDTo", obj.StockSolutionIDTo);
            if (obj.InitiatedBy > default(int))
                ctx.AddInParameter<int>(cmd, "@InitiatedBy", obj.InitiatedBy);
            if (obj.InitiatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InitiatedOn",obj.InitiatedOn);
            if (obj.StockSolutionCode > default(int))
                ctx.AddInParameter<int>(cmd, "@StockSolutionID",obj.StockSolutionCode);
            ctx.AddInParameter<short>(cmd, "@PlantID", plantID);

            using (var reader = cmd.ExecuteReader())
            {
                var totalRecords = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var tnr in totalRecords)
                    list.TotalNumberOfRows = tnr;

                reader.NextResult();

                List<GetSTOCKSearchStockSolutions> gsts = new List<GetSTOCKSearchStockSolutions>();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSTOCKSearchStockSolutions>(reader);
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
