using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.DataReview;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Interface.DataReview;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.DataReview
{
    public class DataReviewRepository : IDataReview
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public DataReviewTestList GetTestForReview(GetReviewTests obj)
        {
            DataReviewTestList lst = new DataReviewTestList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetTestForDataReview");
            ctx.AddInParameter<int>(cmd, "@RequestID", obj.RequestID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<string>(cmd, "@RequestType", obj.RequestType);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<DataReviewTest>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetActionAndRptData ManageDataReviewData(DataReviewData obj, TransResults tr)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspDRManageDataReviewRequest");
            if (obj.ReviewID > default(int))
                ctx.AddInParameter<int>(cmd, "@ReviewID", obj.ReviewID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            if (obj.RequestType > default(int))
                ctx.AddInParameter<int>(cmd, "@RequestType", obj.RequestType);
            if (obj.RequestID > default(int))
                ctx.AddInParameter<int>(cmd, "@RequestID", obj.RequestID);
            if (!string.IsNullOrEmpty(obj.Observations))
                ctx.AddInParameter<string>(cmd, "@Observations", obj.Observations);
            if (!string.IsNullOrEmpty(obj.Recommendations))
                ctx.AddInParameter<string>(cmd, "@Recommendations", obj.Recommendations);
            if (!string.IsNullOrEmpty(obj.Remarks))
                ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            if (obj.Lst != null && obj.Lst.Count > 0)
                ctx.AddInParameter<string>(cmd, "@TestXml", obj.TestXml);
            if (obj.CheckLst != null && obj.CheckLst.Count > 0)
                ctx.AddInParameter<string>(cmd, "@ChecklistXml", obj.ChecklistXml);
            if(!string.IsNullOrEmpty(obj.DataFileNo))
                ctx.AddInParameter<string>(cmd, "@DataFileNo", obj.DataFileNo);
            if(!string.IsNullOrEmpty(obj.ApplicationSoftware))
                ctx.AddInParameter<string>(cmd, "@ApplicationSoftware",obj.ApplicationSoftware);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
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

        public GetDataReviewDetails GetDataReviewData(int reviewID, TransResults tr)
        {
            GetDataReviewDetails obj = new GetDataReviewDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspDRGetDataReviewDetails");
            ctx.AddInParameter<int>(cmd, "@ReviewID", reviewID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetDataReviewDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();
                var rrObj = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrObj)
                    obj.Act = rr;

                reader.NextResult();
                var rrRev = ((IObjectContextAdapter)context).ObjectContext.Translate<DataReviewTest>(reader);
                obj.ReviewLst = new DataReviewTestList();
                foreach (var rr in rrRev)
                    obj.ReviewLst.Add(rr);


                reader.NextResult();
                var rrpack = ((IObjectContextAdapter)context).ObjectContext.Translate<ContainerList>(reader);
                obj.PackLst = new List<ContainerList>();
                foreach (var rr in rrpack)
                    obj.PackLst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return obj;
        }

        public SearchResults<GetDataReviewDetailsBySearch> GetDataReviewDetailsBySearchId(SearchDataReview obj, short plantID)
        {
            var list = new SearchResults<GetDataReviewDetailsBySearch>();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspDRSearchDataReview");

            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (obj.RequestType > default(int))
                ctx.AddInParameter<int>(cmd, "@RequestType", obj.RequestType);
            if (obj.ArID > default(int))
                ctx.AddInParameter<int>(cmd, "@RequestID", obj.ArID);
            if (obj.MatID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatID", obj.MatID);
            if (obj.InstrumentID > default(int))
                ctx.AddInParameter<int>(cmd, "@InstrumentID", obj.InstrumentID);
            if (obj.InvID > default(int))
                ctx.AddInParameter<int>(cmd, "@InvID", obj.InvID);
            if (obj.AnalysisType > default(int))
                ctx.AddInParameter<int>(cmd, "@AnalysisType", obj.AnalysisType);
            if (obj.ScheduleType > default(int))
                ctx.AddInParameter<int>(cmd, "@ScheduleType", obj.ScheduleType);
            if (obj.MatCategoryID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatCategoryID", obj.MatCategoryID);
            if (obj.SampleID > default(int))
                ctx.AddInParameter<int>(cmd, "@SampleID", obj.SampleID);
            if (obj.DateOfReviewFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateOfReviewFrom", obj.DateOfReviewFrom);
            if (obj.DateOfReviewTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateOfReviewTo", obj.DateOfReviewTo);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (obj.InitiatedBy > default(int))
                ctx.AddInParameter<int>(cmd, "@InitiatedBy",obj.InitiatedBy);
            if (obj.InitiatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InitiatedOn",obj.InitiatedOn);
            ctx.AddInParameter<short>(cmd, "@PlantID", plantID);

            using (var reader = cmd.ExecuteReader())
            {
                var totalRecords = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var tnr in totalRecords)
                    list.TotalNumberOfRows = tnr;

                reader.NextResult();

                List<GetDataReviewDetailsBySearch> gsts = new List<GetDataReviewDetailsBySearch>();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetDataReviewDetailsBySearch>(reader);
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
