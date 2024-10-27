using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Interface.Common;
using MedicalLIMSApi.Web.App_Start;
using MedicalLIMSApi.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.Common
{
    [LIMSAuthorization]
    public class CommonController : ApiController
    {
        ICommon db;
        public CommonController(ICommon db)
        {
            this.db = db;
        }


        [HttpGet]
        [Route("GetRoles")]
        public RoleMasterSmallList GetRoles(byte deptID)
        {
            return db.GetRolesDetails(deptID);
        }

        [HttpGet]
        [Route("GetLookupData")]
        public List<LookupData> GetLookupData(string lookupCode, string condition, string searchText, string purpose, bool isRs232Mode)
        {

            if (condition.IndexOf('[') > -1)
            {
                var length = condition.Length;

                var firstIndex = condition.IndexOf('[');
                var lastIndex = condition.LastIndexOf(']');

                var encKey = condition.Substring(firstIndex + 1, lastIndex - (firstIndex + 1));
                int key = CommonStaticMethods.Decrypt<int>(encKey);

                var splt = condition.Split('[');
                var splt2 = condition.Split(']');
                condition = splt[0] + key + splt2[1];

            }

            LookupInfo obj = new LookupInfo();
            obj.Purpose = purpose;
            obj.IsRs232Mode = isRs232Mode;
            obj.LookupCode = lookupCode;
            obj.Condition = condition == "undefined" ? "" : condition;
            obj.SearchText = searchText;
            obj.PlantID = Utilities.Common.GetUserDetails().PlantID;

            if (obj.Purpose == "MEDICALLIMSCS")
            {
                List<LookupData> list = CommonStaticMethods.PostApiConnectionData<List<LookupData>>("GetLookUpData", obj);
                return list;
            }
            if (obj.Purpose == "DMS")
            {
                List<LookupData> list = CommonStaticMethods.PostApiConnectionData<List<LookupData>>("GetLookUpData", obj, "DMS_URL");
                return list;
            }

            return db.GetLookupData(obj);
        }


        [HttpGet]
        [Route("GetUserToDoListCount")]

        public GetTodoListCountDetails GetUserToDoListCount(string entityType)
        {
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            return db.GetUserToDoListCount(trn, entityType);
        }

        [HttpGet]
        [Route("GetToDoListByCondition")]
        public DataTable GetToDoListByCondition(int conditionID)
        {
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            trn.TransKey = conditionID;
            return db.GetUserToDoListByCondition(trn);
        }


        [HttpGet]
        [Route("GetCatItemsByCatCode")]
        public CatItemsMasterList GetCatItemsByCatCode(string catCode, string type)
        {
            return db.GetCatItemsByCatCode(catCode, type);
        }

        [HttpGet]
        [Route("GetCategoryMasterList")]
        public CategoryMasterList GetCategoryMasterList(string entityType)
        {
            return db.GetCategoryMasterList(entityType);
        }

        [HttpPost]
        [Route("InsertCategoryItems")]
        public string InsertCategoryItems(CatItemsMaster obj)
        {
            return db.InsertCategoryItems(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SearchCatItemsDetails")]
        public SearchResults<CatItemsMaster> SearchCatItemsDetails(searchCat obj)
        {
            return db.SearchCatItemsDetails(obj);
        }


        [HttpPost]
        [Route("ChangeCatagoryItemStatus")]
        public string ChangeCatagoryItemStatus(int catItemID)
        {
            return db.ChangeCatagoryItemStatus(catItemID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetMenuList")]
        public MenuList GetMenuList(string entityType)
        {
            MenuList lst = db.GetMenuList(MedicalLIMSApi.Web.Utilities.Common.GetUserDetails(), entityType);
            foreach (NavItem item in lst.MainList)
            {
                List<NavItem> childs = lst.Childs.FindAll(delegate (NavItem s) { return s.EntityType == item.EntityType; });
                if (childs.Count > 1)
                    item.Children = childs;
                else if (childs.Count == 1)
                    item.Route = childs[0].Route;
            }

            return lst;
        }

        [HttpGet]
        [Route("GetStatusList")]
        public GetStatusList GetStatusList(string entityCode)
        {
            return db.GetStatusList(entityCode);
        }

        [HttpGet]
        [Route("ChangeStatus")]
        public string EntityChangeStatus(string entityActID, string entityCode)
        {
            int entityID = default(int);
            entityID = CommonStaticMethods.Decrypt<int>(entityActID);
            return db.EntityChangeStatus(entityID, entityCode, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("ViewHistory")]
        public GetViewHistory GetViewHistory(string encEntActID, string conditionCode)
        {
            int entActID = default(int);
            entActID = CommonStaticMethods.Decrypt<int>(encEntActID);
            return db.GetViewHistory(entActID, conditionCode);
        }

        [HttpGet]
        [Route("GetStatusListByCodition")]
        public GetStatusList GetStatusListByCodition(string condition)
        {
            return db.GetStatusListByCodition(condition);
        }

        [HttpGet]
        [Route("GetPlantDeptsList")]
        public GetDeptPlantList GetPlantDeptsList()
        {
            return db.GetPlantDeptsList();
        }

        [HttpGet]
        [Route("GetDeptMaster")]
        public DeptMasterList GetDeptMaster()
        {
            return db.GetDeptMaster();
        }

        [HttpGet]
        [Route("GetPlantMaster")]
        public PlantMasterList GetPlantMaster()
        {
            return db.GetPlantMaster();
        }

        [HttpGet]
        [Route("GetMaterialCategories")]
        public MaterialCatList GetMaterialCategories()
        {
            return db.GetMaterialCategories();
        }

        [HttpGet]
        [Route("GetMaterialSubCategories")]
        public MaterialSubCatList GetMaterialSubCategories(string catCode)
        {
            return db.GetMaterialSubCategories(catCode);
        }

        [HttpGet]
        [Route("GetProductStagesByProductID")]
        public ProductStagesList GetProductStagesByProductID(int productID)
        {
            return db.GetProductStagesByProductID(productID);
        }

        [HttpPost]
        [Route("ManageOccupancy")]
        public GetOccupancyDetails ManageOccupancy(ManageOccupancy obj)
        {
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            obj.PlantID = trn.PlantID;
            obj.UserID = trn.UserID;
            obj.DeptCode = trn.DeptCode;
            obj.UserRoleID = trn.UserRoleID;

            return CommonStaticMethods.PostApiConnectionData<GetOccupancyDetails>("ManageOccupancy", obj);
        }

        [HttpGet]
        [Route("GetMaterialDetailsByMatID")]

        public MaterialDetails GetMaterialDetailsByMatID(int matID)
        {
            return db.GetMaterialDetailsByMatID(matID);
        }

        [HttpGet]
        [Route("GetChecklistItemsByCategory")]
        public Checklist GetChecklistItemsByCategory(string encEntActID, string categoryCode, string entityCode)
        {
            int entActID = default(int);
            entActID = CommonStaticMethods.Decrypt<int>(encEntActID);
            return db.GetChecklistItemsByCategory(entActID, categoryCode, entityCode);
        }

        [HttpPost]
        [Route("MangeChecklistAnswers")]
        public string MangeChecklistAnswers(ManageChecklist obj)
        {
            ChecklistReportBo retObj = new ChecklistReportBo();
            retObj = db.MangeChecklistAnswers(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            if (retObj.ReturnFlag == "SUCCESS" && retObj.Lst != null && retObj.Lst.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = obj.EntityActID;
                dmsObj.EntityCode = obj.EntityCode;
                dmsObj.ReferenceNumber = retObj.Lst[0].ReferenceCode;
                dmsObj.List = retObj.Lst;
                string retCode = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
                if (retCode != "OK" && retCode != "SUCCESS")
                    retObj.ReturnFlag = retCode;
            }

            return retObj.ReturnFlag;
        }

        [HttpGet]
        [Route("AuditUnderProcessRecords")]
        public AuditUnderRecordsList AuditUnderProcessRecords()
        {
            return db.AuditUnderProcessRecords(MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetParamMasterData")]
        public ParamMasterList GetParamMasterList(ParamFilter obj)
        {
            return db.GetParamMasterList(obj);
        }

        [HttpPost]
        [Route("AddMaterial")]
        public string AddMaterial(AddMaterial obj)
        {
            return db.AddMaterial(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageSolventQuantityPreparation")]
        public Preparations ManageSolventQuantityPreparation(SolventPreparationQnty obj)
        {
            return db.ManageSolventQuantityPreparation(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetUomDetailsByMaterialID")]
        public UOMList GetUomDetailsByMaterialID(int materialID)
        {
            return db.GetUomDetailsByMaterialID(materialID);
        }

        [HttpPost]
        [Route("GetCatItemsByCatCodeList")]
        public CatItemsMasterList GetCatItemsByCatCodeList(GetCategoryList obj)
        {
            return db.GetCatItemsByCatCodeList(obj);
        }

        [HttpGet]
        [Route("GetAllSamplePlanAssignedAnalysts")]
        public GetSamplePlanAnalystsDetailsList GetAllSamplePlanAssignedAnalysts()
        {
            return db.GetAllSamplePlanAssignedAnalysts(MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID);
        }

        [HttpPost]
        [Route("ManageDiscardCommnets")]
        public string ManageDiscardCommnets(CommentsBO obj)
        {
            return db.ManageDiscardCommnets(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetValidityEntities")]
        public ValidityEntityList GetValidityEntities(string entityType)
        {
            return db.GetValidityEntities(entityType, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("AddValidityPeriods")]
        public string AddValidityPeriods(AddValidityPeriods obj)
        {
            return db.AddValidityPeriods(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetCurrentDateTime")]
        public DateTime GetCurrentDateTime()
        {
            return db.GetCurrentDateTime();
        }

        [HttpGet]
        [Route("GetProductStageDetailsByMaterialID")]
        public GetProductStageDetails GetProductStageDetailsByMaterialID(int materialID)
        {
            return db.GetProductStageDetailsByMaterialID(materialID);
        }

        [HttpGet]
        [Route("GetUomsToConvert")]
        public ParamMasterList GetUomsToConvert(int materialID)
        {
            return db.GetUomsToConvert(materialID);
        }

        [HttpGet]
        [Route("GetMaterialUomDetails")]
        public MaterialUomConvert getMaterialUomDetails(int materialID)
        {
            return db.getMaterialUomDetails(materialID);
        }

        [HttpPost]
        [Route("AddMaterialConvertData")]
        public string AddMaterialConvertData(ConvertionData obj)
        {
            return db.AddMaterialConvertData(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("ChangeUomConvertionStatus")]
        public string ChangeUomConvertionStatus(int convertedUomID)
        {
            return db.ChangeUomConvertionStatus(convertedUomID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetMaterialUomList")]
        public MaterialUomList GetMaterialUomList(int materialID)
        {
            return db.GetMaterialUomList(materialID, true);
        }

        [HttpGet]
        [Route("GetAllMaterialCategories")]
        public MaterialCatList GetAllMaterialCategories()
        {
            return db.GetAllMaterialCategories();
        }

        [HttpGet]
        [Route("GetMaterialDetailsByID")]
        public MaterialUomDetails GetMaterialDetailsByID(int matID)
        {
            return db.GetMaterialDetailsByID(matID);
        }

        [Route("GetEntityDetails")]
        public GetEntityDetailsList GetEntityDetails()
        {
            return db.GetEntityDetails();
        }

        [Route("GetMaterialUOMConvertions")]
        public UOMCodeList GetMaterialUOMConvertions(string baseUOM)
        {
            return db.GetMaterialUOMConvertions(baseUOM);
        }

        [HttpPost]
        [Route("GetUOMConvertionDenomination")]
        public string GetUOMConvertionDenomination(GetUOMDenomination obj)
        {
            return db.GetUOMConvertionDenomination(obj);
        }

        [HttpGet]
        [Route("GetChemicalBatchDetailsByPackInvID")]
        public ChemicalBatch GetChemicalBatchDetailsByPackInvID(int packInvID, int refPackID)
        {
            return db.GetChemicalBatchDetailsByPackInvID(packInvID, refPackID);
        }


        [HttpGet]
        [Route("GetExportColumns")]
        public List<Columns> GetExportColumns(string entityCode)
        {
            return db.GetExportColumns(entityCode);
        }

        [HttpPost]
        [Route("ExportData")]
        public DataTable ExportData(ExportBO obj)
        {
            obj.PlantID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID;
            return db.ExportData(obj);
        }

        [HttpPost]
        [Route("ManageRs232Integration")]
        public GetRs232IntegrationBO ManageRs232Integration(ManageRs232IntegrationBO obj)
        {
            var objInte = new GetRs232IntegrationBO();

            objInte = db.ManageRs232Integration(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            if (objInte.ReturnFlag == "OK" && obj.RS232Mode == "RS232")
            {
                var retVal = string.Empty;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["EARDSAPI_URL"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage resp = client.PostAsJsonAsync("SaveRS232IntegrationDetails", objInte).Result;

                if (resp.IsSuccessStatusCode)
                {
                    obj.RSPostFlag = resp.Content.ReadAsAsync<string>().Result;
                    obj.EncEntityActID = objInte.RSIntegrationID.ToString();
                    objInte = db.ManageRs232Integration(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
                }
            }

            return objInte;
        }

        [HttpPost]
        [Route("GetRs232Integration")]
        public GetRs232IntegrationBO GetRs232Integration(ManageRs232IntegrationBO obj)
        {
            return db.GetRs232Integration(obj);
        }

        [HttpGet]
        [Route("GetSysConfigurationData")]
        public List<GetSysConfigurationData> GetSysConfigurationData()
        {
            return db.GetSysConfigurationData();
        }

        [HttpPost]
        [Route("UpdateSysConfiguration")]
        public string UpdateSysConfiguration(UpdateSysConfiguration obj)
        {
            return db.UpdateSysConfiguration(obj);
        }

        [HttpGet]
        [Route("GetSpecHeaderInfo")]
        public GetSpecHeaderInfo GetSpecHeaderInfo(string encSpecID, string encCalibID)
        {
            return db.GetSpecHeaderInfo(CommonStaticMethods.Decrypt<int>(encSpecID), CommonStaticMethods.Decrypt<int>(encCalibID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("CAPAGetCAPAActionsBySourceRefType")]
        public List<CAPAGetCAPAActionsBySourceRefType> CAPAGetCAPAActionsBySourceRefType(CAPAActionsBySourceRefType obj)
        {
            return db.CAPAGetCAPAActionsBySourceRefType(obj);
        }

        [HttpPost]
        [Route("CAPAInsertUpdateCAPA")]
        public CAPAInsertUpdateCAPAResult CAPAInsertUpdateCAPA(CAPAInsertUpdateCAPA obj)
        {
            return db.CAPAInsertUpdateCAPA(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageRS232RequestMode")]
        public string ManageRS232RequestMode(ManageRS232RequestModeBO obj)
        {
            return db.ManageRS232RequestMode(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("EQPUpdateToDateTime")]
        public string EQPUpdateToDateTime(string encOccupancyID)
        {
            return db.EQPUpdateToDateTime(CommonStaticMethods.Decrypt<int>(encOccupancyID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageRs232IntegrationOther")]
        public string ManageRs232IntegrationOther(ManageRS232IntegrationOtherBO obj)
        {
            return db.ManageRs232IntegrationOther(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetRs232IntegrationOther")]
        public GetRS232IntegrationOtherBOList GetRs232IntegrationOther(ManageRS232IntegrationOtherBO obj)
        {
            return db.GetRs232IntegrationOther(obj);
        }

        [HttpGet]
        [Route("GetReportsInfoForSyncToDMS")]
        public string GetReportsInfoForSyncToDMS(string entityType)
        {
            string returnFlag = "SUCCESS";
            UploadReportList lst = new UploadReportList();
            lst = db.GetReportsInfoForSyncToDMS(entityType);

            if (lst != null && lst.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();

                dmsObj.List = lst;
                string retCode = FileUploadUtility.SynReportInfoToDMS(dmsObj);
                if (retCode != "OK")
                    returnFlag = retCode;
            }

            return returnFlag;
        }

        [HttpPost]
        [Route("ResetRs232EqpOtherOcc")]
        public string ResetRs232EqpOtherOcc(ResetRS232IntegrationBO obj)
        {
            return db.ResetRs232EqpOtherOcc(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageRS232OtherFieldsValues")]
        public string ManageRS232OtherFieldsValues(RS232OtherFieldsBOList list)
        {
            string xmlString = string.Empty;
            xmlString = CommonStaticMethods.Serialize<RS232OtherFieldsBOList>(list);

            return db.ManageRS232OtherFieldsValues(xmlString, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("DeleteChecklist")]
        public string DeleteChecklist(ManageChecklist obj)
        {
            return db.DeleteChecklist(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }
    }
}
