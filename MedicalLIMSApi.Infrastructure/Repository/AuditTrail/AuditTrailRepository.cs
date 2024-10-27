using MedicalLIMSApi.Core.Entities.AuditTrail;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Interface.AuditTrail;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.AuditTrail
{
    public class AuditTrailRepository : IAuditTrail
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public SearchResults<AuditData> GetAuditTrailLogDetails(SearchAuditData obj)
        {
            var lst = new SearchResults<AuditData>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspAUDGetTrialDetails");
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            ctx.AddInParameter<short>(cmd, "@PlantID", obj.PlantID);
            if (obj.EntityID > default(int))
                ctx.AddInParameter<int?>(cmd, "@EntityID", obj.EntityID);
            if (!string.IsNullOrEmpty(obj.EntityRef))
                ctx.AddInParameter<string>(cmd, "@RefNum", obj.EntityRef);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime?>(cmd, "@DateFrom", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime?>(cmd, "@DateTo", obj.DateTo);
            if (obj.Action > default(int))
                ctx.AddInParameter<int?>(cmd, "@ActionID", obj.Action);
            if (obj.ActionBy > default(int))
                ctx.AddInParameter<int?>(cmd, "@ActionBY", obj.ActionBy);


            using (var reader = cmd.ExecuteReader())
            {

                var rrTot = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);

                foreach (var item in rrTot)
                    lst.TotalNumberOfRows = item;

                reader.NextResult();

                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<AuditData>(reader);

                AuditDataList list = new AuditDataList();
                foreach (var rr in rrResult)
                    list.Add(rr);

                lst.SearchList = list;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }


        public AuditDataTitleList GetAuditTableByAuditID(long auditID)
        {
            var lst = new AuditDataTitleList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspAUDGetAuditTableByAuditID");
            ctx.AddInParameter<long>(cmd, "@AuditID", auditID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrTitle = ((IObjectContextAdapter)context).ObjectContext.Translate<AuditDataTitle>(reader);

                AuditDataTitle obj = null;

                foreach (var rr in rrTitle)
                {
                    obj = new AuditDataTitle();
                    obj.AuditTableID = rr.AuditTableID;
                    obj.AuditSourceTable = rr.AuditSourceTable;
                    obj.AuditDmlType = rr.AuditDmlType;
                    lst.Add(obj);
                }
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public AuditDataByIDList GetAuditColumnsByTableID(long auditTableID)
        {
            var lst = new AuditDataByIDList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspAUDviewfromxml");
            ctx.AddInParameter<long>(cmd, "@AuditDataID", auditTableID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrList = ((IObjectContextAdapter)context).ObjectContext.Translate<AuditDataByID>(reader);

                AuditDataByID obj = null;
                foreach (var rr in rrList)
                {
                    obj = new AuditDataByID();
                    obj.ColumnName = rr.ColumnName;
                    obj.NewData = rr.NewData;
                    obj.OldData = rr.OldData;
                    obj.IsModified = rr.IsModified;
                    lst.Add(obj);
                }
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public AudObjectList GetDBObjects(int? tabID)
        {
            var lst = new AudObjectList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspAUDGetDBObjects");

            if (tabID > default(int))
                ctx.AddInParameter<int?>(cmd, "@TableID", tabID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrList = ((IObjectContextAdapter)context).ObjectContext.Translate<AudObject>(reader);

                AudObject obj = null;
                foreach (var rr in rrList)
                {
                    obj = new AudObject();
                    obj.TableID = rr.TableID;
                    obj.ObjName = rr.ObjName;
                    obj.ObjFriendlyName = rr.ObjFriendlyName;
                    lst.Add(obj);
                }
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public string ManageDBObjects(ManageAudObj obj)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspAUDSaveDBObjectNames");

            if (obj.TableID > default(int))
                ctx.AddInParameter<int>(cmd, "@TableID", obj.TableID);
            ctx.AddInParameter<string>(cmd, "@ObjectXml", obj.ObjectXml);
            ctx.AddInOutParameter<string>(cmd, "@RetVal", retVal, 10, System.Data.DbType.String);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetVal");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }
    }
}
