using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Interface.Common;
using MedicalLIMSApi.Infrastructure.Context;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.Common
{
    public class CommonRepository : ICommon
    {
        TrainingContext context = new TrainingContext();
        DBHelper ctx = new DBHelper();

        public RoleMasterSmallList GetRolesDetails(byte deptID)
        {
            var lst = new RoleMasterSmallList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "dbo.uspGetRoles");
            ctx.AddInParameter<byte>(cmd, "@DeptID", deptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<RoleMasterSmall>(reader);

                foreach (var rr in rrResult)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public List<LookupData> GetLookupData(LookupInfo obj)
        {
            List<LookupData> lst = new List<LookupData>();
            LookupData lobj = null;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "qrc.uspGetLookupData");

            ctx.AddInParameter<string>(cmd, "@ViewName", obj.DbInfo.ViewName);
            ctx.AddInParameter<string>(cmd, "@Name", obj.DbInfo.Name);
            ctx.AddInParameter<string>(cmd, "@ID", obj.DbInfo.ID);
            ctx.AddInParameter<string>(cmd, "@Code", obj.DbInfo.Code);
            if (!string.IsNullOrEmpty(obj.DbInfo.ExtColName))
                ctx.AddInParameter<string>(cmd, "@ExtColName", obj.DbInfo.ExtColName);
            if (!string.IsNullOrEmpty(obj.SearchText))
                ctx.AddInParameter<string>(cmd, "@SearchText", obj.SearchText);
            if (!string.IsNullOrEmpty(obj.Condition))
                ctx.AddInParameter<string>(cmd, "@Condition", obj.Condition);
            if (obj.DbInfo.PlantFilter)
                ctx.AddInParameter<short>(cmd, "@PlantID", obj.PlantID);
            if (!string.IsNullOrEmpty(obj.ExtCondition))
                ctx.AddInParameter<string>(cmd, "@ExtCodition", obj.ExtCondition);

            using (var reader = cmd.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(reader);


                foreach (DataRow rr in dt.Rows)
                {
                    lobj = new LookupData();
                    lobj.ID = Convert.ToInt32(rr["id"]);
                    lobj.Code = Convert.ToString(rr["Code"]);
                    lobj.Name = Convert.ToString(rr["Name"]);
                    if (!string.IsNullOrEmpty(obj.DbInfo.ExtColName))
                        lobj.ExtColName = Convert.ToString(rr["ExtColName"]);
                    lst.Add(lobj);
                }

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public GetTodoListCountDetails GetUserToDoListCount(TransResults tran, string entityType)
        {
            var lst = new GetTodoListCountDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspUsrGetUserToDoList");
            ctx.AddInParameter<int>(cmd, "@UserID", tran.UserID);
            ctx.AddInParameter<int>(cmd, "@RoleID", tran.RoleID);
            ctx.AddInParameter<string>(cmd, "@ApplicationType", tran.ApplicationType);
            ctx.AddInParameter<string>(cmd, "@EntityType", entityType);
            ctx.AddInParameter<int>(cmd, "@DeptID", tran.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tran.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var tr = ((IObjectContextAdapter)context).ObjectContext.Translate<UserToDoListCount>(reader);

                lst.TodoList = new UserToDoListCountList();
                foreach (var item in tr)
                    lst.TodoList.Add(item);

                if (entityType == "SAMPLAN")
                {
                    reader.NextResult();

                    var sampleResult = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSamplePlanAnalystsDetails>(reader);

                    lst.SamplePlan = new GetSamplePlanAnalystsDetailsList();
                    foreach (var item in sampleResult)
                        lst.SamplePlan.Add(item);
                }
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public DataTable GetUserToDoListByCondition(TransResults tran)
        {

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            int i = default(int);
            SqlDatabase db = new SqlDatabase(DBInfo.GetInstance().ConnectionString);

            using (DbCommand cmd = db.GetStoredProcCommand("usr.uspUsrGetToDoListByCondition"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                ctx.AddInParameter<int>(cmd, "@ConditionID", tran.TransKey);
                ctx.AddInParameter<int>(cmd, "@RoleID", tran.RoleID);
                ctx.AddInParameter<int>(cmd, "@UserID", tran.UserID);
                ctx.AddInParameter<string>(cmd, "@ApplicationType", tran.ApplicationType);
                ctx.AddInParameter<int>(cmd, "@DeptID", tran.DeptID);
                ctx.AddInParameter<short>(cmd, "@PlantID", tran.PlantID);
                ds = db.ExecuteDataSet(cmd);
                if (i < ds.Tables.Count)
                    dt = ds.Tables[i];
                dt.Columns.Add("EncID", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    row["EncID"] = CommonStaticMethods.Encrypt(Convert.ToString(row.Field<Int32>("EntActID")));
                }
                cmd.Connection.Close();
            }
            return dt;
        }

        public CatItemsMasterList GetCatItemsByCatCode(string catCode, string type)
        {
            var lst = new CatItemsMasterList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetItemsByCategoryCode");
            ctx.AddInParameter<string>(cmd, "@CatCode", catCode);
            if (!string.IsNullOrEmpty(type))
                ctx.AddInParameter<string>(cmd, "@Type", type);


            using (var reader = cmd.ExecuteReader())
            {
                var tr = ((IObjectContextAdapter)context).ObjectContext.Translate<CatItemsMaster>(reader);


                foreach (var item in tr)
                    lst.Add(item);

                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public CategoryMasterList GetCategoryMasterList(string entityType)
        {
            var lst = new CategoryMasterList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetCategoryMasters");
            ctx.AddInParameter<string>(cmd, "@EntityType", entityType);
            using (var reader = cmd.ExecuteReader())
            {
                var tr = ((IObjectContextAdapter)context).ObjectContext.Translate<CategoryMaster>(reader);


                foreach (var item in tr)
                    lst.Add(item);

                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public string InsertCategoryItems(CatItemsMaster obj, TransResults trn)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspManageCategoryItems");
            ctx.AddInParameter<short>(cmd, "@CategoryID", obj.CategoryID);
            ctx.AddInParameter<string>(cmd, "@CatItem", obj.CatItem);
            ctx.AddInParameter<string>(cmd, "@CatItemCode", obj.CatItemCode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);

            ctx.AddInOutParameter<string>(cmd, "@RetCode", retVal, 15, DbType.String);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public SearchResults<CatItemsMaster> SearchCatItemsDetails(searchCat obj)
        {
            var lst = new SearchResults<CatItemsMaster>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspSearchCatItemsDetails");
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            if (obj.CategoryID > default(int))
                ctx.AddInParameter<int?>(cmd, "@CatagoryID", obj.CategoryID);
            if (!string.IsNullOrEmpty(obj.CatItemCode))
                ctx.AddInParameter<string>(cmd, "@CatitemCode", obj.CatItemCode);
            if (!string.IsNullOrEmpty(obj.CatItem))
                ctx.AddInParameter<string>(cmd, "@CatItem", obj.CatItem);

            ctx.AddInParameter<string>(cmd, "@EntityType", obj.EntityType);

            using (var reader = cmd.ExecuteReader())
            {
                var tr = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);

                foreach (var item in tr)
                    lst.TotalNumberOfRows = item;


                reader.NextResult();

                var itemObj = ((IObjectContextAdapter)context).ObjectContext.Translate<CatItemsMaster>(reader);
                CatItemsMaster cat = null;
                List<CatItemsMaster> list = new List<CatItemsMaster>();

                foreach (var rr in itemObj)
                {
                    cat = new CatItemsMaster();
                    cat.CatItemID = rr.CatItemID;
                    cat.CatItem = rr.CatItem;
                    cat.CatItemCode = rr.CatItemCode;
                    cat.Status = rr.Status;
                    cat.Category = rr.Category;
                    list.Add(cat);
                }
                lst.SearchList = list;
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public string ChangeCatagoryItemStatus(int catagoryItemID, TransResults trn)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspChangeCategoryItemsStatus");
            ctx.AddInParameter<int>(cmd, "@CategoryItemID", catagoryItemID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);

            ctx.AddOutParameter(cmd, "@RetCode", System.Data.DbType.String, 10);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public MenuList GetMenuList(TransResults tr, string entityType)
        {
            MenuList lst = new MenuList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetEntityList");
            ctx.AddInParameter<int>(cmd, "@RoleID", tr.RoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<string>(cmd, "@RoleType", tr.RoleType);
            ctx.AddInParameter<string>(cmd, "@EntityType", entityType);
            ctx.AddInParameter<string>(cmd, "@ApplicationType", tr.ApplicationType);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                lst.MainList = new List<NavItem>();
                var main = ((IObjectContextAdapter)context).ObjectContext.Translate<NavItem>(reader);

                foreach (var item in main)
                    lst.MainList.Add(item);


                reader.NextResult();

                lst.Childs = new List<NavItem>();
                var child = ((IObjectContextAdapter)context).ObjectContext.Translate<NavItem>(reader);

                foreach (var item in child)
                    lst.Childs.Add(item);

                reader.NextResult();

                lst.NewReqList = new List<NavItem>();
                var newList = ((IObjectContextAdapter)context).ObjectContext.Translate<NavItem>(reader);

                foreach (var item in newList)
                    lst.NewReqList.Add(item);

                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;

        }

        public GetStatusList GetStatusList(string entityCode)
        {

            var lst = new GetStatusList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.getEntityStatuslistByEntityCode");
            ctx.AddInParameter<string>(cmd, "@EntityCode", entityCode);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<GetStatus>(reader);

                foreach (var item in rrResult)
                    lst.Add(item);
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public string EntityChangeStatus(int entityActID, string entityCode, TransResults tr)
        {
            var retval = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "ams.uspChangeStatus");
            ctx.AddInParameter<int>(cmd, "@EntityActID", entityActID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", entityCode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetVal", System.Data.DbType.String, 10);
            cmd.ExecuteNonQuery();
            retval = ctx.GetOutputParameterValue(cmd, "@RetVal");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retval;
        }

        public GetViewHistory GetViewHistory(int entActID, string conditionCode)
        {
            var lst = new GetViewHistory();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspMASGetMasterActionHistory");
            ctx.AddInParameter<int>(cmd, "@EntActID", entActID);
            ctx.AddInParameter<string>(cmd, "@ConditionCode", conditionCode);

            using (var reader = cmd.ExecuteReader())
            {
                var actions = ((IObjectContextAdapter)context).ObjectContext.Translate<ViewHistory>(reader);

                ViewHistory obj = null;
                ViewHistoryList list = new ViewHistoryList();

                foreach (var act in actions)
                {
                    obj = new ViewHistory()
                    {
                        ActionResult = act.ActionResult,
                        ActionDate = act.ActionDate,
                        AppLevel = act.AppLevel,
                        ActionRemarks = act.ActionRemarks,
                        FinalAppFlag = act.FinalAppFlag,
                        ActionUserRoleID = act.ActionUserRoleID,
                        UserName = act.UserName,
                        DeptName = act.DeptName,
                        RoleName = act.RoleName,
                        ActionCode = act.ActionCode
                    };
                    list.Add(obj);
                }

                lst.list = list;
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public GetStatusList GetStatusListByCodition(string condition)
        {
            var lst = new GetStatusList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "ams.uspAUDGetStatusListByCondition");
            ctx.AddInParameter<string>(cmd, "@Condition", condition);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<GetStatus>(reader);
                foreach (var item in rrResult)
                    lst.Add(item);
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public GetDeptPlantList GetPlantDeptsList()
        {
            var lst = new GetDeptPlantList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetPlantsAndDeptMasters");

            using (var reader = cmd.ExecuteReader())
            {
                var rrResultPlantList = ((IObjectContextAdapter)context).ObjectContext.Translate<PlantMaster>(reader);

                lst.PlantList = new PlantMasterList();
                foreach (var item in rrResultPlantList)
                    lst.PlantList.Add(item);

                reader.NextResult();

                var rrResultDeptList = ((IObjectContextAdapter)context).ObjectContext.Translate<DeptMaster>(reader);

                lst.DeptList = new DeptMasterList();
                foreach (var item in rrResultDeptList)
                    lst.DeptList.Add(item);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public DeptMasterList GetDeptMaster()
        {
            DeptMasterList lst = new DeptMasterList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetDepartmentMaster");

            using (var reader = cmd.ExecuteReader())
            {
                var rrResultDeptList = ((IObjectContextAdapter)context).ObjectContext.Translate<DeptMaster>(reader);
                foreach (var item in rrResultDeptList)
                    lst.Add(item);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public PlantMasterList GetPlantMaster()
        {
            PlantMasterList lst = new PlantMasterList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetPlantMaster");

            using (var reader = cmd.ExecuteReader())
            {
                var rrResultPlantList = ((IObjectContextAdapter)context).ObjectContext.Translate<PlantMaster>(reader);

                foreach (var item in rrResultPlantList)
                    lst.Add(item);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public MaterialCatList GetMaterialCategories()
        {
            MaterialCatList lst = new MaterialCatList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetMaterialCategories");
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<MaterialCat>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public MaterialSubCatList GetMaterialSubCategories(string catCode)
        {
            MaterialSubCatList lst = new MaterialSubCatList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "dbo.uspCATGetCategoryItems");
            ctx.AddInParameter<string>(cmd, "@CAT_CODE", catCode);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<MaterialSubCat>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ProductStagesList GetProductStagesByProductID(int productID)
        {
            ProductStagesList lst = new ProductStagesList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "dbo.uspGetAllProdStages");
            ctx.AddInParameter<int>(cmd, "@PROD_ID", productID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ProductStages>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public MaterialDetails GetMaterialDetailsByMatID(int matID)
        {
            MaterialDetails obj = new MaterialDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "dbo.uspGetMaterialDetailsByMaterialID");
            ctx.AddInParameter<int>(cmd, "@MaterialID", matID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<MaterialDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public Checklist GetChecklistItemsByCategory(int entActID, string categoryCode, string entityCode)
        {
            Checklist lst = new Checklist();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetChecklistItems");
            ctx.AddInParameter<int>(cmd, "@EntityActualID", entActID);
            ctx.AddInParameter<string>(cmd, "@EntityCategoryCode", categoryCode);
            ctx.AddInParameter<string>(cmd, "@EntityCode", entityCode);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ChecklistBO>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ChecklistReportBo MangeChecklistAnswers(ManageChecklist obj, TransResults tr)
        {
            ChecklistReportBo retObj = new ChecklistReportBo(); ;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspManageChecklistData");
            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@EntitySourceCode", obj.EntitySourceCode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<string>(cmd, "@ChklistXMl", obj.ChklistXml);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (!string.IsNullOrEmpty(obj.Remarks))
                ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            ctx.AddOutParameter(cmd, "@RetMsg", DbType.String, 10);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadReports>(reader);
                retObj.Lst = new UploadReportList();
                foreach (var rr in rrRes)
                    retObj.Lst.Add(rr);
            }
            retObj.ReturnFlag = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retObj;
        }

        public AuditUnderRecordsList AuditUnderProcessRecords(TransResults trn)
        {
            var lst = new AuditUnderRecordsList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspUMGetAuditUnderProcessRecords");

            //if (trn.RoleID > default(int))
            //    ctx.AddInParameter<int>(cmd, "@RoleID", trn.RoleID);
            //if (trn.DeptID > default(int))
            //    ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            //ctx.AddInParameter<string>(cmd, "@RoleType", trn.RoleType);
            //if (trn.UserID > default(int))
            //    ctx.AddInParameter<int>(cmd, "@UserID", trn.UserID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<AuditUnderRecords>(reader);

                foreach (var rr in rrAct)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ParamMasterList GetParamMasterList(ParamFilter obj)
        {
            var lst = new ParamMasterList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetMaterialParams");

            ctx.AddInParameter<string>(cmd, "@ParamField", obj.ParamField);
            if (!string.IsNullOrEmpty(obj.ParamFType))
                ctx.AddInParameter<string>(cmd, "@ParamFType", obj.ParamFType);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResultParam = ((IObjectContextAdapter)context).ObjectContext.Translate<ParamMaster>(reader);
                foreach (var rr in rrResultParam)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string AddMaterial(AddMaterial obj, TransResults trn)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspAddNewMaterial");

            ctx.AddInParameter<string>(cmd, "@Material", obj.Material);
            ctx.AddInParameter<string>(cmd, "@MaterialAlies", obj.MaterialAlies);
            ctx.AddInParameter<int>(cmd, "@UOM", obj.MaterialUom);
            ctx.AddInParameter<int>(cmd, "@TYPE", obj.MaterialType);
            ctx.AddInParameter<int>(cmd, "@CategoryID", obj.CategoryID);
            ctx.AddInParameter<int>(cmd, "@CatItemID", obj.CatItemID);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@UserID", trn.UserID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);

            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddOutParameter(cmd, "@RetCode", DbType.String, 10);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public Preparations ManageSolventQuantityPreparation(SolventPreparationQnty obj, TransResults trn)
        {
            var lst = new Preparations();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspManageSolventQuantityPreparation");

            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            if (!string.IsNullOrEmpty(obj.QntyListXML))
                ctx.AddInParameter<string>(cmd, "@QntyXML", obj.QntyListXML);

            if (!string.IsNullOrEmpty(obj.ChemicalConsumeComments))
                ctx.AddInParameter<string>(cmd, "@ChemicalConsumeComments", obj.ChemicalConsumeComments);

            if (obj.ChemicalConsumeRefArdsExecID > default(int))
                ctx.AddInParameter<int>(cmd, "@ChemicalConsumeRefArdsExecID", obj.ChemicalConsumeRefArdsExecID);

            if (!string.IsNullOrEmpty(obj.SourceType))
                ctx.AddInParameter<string>(cmd, "@SourceType", obj.SourceType);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResultAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);

                lst.Act = new RecordActionDetails();
                foreach (var rr in rrResultAct)
                    lst.Act = rr;

                reader.NextResult();

                var rrResultSol = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);

                lst.List = new SolventsList();
                foreach (var rr in rrResultSol)
                    lst.List.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public UOMList GetUomDetailsByMaterialID(int materialID)
        {
            var lst = new UOMList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "dbo.uspGetUomByMatId");

            ctx.AddInParameter<int>(cmd, "@MAT_ID", materialID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResultSol = ((IObjectContextAdapter)context).ObjectContext.Translate<UOM>(reader);

                foreach (var rr in rrResultSol)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public CatItemsMasterList GetCatItemsByCatCodeList(GetCategoryList obj)
        {
            var lst = new CatItemsMasterList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetAllItemsByCategoryCode");
            ctx.AddInParameter<string>(cmd, "@CatCode", obj.CodeXml);
            if (!string.IsNullOrEmpty(obj.Type))
                ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            using (var reader = cmd.ExecuteReader())
            {
                var tr = ((IObjectContextAdapter)context).ObjectContext.Translate<CatItemsMaster>(reader);
                foreach (var item in tr)
                    lst.Add(item);
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public GetSamplePlanAnalystsDetailsList GetAllSamplePlanAssignedAnalysts(short plantID)
        {
            var lst = new GetSamplePlanAnalystsDetailsList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetAllSamplePlanAssignedAnalystsDetails");
            ctx.AddInParameter<short>(cmd, "@PlantID", plantID);

            using (var reader = cmd.ExecuteReader())
            {
                var sampleResult = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSamplePlanAnalystsDetails>(reader);

                foreach (var item in sampleResult)
                    lst.Add(item);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string ManageDiscardCommnets(CommentsBO obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspInsertCommentsForChangeStatus");
            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@PurposeCode", obj.PurposeCode);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<string>(cmd, "@Comments", obj.Comment);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetMsg", DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public ValidityEntityList GetValidityEntities(string entityType, TransResults tr)
        {
            ValidityEntityList lst = new ValidityEntityList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspGetValidityPeriodApplicableEntities");
            ctx.AddInParameter<string>(cmd, "@EntityType", entityType);
            ctx.AddInParameter<string>(cmd, "@ApplicationType", tr.ApplicationType);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ValidityEntities>(reader);

                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string AddValidityPeriods(AddValidityPeriods obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspAddValidityPeriodsByEntityID");
            ctx.AddInParameter<string>(cmd, "@CategoryCode", obj.CategoryCode);
            ctx.AddInParameter<int>(cmd, "@EntityID", obj.EntityID);
            ctx.AddInParameter<int>(cmd, "@CatItemID", obj.CatItemID);
            ctx.AddInParameter<int>(cmd, "@PeriodValue", obj.Value);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetCode", DbType.String, 15);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public DateTime GetCurrentDateTime()
        {
            DateTime dt = default(DateTime);

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetCurrentDateTime");
            ctx.AddOutParameter(cmd, "@DateTime", DbType.DateTime, 8);
            cmd.ExecuteNonQuery();
            dt = Convert.ToDateTime(ctx.GetOutputParameterValue(cmd, "@DateTime"));
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return dt;
        }

        public GetProductStageDetails GetProductStageDetailsByMaterialID(int materialID)
        {
            var lst = new GetProductStageDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspGetProductStageDetails");
            ctx.AddInParameter<int>(cmd, "@MaterialID", materialID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetProductStageDetails>(reader);

                foreach (var rr in rrRes)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ParamMasterList GetUomsToConvert(int materialID)
        {
            ParamMasterList lst = new ParamMasterList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspMATGetMatNonConvertbleUOM");
            ctx.AddInParameter<int>(cmd, "@MatID", materialID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<ParamMaster>(reader);
                foreach (var rr in rrLst)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public MaterialUomConvert getMaterialUomDetails(int materialID)
        {
            MaterialUomConvert obj = new MaterialUomConvert();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspMATGetMatConvertionUOMsDetails");
            ctx.AddInParameter<int>(cmd, "@MatID", materialID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UomConvertedDetails>(reader);
                obj.List = new UomConvertedList();
                foreach (var rr in rrLst)
                    obj.List.Add(rr);

                reader.NextResult();

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<MaterialUomConvert>(reader);
                foreach (var rr in rrRes)
                {
                    obj.MaterialName = rr.MaterialName;
                    obj.BaseUom = rr.BaseUom;
                }
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public string AddMaterialConvertData(ConvertionData obj, TransResults tr)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspMATAddMatConvertableUOMRangeValue");
            ctx.AddInParameter<int>(cmd, "@MaterialID", obj.MaterialID);
            ctx.AddInParameter<int>(cmd, "@TargetUomID", obj.TargetUom);
            ctx.AddInParameter<decimal>(cmd, "@TargetValue", obj.TargetValue);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetVal", DbType.String, 15);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetVal");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public string ChangeUomConvertionStatus(int convertedUomID, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspMATInActiveMatConvertedUOM");
            ctx.AddInParameter<int>(cmd, "@ConvertedUomID", convertedUomID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetVal", DbType.String, 15);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetVal");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public MaterialUomList GetMaterialUomList(int materialID, bool isActive)
        {
            MaterialUomList lst = new MaterialUomList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "dbo.uspMATGetConvertUOMsByMatID");
            ctx.AddInParameter<int>(cmd, "@MAT_ID", materialID);
            ctx.AddInParameter<bool>(cmd, "@IS_ACTIVE", isActive);
            ctx.AddOutParameter(cmd, "@MAT_UOMCODE", DbType.String, 25);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<MaterialUom>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public MaterialCatList GetAllMaterialCategories()
        {
            MaterialCatList lst = new MaterialCatList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspGetAllMaterialCategories");
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<MaterialCat>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public MaterialUomDetails GetMaterialDetailsByID(int matID)
        {
            MaterialUomDetails obj = new MaterialUomDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "uspGetMaterialDetailsByID");
            ctx.AddInParameter<int>(cmd, "@MaterialID", matID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<MaterialUomDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public GetEntityDetailsList GetEntityDetails()
        {
            var lst = new GetEntityDetailsList();

            var cmd = ctx.PrepareCommand(context);

            ctx.PrepareProcedure(cmd, "lims.uspGetQCInventoryEntityList");
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetEntityDetails>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public UOMCodeList GetMaterialUOMConvertions(string baseUOM)
        {
            var lst = new UOMCodeList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetMaterialUOMConvertions");
            if (!string.IsNullOrEmpty(baseUOM))
                ctx.AddInParameter<string>(cmd, "@BaseUOM", baseUOM);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<UOMCode>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }


        public string GetUOMConvertionDenomination(GetUOMDenomination obj)
        {
            var retCode = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspCheckConvertionUOMExists");
            ctx.AddInParameter<string>(cmd, "@SourceUOM", obj.SourceUOM);
            ctx.AddInParameter<string>(cmd, "@TargetUOM", obj.TargetUOM);
            ctx.AddInParameter<int>(cmd, "@MaterialID", obj.MaterialID);
            ctx.AddOutParameter(cmd, "@RetCode", DbType.String, 25);
            cmd.ExecuteNonQuery();
            retCode = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retCode;
        }

        public ChemicalBatch GetChemicalBatchDetailsByPackInvID(int packInvID, int refPackID)
        {
            ChemicalBatch obj = new ChemicalBatch();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspGetChemicalBatchDetailsByPackInvID");
            if (packInvID > default(int))
                ctx.AddInParameter<int>(cmd, "@PackInvID", packInvID);
            if (refPackID > default(int))
                ctx.AddInParameter<int>(cmd, "@RefPackID", refPackID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ChemicalBatch>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        //public ViewSDMSDetailsList GetSDMSDetails()
        //{
        //    var lst = new ViewSDMSDetailsList();

        //    var cmd = ctx.PrepareCommand(context);
        //    ctx.PrepareProcedure(cmd, "root.uspGetSDMSDetails");

        //    using (var reader = cmd.ExecuteReader())
        //    {
        //        var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<ViewSDMSDetails>(reader);

        //        foreach (var rr in rrResult)
        //            lst.Add(rr);

        //        cmd.Connection.Close();
        //    }
        //    ctx.CloseConnection(context);
        //    return lst;
        //}

        public List<Columns> GetExportColumns(string entityCode)
        {
            var lst = new List<Columns>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetColumnsForExportData");
            ctx.AddInParameter<string>(cmd, "@EntityCode", entityCode);
            using (var reader = cmd.ExecuteReader())
            {
                var tr = ((IObjectContextAdapter)context).ObjectContext.Translate<Columns>(reader);
                foreach (var item in tr)
                    lst.Add(item);
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public DataTable ExportData(ExportBO obj)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            int i = default(int);
            SqlDatabase db = new SqlDatabase(DBInfo.GetInstance().ConnectionString);

            using (DbCommand cmd = db.GetStoredProcCommand("root.uspGetExportData"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                db.AddInParameter(cmd, "@Columns", DbType.String, obj.Columns);
                db.AddInParameter(cmd, "@EntityCode", DbType.String, obj.EntityCode);
                if (!string.IsNullOrEmpty(obj.Condition))
                    db.AddInParameter(cmd, "@Condition", DbType.String, obj.Condition);
                if (obj.PlantFilter)
                    db.AddInParameter(cmd, "@PlantID", DbType.Int16, obj.PlantID);
                ds = db.ExecuteDataSet(cmd);
                if (i < ds.Tables.Count)
                    dt = ds.Tables[i];
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return dt;
        }

        public GetRs232IntegrationBO ManageRs232Integration(ManageRs232IntegrationBO obj, TransResults trn)
        {
            var lst = new GetRs232IntegrationBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspManageRSIntegration");

            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@SourceCode", obj.SourceCode);
            ctx.AddInParameter<int>(cmd, "@InstrumentID", obj.InstrumentID);
            ctx.AddInParameter<string>(cmd, "@OccupancyCode", obj.OccupancyCode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@UserID", trn.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            if (!string.IsNullOrEmpty(obj.RSPostFlag))
                ctx.AddInParameter<string>(cmd, "@RSPostFlag", obj.RSPostFlag);

            if (!string.IsNullOrEmpty(obj.ReqCode))
                ctx.AddInParameter<string>(cmd, "@RequestCode", obj.ReqCode);

            if (!string.IsNullOrEmpty(obj.SourceName))
                ctx.AddInParameter<string>(cmd, "@SourceName", obj.SourceName);
            if (!string.IsNullOrEmpty(obj.ChemicalName))
                ctx.AddInParameter<string>(cmd, "@ChemicalName", obj.ChemicalName);
            if (!string.IsNullOrEmpty(obj.BatchNumber))
                ctx.AddInParameter<string>(cmd, "@BatchNumber", obj.BatchNumber);

            if (!string.IsNullOrEmpty(obj.ConditionCode))
                ctx.AddInParameter<string>(cmd, "@ConditionCode", obj.ConditionCode);

            ctx.AddInParameter<bool>(cmd, "@IsComingLabchamical", obj.IsComingLabchamical);
            ctx.AddInParameter<string>(cmd, "@RS232Mode", obj.RS232Mode);
            ctx.AddInParameter<string>(cmd, "@OccSource", obj.OccSource);

            ctx.AddInParameter<string>(cmd, "@SectionCode", obj.SectionCode);

            ctx.AddInParameter<int>(cmd, "@ParentID", CommonStaticMethods.Decrypt<int>(obj.ParentID));

            if (obj.RefEqpOccID > default(int))
                ctx.AddInParameter<int>(cmd, "@RefEqpOccID", obj.RefEqpOccID);


            using (var reader = cmd.ExecuteReader())
            {
                var result = ((IObjectContextAdapter)context).ObjectContext.Translate<GetRs232IntegrationBO>(reader);
                foreach (var item in result)
                    lst = item;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetRs232IntegrationBO GetRs232Integration(ManageRs232IntegrationBO obj)
        {

            var lst = new GetRs232IntegrationBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspRS232IntegrationDetails");

            ctx.AddInParameter<int>(cmd, "@PreparationID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@SourceCode", obj.SourceCode);


            using (var reader = cmd.ExecuteReader())
            {
                var result = ((IObjectContextAdapter)context).ObjectContext.Translate<GetRs232IntegrationBO>(reader);
                foreach (var item in result)
                    lst = item;

                reader.NextResult();

                var resultSDMS = ((IObjectContextAdapter)context).ObjectContext.Translate<SDMSRSIntegrationBO>(reader);

                lst.SDMSList = new SDMSRSIntegrationBOList();

                foreach (var item in resultSDMS)
                    lst.SDMSList.Add(item);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public List<GetSysConfigurationData> GetSysConfigurationData()
        {
            var lst = new List<GetSysConfigurationData>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetSysConfigurationData");

            using (var reader = cmd.ExecuteReader())
            {
                var result = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSysConfigurationData>(reader);
                foreach (var item in result)
                    lst.Add(item);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public string UpdateSysConfiguration(UpdateSysConfiguration obj)
        {
            var retCode = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspUpdateSysConfiguration");

            ctx.AddInParameter<int>(cmd, "@ConfigID", obj.ConfigID);
            ctx.AddInParameter<string>(cmd, "@ConfigValue", obj.ConfigValue);
            ctx.AddOutParameter(cmd, "@RetCode", DbType.String, 10);
            cmd.ExecuteNonQuery();
            retCode = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retCode;
        }

        public GetSpecHeaderInfo GetSpecHeaderInfo(int specID, int calibID, TransResults tr)
        {
            var lst = new GetSpecHeaderInfo();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "spec.uspSPECGetSpecHeaderInfo");
            if (specID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecID", specID);
            if (calibID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibParamID", calibID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            using (var reader = cmd.ExecuteReader())
            {
                var result = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSpecHeaderInfo>(reader);
                foreach (var item in result)
                    lst = item;

                reader.NextResult();

                lst.GetSpecificationData = new List<GetSpecificationData>();
                var getSpec = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSpecificationData>(reader);
                foreach (var rr in getSpec)
                    lst.GetSpecificationData.Add(rr);

                cmd.Connection.Close();
            }
            cmd.Connection.Close();

            return lst;
        }

        public List<CAPAGetCAPAActionsBySourceRefType> CAPAGetCAPAActionsBySourceRefType(CAPAActionsBySourceRefType obj)
        {
            var lst = new List<CAPAGetCAPAActionsBySourceRefType>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspCAPAGetCAPAActionsBySourceRefType");

            ctx.AddInParameter<int>(cmd, "@SourceRefID", obj.SourceRefID);
            ctx.AddInParameter<string>(cmd, "@SourceRefCode", obj.SourceRefCode);
            if (!string.IsNullOrEmpty(obj.CapaType))
                ctx.AddInParameter<string>(cmd, "@CapaType", obj.CapaType);
            if (!string.IsNullOrEmpty(obj.CapaModuleCode))
                ctx.AddInParameter<string>(cmd, "@CapaModuleCode", obj.CapaModuleCode);

            using (var reader = cmd.ExecuteReader())
            {
                var getList = ((IObjectContextAdapter)context).ObjectContext.Translate<CAPAGetCAPAActionsBySourceRefType>(reader);
                foreach (var rr in getList)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            cmd.Connection.Close();

            return lst;
        }

        public CAPAInsertUpdateCAPAResult CAPAInsertUpdateCAPA(CAPAInsertUpdateCAPA obj, TransResults tr)
        {
            var lst = new CAPAInsertUpdateCAPAResult();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspCAPAInsertUpdateCAPA");
            if (obj.CapaID > default(int))
                ctx.AddInParameter<int>(cmd, "@CapaID", obj.CapaID);
            ctx.AddInParameter<string>(cmd, "@Capa", obj.Capa);
            if (obj.TargetDate > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@TargetDate", obj.TargetDate);
            ctx.AddInParameter<string>(cmd, "@CapaType", obj.CapaType);
            if (obj.CapaSourceID > default(int))
                ctx.AddInParameter<int>(cmd, "@CapaSourceID", obj.CapaSourceID);
            if (!string.IsNullOrEmpty(obj.CapaSourceCode))
                ctx.AddInParameter<string>(cmd, "@CapaSourceCode", obj.CapaSourceCode);
            if (!string.IsNullOrEmpty(obj.CapaSourceCode))
                ctx.AddInParameter<string>(cmd, "@CapaSrcOthers", obj.CapaSrcOthers);
            if (obj.SourceReferenceID > default(int))
                ctx.AddInParameter<int>(cmd, "@SourceReferenceID", obj.SourceReferenceID);
            if (!string.IsNullOrEmpty(obj.SourceReference))
                ctx.AddInParameter<string>(cmd, "@SourceReference", obj.SourceReference);
            if (!string.IsNullOrEmpty(obj.CapaOwner))
                ctx.AddInParameter<string>(cmd, "@CapaOwner	", obj.CapaOwner);
            if (obj.BulidID > default(int))
                ctx.AddInParameter<int>(cmd, "@BulidID", obj.BulidID);
            if (!string.IsNullOrEmpty(obj.CapaNature))
                ctx.AddInParameter<string>(cmd, "@CapaNature", obj.CapaNature);
            if (!string.IsNullOrEmpty(obj.QualityIssDesc))
                ctx.AddInParameter<string>(cmd, "@QualityIssDesc", obj.QualityIssDesc);
            if (!string.IsNullOrEmpty(obj.DoctString))
                ctx.AddInParameter<string>(cmd, "@DoctString", obj.DoctString);
            if (obj.Type > default(int))
                ctx.AddInParameter<int>(cmd, "@Type", obj.Type);
            if (obj.AreaOfImplementation > default(int))
                ctx.AddInParameter<int>(cmd, "@AreaOfImplementation", obj.AreaOfImplementation);
            if (obj.ScopeOfCapa > default(int))
                ctx.AddInParameter<int>(cmd, "@ScopeOfCapa", obj.ScopeOfCapa);
            if (obj.CapaOwnerID > default(int))
                ctx.AddInParameter<int>(cmd, "@CapaOwnerID", obj.CapaOwnerID);
            ctx.AddInParameter<bool>(cmd, "@IsNewCapa", obj.IsNewCapa);
            ctx.AddInParameter<bool>(cmd, "@IsFromUC", obj.IsFromUC);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<string>(cmd, "@DeptCode", obj.ModuleCode);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInOutParameter<string>(cmd, "@InitTime", obj.InitTime, 15, DbType.String);
            ctx.AddOutParameter(cmd, "@RetVal", DbType.Int16, 10);

            cmd.ExecuteNonQuery();

            lst.InitTime = ctx.GetOutputParameterValue(cmd, "@InitTime");
            lst.RetVal = Convert.ToInt32(ctx.GetOutputParameterValue(cmd, "@RetVal"));

            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return lst;
        }

        public string ManageRS232RequestMode(ManageRS232RequestModeBO obj, TransResults trn)
        {
            string retCode = string.Empty;


            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspManageRS232RequestMode");

            ctx.AddInParameter<string>(cmd, "@ConditionCode", obj.ConditionCode);
            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);

            if (!string.IsNullOrEmpty(obj.SectionCode))
                ctx.AddInParameter<string>(cmd, "@SectionCode", obj.SectionCode);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            ctx.AddInOutParameter<string>(cmd, "@ReqMode", obj.ReqCode, 3, DbType.String);

            cmd.ExecuteNonQuery();

            retCode = ctx.GetOutputParameterValue(cmd, "@ReqMode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retCode;
        }

        public string EQPUpdateToDateTime(int occupancyID, TransResults trn)
        {
            string retCode = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspEQPUpdateToDateTime");

            ctx.AddInParameter<int>(cmd, "@OccupancyID", occupancyID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInOutParameter<string>(cmd, "@RetCode", retCode, 5, DbType.String);
            cmd.ExecuteNonQuery();
            retCode = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retCode;
        }

        public string ManageRs232IntegrationOther(ManageRS232IntegrationOtherBO obj, TransResults trn)
        {
            string retval = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspManageRs232IntegrationOther");

            if (obj.Rs232IntegrationID > default(int))
                ctx.AddInParameter<int>(cmd, "@Rs232OtherID", obj.Rs232IntegrationID);

            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActualID);
            ctx.AddInParameter<string>(cmd, "@ConditionCode", obj.ConditionCode);
            ctx.AddInParameter<string>(cmd, "@ReqCode", obj.ReqCode);

            if (!string.IsNullOrEmpty(obj.KeyTitle))
                ctx.AddInParameter<string>(cmd, "@KeyTitle", obj.KeyTitle);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            if (!string.IsNullOrEmpty(obj.KeyActualValue))
                ctx.AddInParameter<string>(cmd, "@KeyActualValue", obj.KeyActualValue);

            if (obj.KeyValue > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@KeyValue", obj.KeyValue);

            ctx.AddInOutParameter<string>(cmd, "@RetCode", retval, 3, DbType.String);
            cmd.ExecuteNonQuery();
            retval = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retval;
        }

        public GetRS232IntegrationOtherBOList GetRs232IntegrationOther(ManageRS232IntegrationOtherBO obj)
        {
            var lst = new GetRS232IntegrationOtherBOList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspGetRs232IntegrationOther");

            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActualID);
            ctx.AddInParameter<string>(cmd, "@ConditionCode", obj.ConditionCode);

            using (var reader = cmd.ExecuteReader())
            {
                var result = ((IObjectContextAdapter)context).ObjectContext.Translate<GetRS232IntegrationOtherBO>(reader);
                foreach (var rr in result)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public UploadReportList GetReportsInfoForSyncToDMS(string entityType)
        {
            UploadReportList lst = new UploadReportList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspRPTGetReportInfoForSyncToDMS");
            if (!string.IsNullOrEmpty(entityType))
                ctx.AddInParameter<string>(cmd, "@EntityType", entityType);
            using (var reader = cmd.ExecuteReader())
            {
                var result = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadReports>(reader);
                foreach (var rr in result)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string ResetRs232EqpOtherOcc(ResetRS232IntegrationBO obj, TransResults trn)
        {
            string retCode = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspResetRS232Integration");

            ctx.AddInParameter<int>(cmd, "@RS232IntegrationID", obj.RS232IntegrationID);
            ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            ctx.AddInOutParameter<string>(cmd, "@RetCode", retCode, 5, DbType.String);
            cmd.ExecuteNonQuery();
            retCode = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retCode;
        }

        public string ManageRS232OtherFieldsValues(string xmlString, TransResults trn)
        {
            string retCode = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "rawdata.uspManageOtherFieldValues");

            ctx.AddInParameter<string>(cmd, "@OtherFieldsXML", xmlString);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            ctx.AddInOutParameter<string>(cmd, "@RetCode", retCode, 5, DbType.String);
            cmd.ExecuteNonQuery();
            retCode = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retCode;
        }

        public string DeleteChecklist(ManageChecklist obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspCHKDeleteAddedChecklist");

            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@EntitySource", obj.EntitySourceCode);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);

            ctx.AddInOutParameter<string>(cmd, "@RetMsg", retMsg, 25, DbType.String);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retMsg;
        }
    }
}
