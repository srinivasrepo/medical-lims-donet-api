using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Invalidations;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Interface.Invalidations;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.Invalidations
{
    public class InvalidationsRepository : IInvalidations
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();


        public SearchResults<SearchInvalidations> SearchInvalidations(InvSearchBO obj)
        {
            var lst = new SearchResults<SearchInvalidations>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspIVSearchInvalidations");

            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            if (obj.InvalidationID > default(int))
                ctx.AddInParameter<int>(cmd, "@InvalidationID", obj.InvalidationID);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (obj.InstrumentTypeID > default(int))
                ctx.AddInParameter<int>(cmd, "@InsTypeID", obj.InstrumentTypeID);
            if (obj.InvTypeID > default(int))
                ctx.AddInParameter<int>(cmd, "@InvType", obj.InvTypeID);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateFrom", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateTo", obj.DateTo);
            ctx.AddInParameter<short>(cmd, "@PlantID", obj.PlantID);
            if (obj.SourceTypeID > default(int))
                ctx.AddInParameter<int>(cmd, "@SourceTypeID", obj.SourceTypeID);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.InstrumentID > default(int))
                ctx.AddInParameter<int>(cmd, "@InstrumentID",obj.InstrumentID);
            if (obj.ArID > default(int))
                ctx.AddInParameter<int>(cmd, "@ArID",obj.ArID);
            if (obj.MatID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatID",obj.MatID);
            if (obj.AnalysisDoneBy > default(int))
                ctx.AddInParameter<int>(cmd, "@AnalysisDoneBy",obj.AnalysisDoneBy);
            if (obj.InitiatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InitiatedOn",obj.InitiatedOn);
            if (obj.InitiatedBy > default(int))
                ctx.AddInParameter<int>(cmd, "@InitiatedBy",obj.InitiatedBy);
            if (obj.AnalysisType > default(int))
                ctx.AddInParameter<int>(cmd, "@AnalysisType",obj.AnalysisType);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRowCount = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);

                foreach (var item in rrRowCount)
                    lst.TotalNumberOfRows = item;

                reader.NextResult();

                var rrSearchResult = ((IObjectContextAdapter)context).ObjectContext.Translate<SearchInvalidations>(reader);
                SearchInvalidationsList list = new SearchInvalidationsList();

                foreach (var item in rrSearchResult)
                    list.Add(item);

                lst.SearchList = list;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public InvalidationInfo GetInvalidationData(int invalidationID, TransResults tr)
        {
            InvalidationInfo lst = new InvalidationInfo();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspINVALGetInvalidationRecordInfo");
            ctx.AddInParameter<int>(cmd, "@InvalidationID", invalidationID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<InvalidationInfo>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                reader.NextResult();

                var rrRoot = ((IObjectContextAdapter)context).ObjectContext.Translate<rootCause>(reader);
                lst.RootCause = new rootCauseList();
                foreach (var rr in rrRoot)
                    lst.RootCause.Add(rr);

                reader.NextResult();

                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                lst.Act = new RecordActionDetails();
                foreach (var rr in rrAct)
                    lst.Act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetActionAndRptData ManageInvalidationInfo(ManageInvalidationBO obj, TransResults tr)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspINVALManageInvalidationInfo");

            ctx.AddInParameter<int>(cmd, "@InvalidationID", obj.InvalidationID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            if (obj.Type == "REQ")
            {
                ctx.AddInParameter<string>(cmd, "@ImpactTypeCode", obj.ImpactTypeCode);
                ctx.AddInParameter<string>(cmd, "@SampleSetNo", obj.SampleSetNo);
                ctx.AddInParameter<string>(cmd, "@DataFileNo", obj.DataFileNo);
                ctx.AddInParameter<string>(cmd, "@BrefDesc", obj.Description);
                ctx.AddInParameter<int?>(cmd, "@InstType", obj.InstType);
                ctx.AddInParameter<int>(cmd, "@AnalysisDoneBy", obj.AnalysisDone);
            }
            else if (obj.Type == "EVAL")
            {
                if (!string.IsNullOrEmpty(obj.OtherReasons))
                    ctx.AddInParameter<string>(cmd, "@OtherResasons", obj.OtherReasons);
                ctx.AddInParameter<string>(cmd, "@EvalActRecomanded", obj.ActionsRecommended);
            }
            else
            {
                ctx.AddInParameter<string>(cmd, "@IsAnalResultValid", obj.IsReAnalysisValid);
                ctx.AddInParameter<string>(cmd, "@ImpSummary", obj.ImplimantationSummary);
                if (!string.IsNullOrEmpty(obj.ReviewActRecommended))
                    ctx.AddInParameter<string>(cmd, "@ReviewActRecomanded", obj.ReviewActRecommended);
                ctx.AddInParameter<string>(cmd, "@InitSSTResult", obj.InitSSTResult);
                ctx.AddInParameter<string>(cmd, "@InitAnaResult", obj.InitAnaResult);
                ctx.AddInParameter<string>(cmd, "@ReSSTResult", obj.ReSSTResult);
                ctx.AddInParameter<string>(cmd, "@ReAnaResult", obj.ReAnaResult);
                ctx.AddInParameter<string>(cmd, "@RootCauseCat", obj.RootXml);
                ctx.AddInParameter<string>(cmd, "@OtherRootCause", obj.OtherRootCause);
                ctx.AddInParameter<string>(cmd, "@QARemarks", obj.QARemarks);
            }

            using (var reader = cmd.ExecuteReader())
            {
                retObj.Act = new RecordActionDetails();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
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

        public SearchInvalidationList GetInvalidationTypesInstrumentTypes()
        {
            var lst = new SearchInvalidationList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetInvalidationTypesInstrumentTypes");

            using (var reader = cmd.ExecuteReader())
            {
                var rrResInstr = ((IObjectContextAdapter)context).ObjectContext.Translate<GenericBO>(reader);
                lst.InsTypes = new GenericBOList();

                foreach (var rr in rrResInstr)
                    lst.InsTypes.Add(rr);

                reader.NextResult();

                var rrResInvType = ((IObjectContextAdapter)context).ObjectContext.Translate<GenericBO>(reader);
                lst.InvTypes = new GenericBOList();

                foreach (var rr in rrResInvType)
                    lst.InvTypes.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public List<GetPreviousInvalidations> GetPreviousInvalidations(int invalidationID)
        {
            var lst = new List<GetPreviousInvalidations>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspINVGetPreviousInvalidations");

            ctx.AddInParameter<int>(cmd, "@InvalidationID", invalidationID);

            using (var reader = cmd.ExecuteReader())
            {
                var getPre = ((IObjectContextAdapter)context).ObjectContext.Translate<GetPreviousInvalidations>(reader);
                foreach (var rr in getPre)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public RecordActionDetails ManageInvalidationManualInfo(ManageInvalidationManualInfo obj, TransResults trn)
        {
            var recordDetails = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspINVALManageInvalidationManualInfo");
            if (obj.InvalidationID > default(int))
                ctx.AddInParameter<int>(cmd, "@InvalidationID",obj.InvalidationID);
            ctx.AddInParameter<string>(cmd, "@ProductName",obj.ProductName);
            ctx.AddInParameter<string>(cmd, "@Stage",obj.Stage);

            ctx.AddInParameter<string>(cmd, "@BatchNumber",obj.BatchNumber);
            ctx.AddInParameter<string>(cmd, "@ArNumber",obj.ArNumber);
            ctx.AddInParameter<int>(cmd, "@InstrumentTypeID",obj.InstrumentTypeID);
            ctx.AddInParameter<int>(cmd, "@InstrumentID",obj.InstrumentID);

            ctx.AddInParameter<string>(cmd, "@TestName",obj.TestName);
            ctx.AddInParameter<string>(cmd, "@SpecStpNumber",obj.SpecStpNumber);
            if(!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime",obj.InitTime);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrDet = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrDet)
                    recordDetails = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return recordDetails;
        }

    }
}
