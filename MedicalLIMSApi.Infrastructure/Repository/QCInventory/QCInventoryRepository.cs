using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.QCInventory;
using MedicalLIMSApi.Core.Interface.QCInventory;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.QCInventory
{
    public class QCInventoryRepository : IQCInventory
    {
        TrainingContext context = new TrainingContext();
        DBHelper ctx = new DBHelper();

        public SearchResults<GetQCInventoryItems> getQCInventoryList(SearchQCInventory obj, TransResults tr)
        {
            var list = new SearchResults<GetQCInventoryItems>();
            List<GetQCInventoryItems> gqcList = new List<GetQCInventoryItems>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspSearchQCInventory");
            ctx.AddInParameter(cmd, "@PlantID", tr.PlantID);
            if (obj.InvID > default(int))
                ctx.AddInParameter<int>(cmd, "@InvID", obj.InvID);
            if (obj.MatID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatID", obj.MatID);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (obj.EntityID > default(int))
                ctx.AddInParameter<int>(cmd, "@EntityID", obj.EntityID);
            if (obj.ChemicalType > default(int))
                ctx.AddInParameter<int>(cmd, "@ChemicalType", obj.ChemicalType);
            if (obj.BatchUseBeforeDate > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@BatchUseBeforeDate", obj.BatchUseBeforeDate);
            if (obj.UseBeforeDate > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@UseBeforeDate", obj.UseBeforeDate);
            if (obj.InwardDateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InwardDateFrom", obj.InwardDateFrom);
            if (obj.InwardDateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InwardDateTo", obj.InwardDateTo);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.CategoryID > default(int))
                ctx.AddInParameter<int>(cmd, "@CategoryID", obj.CategoryID);
            if (obj.SubCategoryID > default(int))
                ctx.AddInParameter<int>(cmd, "@SubCategoryID", obj.SubCategoryID);
            if (obj.ChemicalGrade > default(int))
                ctx.AddInParameter<int>(cmd, "@ChemicalGrade", obj.ChemicalGrade);
            if (obj.BlockID > default(int))
                ctx.AddInParameter<int>(cmd, "@BlockID", obj.BlockID);
            if (obj.ManufactureID > default(int))
                ctx.AddInParameter<int>(cmd, "@ManufactureID", obj.ManufactureID);
            if (obj.BatchExpDateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@BatchExpDateTo", obj.BatchExpDateTo);
            ctx.AddInParameter<bool>(cmd, "@ShowZeroQtyRecords", obj.ShowZeroQtyRecords);
            ctx.AddInParameter(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter(cmd, "@PageSize", obj.PageSize);
            using (var reader = cmd.ExecuteReader())
            {
                var totalRecords = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var tnr in totalRecords)
                    list.TotalNumberOfRows = tnr;

                reader.NextResult();

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQCInventoryItems>(reader);
                foreach (var rr in rrRes)
                    gqcList.Add(rr);

                list.SearchList = gqcList;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return list;
        }

        public QCInventoryBatchDetails GetQCInventoryDetails(int invSourceID, int UserRoleID)
        {
            QCInventoryBatchDetails obj = new QCInventoryBatchDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetQCInventoryDetails");
            ctx.AddInParameter<int>(cmd, "@InvSourceID", invSourceID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", UserRoleID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<QCInventoryBatchDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();

                obj.List = new ChemicalList();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<ChemicalDetials>(reader);
                foreach (var rr in rrLst)
                    obj.List.Add(rr);

                reader.NextResult();

                obj.Act = new RecordActionDetails();
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrAct)
                    obj.Act = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public PackDetails GetQCInvPackDetails(int invID)
        {
            PackDetails obj = new PackDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetQCInvPackDetails");
            ctx.AddInParameter<int>(cmd, "@InvID", invID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<PackDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();

                obj.List = new PackList();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<Packs>(reader);
                foreach (var rr in rrLst)
                    obj.List.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public string ManageQCInvPackDetails(ManagePacks obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspSaveQCInvPackDetails");

            ctx.AddInParameter<int>(cmd, "@InvID", obj.InvID);
            ctx.AddInParameter<string>(cmd, "@PackXml", obj.PackXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            if (obj.NatureTypeID > default(int))
                ctx.AddInParameter<int>(cmd, "@NatureType", obj.NatureTypeID);
            ctx.AddOutParameter(cmd, "@Retcode", DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@Retcode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public string ManageQCBatchDetails(ManageBatches obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspSaveQCInventoryDetails");

            ctx.AddInParameter<int>(cmd, "@InvSourceID", obj.InvSourceID);
            ctx.AddInParameter<string>(cmd, "@BatchXml", obj.BatchXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@Retcode", DbType.String, 25);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd,"@InitTime",obj.InitTime);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@Retcode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public string ManageQCInventory(ManageQCInventory manageObj, TransResults transObj)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspQCINVGenerateInvBatch");


            ctx.AddInParameter<int>(cmd, "@MatID", manageObj.MatID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", manageObj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@MfgID", manageObj.MfgID);
            ctx.AddInParameter<decimal>(cmd, "@Qty", manageObj.Qty);
            ctx.AddInParameter<string>(cmd, "@BatchNumber", manageObj.BatchNumber);
            ctx.AddInParameter<int>(cmd, "@PlantID", transObj.PlantID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", transObj.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", transObj.DeptID);

            ctx.AddInOutParameter<string>(cmd, "@Retmsg", retMsg, 25, DbType.String);

            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@Retmsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public ViewInvtDetails ViewInvtDetailsByInvID(GetPackInvDetails obj)
        {
            var list = new ViewInvtDetails();
            ViewInvtList viewList = new ViewInvtList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetQCInventoryDetailsForView");

            if (obj.InvID > default(int))
                ctx.AddInParameter<int>(cmd, "@InvID", obj.InvID);
            else if (obj.InvPackID > default(int))
                ctx.AddInParameter<int>(cmd, "@InvPackID", obj.InvPackID);

            using (var reader = cmd.ExecuteReader())
            {
                var invDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<InvtViewData>(reader);
                list.InvViewData = new InvtViewData();

                foreach (var inv in invDetails)
                    list.InvViewData = inv;

                reader.NextResult();

                var actionList = ((IObjectContextAdapter)context).ObjectContext.Translate<ViewInvtList>(reader);
                list.ViewList = new ViewInvtsList();
                foreach (var item in actionList)
                    list.ViewList.Add(item);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return list;
            
        }

        public GetActionAndRptData openPack(OpenPack obj, TransResults tr)
        {
            var cmd = ctx.PrepareCommand(context);

            GetActionAndRptData retObj = new GetActionAndRptData();

            ctx.PrepareProcedure(cmd, "lims.uspOpenPack");
            ctx.AddInParameter<int>(cmd, "@PackInvID", obj.PackInvID);
            if (obj.ValidityPeriodID > default(int))
                ctx.AddInParameter<int>(cmd, "@ValidityPeriodID", obj.ValidityPeriodID);
            if (!string.IsNullOrEmpty(obj.StatusCode))
                ctx.AddInParameter<string>(cmd, "@StatusCode", obj.StatusCode);
            if (!string.IsNullOrEmpty(obj.Remarks))
                ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddOutParameter(cmd, "@Retcode", System.Data.DbType.String, 25);
            ctx.AddOutParameter(cmd, "@RefCode", System.Data.DbType.String, 25);

            using (var reader = cmd.ExecuteReader())
            {
                retObj.RptList = new UploadReportList();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadReports>(reader);
                foreach (var rr in rrLst)
                    retObj.RptList.Add(rr);
            }
            retObj.ReturnFlag = ctx.GetOutputParameterValue(cmd, "@Retcode");
            retObj.refCode = ctx.GetOutputParameterValue(cmd, "@RefCode");

            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retObj;
        }

        public GetMiscConsumptionDetails GetMiscConsumptionDetails(int packInvID)
        {
            GetMiscConsumptionDetails obj = new GetMiscConsumptionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetQCInvMiscConsumptionDetails");
            ctx.AddInParameter<int>(cmd, "@PackInvID", packInvID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetMiscConsumptionDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();
                obj.List = new ConsumptionList();

                var rrData = ((IObjectContextAdapter)context).ObjectContext.Translate<Consumption>(reader);
                foreach (var rr in rrData)
                    obj.List.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public string ManageMiscConsumptionData(MiscConsumption obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspSaveQCInvMiscConsumption");
            ctx.AddInParameter<int>(cmd, "@PackInvID", obj.PackInvID);
            ctx.AddInParameter<decimal>(cmd, "@Qty", obj.Qty);
            ctx.AddInParameter<string>(cmd, "@UomCode", obj.Uom);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            if (!string.IsNullOrEmpty(obj.Remarks))
                ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            ctx.AddOutParameter(cmd, "@Retcode", DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@Retcode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public PackInvDetailsList GetPackInventoryReservationsDetails(int PackInvID)
        {
            var lst = new PackInvDetailsList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetPackInventoryReservationsDetails");
            ctx.AddInParameter<int>(cmd, "@PackInvID", PackInvID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<PackInvDetails>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetQCInventorySourcesList GetQCInventorySources()
        {
            var lst = new GetQCInventorySourcesList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetQCInventorySources");

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQCInventorySources>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetBatchSplitDetailsBO GenerateNewBatchSplit(QCInvBatchSplitBO obj, TransResults trn)
        {
            var lst = new GetBatchSplitDetailsBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspQCInvNewBatchSplit");
            ctx.AddInParameter<int>(cmd, "@InvID", obj.InvID);
            ctx.AddInParameter<int>(cmd, "@InvSourceID", obj.InvSourceID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<decimal>(cmd, "@BatchQnty", obj.BatchQnty);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetBatchSplitDetailsBO>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                reader.NextResult();

                lst.List = new ChemicalList();
                var rrChemicalRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ChemicalDetials>(reader);
                foreach (var rr in rrChemicalRes)
                    lst.List.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetBatchSplitDetailsBO DeleteNewBatchSplit(int invID, TransResults trn)
        {
            var lst = new GetBatchSplitDetailsBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspQCInvDeleteBatchSplit");
            ctx.AddInParameter<int>(cmd, "@InvID", invID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetBatchSplitDetailsBO>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                reader.NextResult();

                lst.List = new ChemicalList();
                var rrChemicalRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ChemicalDetials>(reader);
                foreach (var rr in rrChemicalRes)
                    lst.List.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string SendBatchForSample(int invID, TransResults trn)
        {
            var returnValue = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspSendBatchForSample");

            ctx.AddInParameter<int>(cmd, "@InvID", invID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<string>(reader);
                foreach (var rr in rrRes)
                    returnValue = rr;
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return returnValue;
        }
    }
}
