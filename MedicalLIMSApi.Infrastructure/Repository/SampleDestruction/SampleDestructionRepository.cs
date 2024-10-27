using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SampleDestruction;
using MedicalLIMSApi.Core.Interface.SampleDestruction;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.SampleDestruction
{
    public class SampleDestructionRepository : ISampleDestruction
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public GetResultPacksForDestructionList GetPacksForDestruction(GetPacksForDestruction obj, short plantID)
        {
            var lst = new GetResultPacksForDestructionList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samdest.uspGetPacksForDestruction");

            ctx.AddInParameter<int>(cmd, "@PlantID", plantID);
            if (obj.InvID > default(int))
                ctx.AddInParameter<int>(cmd, "@InvID", obj.InvID);
            if (obj.MatID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatID", obj.MatID);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (obj.BatchUseBeforeDate > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@BatchUseBeforeDate", obj.BatchUseBeforeDate);
            if (obj.UseBeforeDate > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@UseBeforeDate", obj.UseBeforeDate);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetResultPacksForDestruction>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetResultsDestructionSamplesDetails GetResultsDestructionSamples(int destructionID, TransResults tr)
        {
            var lst = new GetResultsDestructionSamplesDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samdest.uspGetDestructionSamples");

            ctx.AddInParameter<int>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            if (destructionID > default(int))
                ctx.AddInParameter<int>(cmd, "@DestructionID", destructionID);

            using (var reader = cmd.ExecuteReader())
            {
                lst.GetDestructionDetailsList = new List<GetDestructionSamplesDetails>();
                var rrSam = ((IObjectContextAdapter)context).ObjectContext.Translate<GetDestructionSamplesDetails>(reader);
                foreach (var item in rrSam)
                    lst.GetDestructionDetailsList.Add(item);

                reader.NextResult();

                lst.GetManageDestructionDetails = new GetManageDestructionSamplesDetails();
                var rrMag = ((IObjectContextAdapter)context).ObjectContext.Translate<GetManageDestructionSamplesDetails>(reader);
                foreach (var item in rrMag)
                    lst.GetManageDestructionDetails = item;

                reader.NextResult();

                lst.RecordActionResults = new RecordActionDetails();
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var item in rrAct)
                    lst.RecordActionResults = item;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public GetPacksForDestructionDetails SavePacksForDestruction(SavePacksForDestruction obj, TransResults tr)
        {
            var lst = new GetPacksForDestructionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samdest.uspSavePacksForDestruction");

            ctx.AddInParameter<string>(cmd, "@SourceCode", obj.SourceCode);
            ctx.AddInParameter<string>(cmd, "@PackXml", obj.PackXml);
            ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddOutParameter(cmd, "@Retcode", System.Data.DbType.String, 25);

            using (var reader = cmd.ExecuteReader())
            {

                lst.GetResultPacksForDestruction = new List<GetResultPacksForDestruction>();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetResultPacksForDestruction>(reader);
                foreach (var rr in rrRes)
                    lst.GetResultPacksForDestruction.Add(rr);

            }
            lst.Retcode = ctx.GetOutputParameterValue(cmd, "@Retcode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return lst;
        }

        public RecordActionDetails ManageDestructionSamples(ManageDestructionSamples obj, TransResults tr)
        {
            var lst = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samdest.uspManageDestructionSamples");

            if (obj.DestructionID > default(int))
                ctx.AddInParameter<int>(cmd, "@DestructionID", obj.DestructionID);
            ctx.AddInParameter<int>(cmd, "@DestructionSource", obj.DestructionSource);
            ctx.AddInParameter<int>(cmd, "@TypeOfWaste", obj.TypeOfWaste);
            if (obj.NatureOfWaste > default(int))
                ctx.AddInParameter<int>(cmd, "@NatureOfWaste", obj.NatureOfWaste);
            if (obj.ModeOfDestruction > default(int))
                ctx.AddInParameter<int>(cmd, "@ModeOfDestruction", obj.ModeOfDestruction);
            ctx.AddInParameter<string>(cmd, "@Quantity", obj.Quantity);
            if (!string.IsNullOrEmpty(obj.DisposalRemarks))
                ctx.AddInParameter<string>(cmd, "@DisposalRemarks", obj.DisposalRemarks);
            if (!string.IsNullOrEmpty(obj.RefNumber))
                ctx.AddInParameter<string>(cmd, "@RefNumber", obj.RefNumber);
            if (!string.IsNullOrEmpty(obj.DestructionOfSource))
                ctx.AddInParameter<string>(cmd, "@DestructionOfSource", obj.DestructionOfSource);
            ctx.AddInParameter<string>(cmd, "@SampleXml", obj.SampleXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public SearchResults<GetSampleDestruction> GetSampleDestruction(SearchSampleDestruction obj, short plantID)
        {
            var lst = new SearchResults<GetSampleDestruction>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samdest.uspSearchSampleDestruction");

            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            ctx.AddInParameter<int>(cmd, "@PlantID", plantID);
            if (obj.DestructionID > default(int))
                ctx.AddInParameter<int>(cmd, "@DestructionID", obj.DestructionID);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateForm", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateTo", obj.DateTo);
            if (obj.DestructionSource > default(int))
                ctx.AddInParameter<int>(cmd, "@DestructionSource", obj.DestructionSource);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (obj.WasteType > default(int))
                ctx.AddInParameter<int>(cmd, "@WasteType", obj.WasteType);
            if (obj.NatureOfWaste > default(int))
                ctx.AddInParameter<int>(cmd, "@NatureOfWaste", obj.NatureOfWaste);
            if (obj.ModeOfDestruction > default(int))
                ctx.AddInParameter<int>(cmd, "@ModeOfDestruction", obj.ModeOfDestruction);
            if (obj.MatID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatID", obj.MatID);
            if (obj.BatchNumberID > default(int))
                ctx.AddInParameter<int>(cmd, "@BatchNumberID", obj.BatchNumberID);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.CreatedUserRoleID > default(int))
                ctx.AddInParameter<int>(cmd, "@CreatedUserRoleID", obj.CreatedUserRoleID);
            if (obj.CreatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@CreatedOn", obj.CreatedOn);

            using (var reader = cmd.ExecuteReader())
            {
                var totalNoRows = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var tnr in totalNoRows)
                    lst.TotalNumberOfRows = tnr;

                reader.NextResult();

                List<GetSampleDestruction> list = new List<GetSampleDestruction>();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSampleDestruction>(reader);
                foreach (var rr in rrRes)
                    list.Add(rr);

                lst.SearchList = list;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;

        }

        public GetResultDiscardSample DiscardSamples(DiscardSamples obj, TransResults tr)
        {
            var lst = new GetResultDiscardSample();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samdest.uspDiscardSamples");

            ctx.AddInParameter<string>(cmd, "@SampleXml", obj.SampleXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddOutParameter(cmd, "@Retcode", System.Data.DbType.String, 25);
            using (var reader = cmd.ExecuteReader())
            {

                List<GetDestructionSamplesDetails> list = new List<GetDestructionSamplesDetails>();
                var rrSam = ((IObjectContextAdapter)context).ObjectContext.Translate<GetDestructionSamplesDetails>(reader);
                foreach (var item in rrSam)
                    list.Add(item);

                lst.GetDestructionDetailsList = list;

                reader.NextResult();

                lst.GetManageDestructionDetails = new GetManageDestructionSamplesDetails();
                var rrMag = ((IObjectContextAdapter)context).ObjectContext.Translate<GetManageDestructionSamplesDetails>(reader);
                foreach (var item in rrMag)
                    lst.GetManageDestructionDetails = item;

                reader.NextResult();

                lst.RecordActionResults = new RecordActionDetails();
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var item in rrAct)
                    lst.RecordActionResults = item;
            }
            lst.Retcode = ctx.GetOutputParameterValue(cmd, "@Retcode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return lst;
        }

        public GetSampleDestructionDetailsForView GetSampleDestructionDetailsForView(int destructionID)
        {
            var lst = new GetSampleDestructionDetailsForView();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samdest.uspGetSampleDestructionDetailsForView");

            ctx.AddInParameter<int>(cmd, "@DestructionID", destructionID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrDes = ((IObjectContextAdapter)context).ObjectContext.Translate<Destructions>(reader);

                lst.Destructions = new Destructions();

                foreach (var rr in rrDes)
                    lst.Destructions = rr;

                reader.NextResult();

                lst.DestructionContainerList = new List<DestructionContainer>();

                var rrDesCon = ((IObjectContextAdapter)context).ObjectContext.Translate<DestructionContainer>(reader);
                foreach (var rr in rrDesCon)
                    lst.DestructionContainerList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }
    }
}
