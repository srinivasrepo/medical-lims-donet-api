using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using System;
using System.Collections.Generic;
using System.Data;

namespace MedicalLIMSApi.Core.Interface.Common
{
    public interface ICommon
    {
        RoleMasterSmallList GetRolesDetails(byte deptID);

        List<LookupData> GetLookupData(LookupInfo obj);

        GetTodoListCountDetails GetUserToDoListCount(TransResults tran, string entityType);

        DataTable GetUserToDoListByCondition(TransResults tran);

        CatItemsMasterList GetCatItemsByCatCode(string catCode, string type);

        CategoryMasterList GetCategoryMasterList(string entityType);

        string InsertCategoryItems(CatItemsMaster obj, TransResults trn);

        SearchResults<CatItemsMaster> SearchCatItemsDetails(searchCat obj);

        string ChangeCatagoryItemStatus(int catagoryItemID, TransResults trn);

        MenuList GetMenuList(TransResults transResults, string entityType);

        GetStatusList GetStatusList(string entityCode);

        string EntityChangeStatus(int entityID, string entityCode, TransResults tr);

        GetViewHistory GetViewHistory(int entActID, string conditionCode);

        GetStatusList GetStatusListByCodition(string condition);

        GetDeptPlantList GetPlantDeptsList();

        DeptMasterList GetDeptMaster();

        PlantMasterList GetPlantMaster();

        MaterialCatList GetMaterialCategories();

        MaterialSubCatList GetMaterialSubCategories(string catCode);

        ProductStagesList GetProductStagesByProductID(int productID);

        MaterialDetails GetMaterialDetailsByMatID(int matID);

        Checklist GetChecklistItemsByCategory(int entActID, string categoryCode, string entityCode);

        ChecklistReportBo MangeChecklistAnswers(ManageChecklist obj, TransResults tr);

        AuditUnderRecordsList AuditUnderProcessRecords(TransResults trn);

        ParamMasterList GetParamMasterList(ParamFilter obj);

        string AddMaterial(AddMaterial obj, TransResults trn);

        Preparations ManageSolventQuantityPreparation(SolventPreparationQnty obj, TransResults trn);

        UOMList GetUomDetailsByMaterialID(int materialID);

        CatItemsMasterList GetCatItemsByCatCodeList(GetCategoryList obj);

        GetSamplePlanAnalystsDetailsList GetAllSamplePlanAssignedAnalysts(short plantID);

        string ManageDiscardCommnets(CommentsBO obj, TransResults tr);

        ValidityEntityList GetValidityEntities(string entityType, TransResults tr);

        string AddValidityPeriods(AddValidityPeriods obj, TransResults tr);

        DateTime GetCurrentDateTime();

        GetProductStageDetails GetProductStageDetailsByMaterialID(int materialID);

        ParamMasterList GetUomsToConvert(int materialID);

        MaterialUomConvert getMaterialUomDetails(int materialID);

        string AddMaterialConvertData(ConvertionData obj, TransResults tr);

        string ChangeUomConvertionStatus(int convertedUomID, TransResults tr);

        MaterialUomList GetMaterialUomList(int materialID, bool isActive);

        MaterialCatList GetAllMaterialCategories();

        MaterialUomDetails GetMaterialDetailsByID(int matID);

        GetEntityDetailsList GetEntityDetails();

        UOMCodeList GetMaterialUOMConvertions(string baseUOM);

        string GetUOMConvertionDenomination(GetUOMDenomination obj);

        ChemicalBatch GetChemicalBatchDetailsByPackInvID(int packInvID, int refpackID);


        //ViewSDMSDetailsList GetSDMSDetails();

        List<Columns> GetExportColumns(string entityCode);

        DataTable ExportData(ExportBO obj);

        GetRs232IntegrationBO ManageRs232Integration(ManageRs232IntegrationBO obj, TransResults trn);

        GetRs232IntegrationBO GetRs232Integration(ManageRs232IntegrationBO obj);

        List<GetSysConfigurationData> GetSysConfigurationData();

        string UpdateSysConfiguration(UpdateSysConfiguration obj);

        GetSpecHeaderInfo GetSpecHeaderInfo(int specID, int calibID, TransResults tr);

        List<CAPAGetCAPAActionsBySourceRefType> CAPAGetCAPAActionsBySourceRefType(CAPAActionsBySourceRefType obj);

        CAPAInsertUpdateCAPAResult CAPAInsertUpdateCAPA(CAPAInsertUpdateCAPA obj, TransResults tr);

        string ManageRS232RequestMode(ManageRS232RequestModeBO obj, TransResults trn);

        string EQPUpdateToDateTime(int occupancyID, TransResults trn);

        string ManageRs232IntegrationOther(ManageRS232IntegrationOtherBO obj, TransResults trn);

        GetRS232IntegrationOtherBOList GetRs232IntegrationOther(ManageRS232IntegrationOtherBO obj);

        UploadReportList GetReportsInfoForSyncToDMS(string entityType);

        string ResetRs232EqpOtherOcc(ResetRS232IntegrationBO obj, TransResults trn);

        string ManageRS232OtherFieldsValues(string xmlString, TransResults trn);

        string DeleteChecklist(ManageChecklist obj, TransResults tr);
    }
}
