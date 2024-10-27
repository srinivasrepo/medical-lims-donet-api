using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Interface.CalibrationArds;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.CalibrationArds
{
    public class CalibrationArdsRepository : ICalibrationArds
    {
        TrainingContext context = new TrainingContext();
        DBHelper ctx = new DBHelper();

        public CalibrationArdsHeader GetCalibrationArdsHeaderInfo(int ID, int userRoleID)
        {
            CalibrationArdsHeader obj = new CalibrationArdsHeader();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.GetARDSHeaderData");
            ctx.AddInParameter<int>(cmd, "@MaintRptID", ID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", userRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<CalibrationArdsHeader>(reader);
                foreach (var rr in rrRes)
                    obj = rr;
                reader.NextResult();
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrAct)
                    obj.Act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public SearchResults<GetEquipmentMaintenance> SearchEquipmentMaintenance(SearchEquipmentMaintenance obj, short plantID)
        {
            var list = new SearchResults<GetEquipmentMaintenance>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspSearchEquipmentMaintenance");

            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (obj.EquipmentID > default(int))
                ctx.AddInParameter<int>(cmd, "@EquipmentID", obj.EquipmentID);
            if (obj.Type > default(int))
                ctx.AddInParameter<int>(cmd, "@Type", obj.Type);
            if (obj.Category > default(int))
                ctx.AddInParameter<int>(cmd, "@Category", obj.Category);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateFrom", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateTo", obj.DateTo);
            if (obj.SchType > default(int))
                ctx.AddInParameter<int>(cmd, "@SchType", obj.SchType);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.SchDate > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@SchDate", obj.SchDate);
            if (obj.CalibParamID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibParamID", obj.CalibParamID);
            if (obj.MaintanceRptID > default(int))
                ctx.AddInParameter<int>(cmd, "@MaintanceRptID", obj.MaintanceRptID);
            if (obj.ExecutionMode > default(int))
                ctx.AddInParameter<int>(cmd, "@ExecutionMode", obj.ExecutionMode);
            if (obj.ExecutionOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ExecutionOn", obj.ExecutionOn);

            ctx.AddInParameter<bool>(cmd, "@ShowDateCrossedRecords",obj.ShowDateCrossedRecords);
            ctx.AddInParameter<short>(cmd, "@PlantID", plantID);
            using (var reader = cmd.ExecuteReader())
            {
                var totalRecords = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var tnr in totalRecords)
                    list.TotalNumberOfRows = tnr;

                reader.NextResult();

                List<GetEquipmentMaintenance> gsts = new List<GetEquipmentMaintenance>();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetEquipmentMaintenance>(reader);
                foreach (var rr in rrRes)
                    gsts.Add(rr);

                list.SearchList = gsts;

                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);

            return list;
        }

        public List<GetEquipmentCategories> GetEquipmentCategories(string deptCode)
        {
            var list = new List<GetEquipmentCategories>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetEquipmentCategories");
            ctx.AddInParameter<string>(cmd, "@DeptCode", deptCode);

            using (var reader = cmd.ExecuteReader())
            {

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetEquipmentCategories>(reader);
                foreach (var rr in rrRes)
                    list.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return list;
        }

        public List<GetEquipmentTypesByCategory> GetEquipmentTypesByCategory(EquipmentTypesByCategory obj)
        {
            var list = new List<GetEquipmentTypesByCategory>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetEquipmentTypesByCategory");

            ctx.AddInParameter<int>(cmd, "@CategoryID", obj.CategoryID);
            ctx.AddInParameter<int>(cmd, "@SchTypeID", obj.SchTypeID);

            using (var reader = cmd.ExecuteReader())
            {

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetEquipmentTypesByCategory>(reader);
                foreach (var rr in rrRes)
                    list.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return list;
        }

        public List<GetScheduleTypesByDeptCode> GetScheduleTypesByDeptCode(string deptCode)
        {
            var list = new List<GetScheduleTypesByDeptCode>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.GetScheduleTypesByDeptCode");

            ctx.AddInParameter<string>(cmd, "@DeptCode", deptCode);

            using (var reader = cmd.ExecuteReader())
            {

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetScheduleTypesByDeptCode>(reader);
                foreach (var rr in rrRes)
                    list.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return list;
        }

        public RecordActionDetails RunCalibration(RunCalibration obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspRunCalibration");

            ctx.AddInParameter<int>(cmd, "@EntActID", obj.EntActID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return act;
        }

        public RecordActionDetails GenerateNewRequest(EQUPMAINInsertNewRequest obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspEQUPMAINInsertNewRequest");

            ctx.AddInParameter<int>(cmd, "@EqpMaintID", obj.EqpMaintID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<DateTime>(cmd, "@ScheduleDate",obj.ScheduleDate);

            using(var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }
    }
}
