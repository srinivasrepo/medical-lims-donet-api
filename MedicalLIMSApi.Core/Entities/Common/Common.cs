using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Login;
using MedicalLIMSApi.Core.Entities.SampleAnalysis;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.Common
{
    [XmlType("ITEM")]
    public class DeptModules
    {
        public int DeptID { get; set; }

        public string DeptName { get; set; }

        public bool IsAssigned { get; set; }

        public byte DeptType { get; set; }
    }
    [XmlType("RT")]
    public class DeptModuleList : List<DeptModules> { }

    public class RoleMasterSmall
    {
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public int DeptID { get; set; }

    }

    public class RoleMasterSmallList : List<RoleMasterSmall> { }


    public class SearchResults<T>
    {
        public int TotalNumberOfRows { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<T> SearchList { get; set; }

    }

    public class RecordActionDetails
    {
        public string InitTime { get; set; }

        public bool CanApprove { get; set; }

        public int? DetailID { get; set; }

        public int TransKey { get; set; }

        public string ReturnFlag { get; set; }

        public string Status { get; set; }

        public string ReferenceNumber { get; set; }

        public string EncTranKey
        {
            get
            {
                string enc = string.Empty;

                if (TransKey > default(int))
                    enc = CommonStaticMethods.Encrypt(TransKey.ToString());

                return enc;
            }

        }

        public string ResultFlag { get; set; }

        public byte AppLevel { get; set; }

        public bool ISFinalApp { get; set; }

        public string OperationType { get; set; }

        public string ComponentCode { get; set; }

    }

    public class GetStatus
    {
        public int StatusID { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }
    }

    public class GetStatusList : List<GetStatus> { }

    [XmlType("ITEM")]
    public class SingleBO
    {
        public int ID { get; set; }
    }

    [XmlType("RT")]
    public class SIngleBOList : List<SingleBO> { }

    public class UserDetailsList
    {
        public int UserRoleID { get; set; }

        public string UserName { get; set; }

        public string DeptName { get; set; }

        public string RoleName { get; set; }

        public string UserCode { get; set; }
    }

    public class UserDetailsLists : List<UserDetailsList> { }


    public class PagerBO
    {
        public int PageIndex { get; set; }

        public int PageSize { get { return 10; } }

        public string AdvanceSearch { get; set; }
    }


    public class ViewHistory
    {
        public string ActionResult { get; set; }

        public DateTime ActionDate { get; set; }

        public string ActionBy { get; set; }

        public string DeptName { get; set; }

        public byte AppLevel { get; set; }

        public string ActionRemarks { get; set; }

        public bool FinalAppFlag { get; set; }

        public string RoleName { get; set; }

        public int ActionUserRoleID { get; set; }

        public string UserName { get; set; }

        public string ActionCode { get; set; }

    }

    public class ViewHistoryList : List<ViewHistory> { }

    public class GetViewHistory
    {
        public UserDetailsLists usrList { get; set; }

        public ViewHistoryList list { get; set; }
    }

    [XmlType("ITEM")]
    public class LookupGridData
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }

    [XmlType("RT")]
    public class LookupGridDataList : List<LookupGridData> { }

    public class PlantMaster
    {
        public short PlantID { get; set; }

        public string PlantName { get; set; }

        public bool HO_Flag { get; set; }
    }
    public class PlantMasterList : List<PlantMaster> { }

    [XmlType("ITEM")]
    public class DeptMaster
    {
        public int DeptID { get; set; }

        public string DeptName { get; set; }

        public byte? DeptType { get; set; }

        public string DeptCode { get; set; }

        public bool IsActive { get; set; }

        public string UserDeptCode { get; set; }

    }
    [XmlType("RT")]
    public class DeptMasterList : List<DeptMaster> { }

    public class GetDeptPlantList
    {
        public DeptMasterList DeptList { get; set; }

        public PlantMasterList PlantList { get; set; }

    }


    public class SingleUsrBasic
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public short Value { get; set; }

    }

    public class SingleUsrBasicList : List<SingleUsrBasic> { }


    public class ChartInfo
    {
        public string Label { get; set; }

        public int value { get; set; }
    }

    public class ChartInfoList : List<ChartInfo> { }


    public class MaterialCat
    {
        public int ParamID { get; set; }

        public string ParamName { get; set; }

        public string ParamCode { get; set; }
    }

    public class MaterialCatList : List<MaterialCat> { }

    public class MaterialSubCat
    {
        public int CAT_ITEMID { get; set; }

        public string CAT_ITEM { get; set; }

        public string CAT_ITEM_CODE { get; set; }
    }

    public class MaterialSubCatList : List<MaterialSubCat> { }

    public class ProductStages
    {
        public int STAGE_ID { get; set; }

        public string STAGE_NAME { get; set; }

        public int STAGE_MAT_ID { get; set; }
    }

    public class ProductStagesList : List<ProductStages> { }

    public class ManageOccupancy
    {
        public int EqpID { get; set; }

        public string OccupancyRequired { get; set; }

        public DateTime FromTime { get; set; }

        public DateTime ToTime { get; set; }

        public string OccupancyCode { get; set; }

        public int EqpOthOccID { get { return CommonStaticMethods.Decrypt<int>(EncEqpOthOccID); } }

        public string EncEqpOthOccID { get; set; }

        public int InvID { get; set; }

        public string BatchNumber { get; set; }

        public int UserID { get; set; }

        public int UserRoleID { get; set; }

        public short PlantID { get; set; }

        public string DeptCode { get; set; }

        public string Type { get; set; }

        public string OccSource { get; set; }

        public string OccSourceName { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public string EncEntityActID { get; set; }

        public string Remarks { get; set; }

        public string Comment { get; set; }

        public string OccupancyType { get; set; }

        public string EntityRefNumber { get; set; }

        public string ConditionCode { get; set; }

        public int RefEqpOccID { get; set; }

    }


    public class GetOccupancyDetailsBO
    {
        public int EqpOthOccID { get; set; }

        public string EncEqpOthOccID { get { return CommonStaticMethods.Encrypt(EqpOthOccID.ToString()); } }

        public string Title { get; set; }

        public string EqpUserCode { get; set; }

        public string EqpCategory { get; set; }

        public string EqpType { get; set; }

        public DateTime FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        public string Duration { get; set; }

        public int EqpID { get; set; }

        public string Remarks { get; set; }

        public string StatusCode { get; set; }

        public string Status { get; set; }

        public string OccupancyType { get; set; }

        public string OccType { get; set; }

        public int? RefEqpOccID { get; set; }

        public bool IsRefEqpOcc { get; set; }

        public string RefInvBatch { get; set; }
    }

    public class GetOccupancyDetailsBOList : List<GetOccupancyDetailsBO> { }

    public class GetOccupancyDetails
    {
        public string ReturnFlag { get; set; }

        public int? EqpOthOccID { get; set; }

        public string IsOccRequired { get; set; }

        public string Comment { get; set; }

        public GetOccupancyDetailsBOList OccuList { get; set; }
    }

    public class MaterialDetails
    {
        public int MAT_ID { get; set; }

        public int CATEGORY { get; set; }

        public int SUBCATID { get; set; }
    }

    [XmlType("ITEM")]
    public class ChecklistBO
    {
        public int? ChkAnswerID { get; set; }

        public int QuestionID { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string Remarks { get; set; }

    }

    [XmlType("RT")]
    public class Checklist : List<ChecklistBO> { }

    public class ManageChecklist
    {
        public Checklist List { get; set; }

        public string ChklistXml { get { return CommonStaticMethods.Serialize<Checklist>(List); } }

        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public string EntitySourceCode { get; set; }

        public string EntityCode { get; set; }

        public string Remarks { get; set; }
    }

    public class ParamMaster
    {
        public int ParamKey { get; set; }

        public string ParamValue { get; set; }

        public string ParamAlies { get; set; }
    }
    public class ParamMasterList : List<ParamMaster> { }

    public class ParamFilter
    {
        public string ParamField { get; set; }

        public string ParamFType { get; set; }
    }

    public class AddMaterial
    {
        public int CategoryID { get; set; }

        public int CatItemID { get; set; }

        public string Material { get; set; }

        public string MaterialAlies { get; set; }

        public int MaterialType { get; set; }

        public int MaterialUom { get; set; }

        public string EntityCode { get; set; }
    }

    public class SolventPreparationQnty
    {
        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public string EntityCode { get; set; }

        public string InitTime { get; set; }

        public SolventQntyList SolList { get; set; }

        public string QntyListXML { get { if (SolList.Count > 0) return CommonStaticMethods.Serialize<SolventQntyList>(SolList); else return string.Empty; } }

        public string SourceType { get; set; }

        public string ChemicalConsumeComments { get; set; }

        public int ChemicalConsumeRefArID { get; set; }

        public int ChemicalConsumeRefArdsExecID { get; set; }
    }

    [XmlType("ITEM")]
    public class SolventQnty
    {
        public int PreparationID { get; set; }

        public string ParamAlies { get; set; }

        public decimal QuantityPreparation { get; set; }

        public string PreparationQuantityString { get; set; }

        public bool? IsPrimaryPreparationBatch { get; set; }
    }

    [XmlType("RT")]
    public class SolventQntyList : List<SolventQnty> { }

    [XmlType("ITEM")]
    public class CategoryCodes
    {
        public string Code { get; set; }

    }
    [XmlType("RT")]
    public class CategoryCodesList : List<CategoryCodes> { }

    public class GetCategoryList
    {
        public CategoryCodesList List { get; set; }

        public string Type { get; set; }

        public string CodeXml { get { return CommonStaticMethods.Serialize<CategoryCodesList>(List); } }
    }

    public class CommentsBO
    {
        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public string PurposeCode { get; set; }

        public string Comment { get; set; }

        public string EntityCode { get; set; }
    }

    public class UomConvertedDetails
    {
        public int ConvertedUomID { get; set; }

        public string MaterialUom { get; set; }

        public string ConvertedUom { get; set; }

        public decimal Value { get; set; }

        public bool IsActive { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }

    public class UomConvertedList : List<UomConvertedDetails> { }

    public class MaterialUomConvert
    {
        public string MaterialName { get; set; }

        public string BaseUom { get; set; }

        public UomConvertedList List { get; set; }
    }

    public class ConvertionData
    {
        public int MaterialID { get; set; }

        public int TargetUom { get; set; }

        public decimal TargetValue { get; set; }

    }

    public class MaterialUom
    {
        public int UOM_ID { get; set; }

        public string UOM { get; set; }

        public string UOM_CODE { get; set; }

        public int MAT_ID { get; set; }
    }

    public class MaterialUomList : List<MaterialUom> { }

    public class MaterialUomDetails
    {
        public int MaterialID { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        public string SubCategory { get; set; }

        public string UomName { get; set; }
    }

    public class GetEntityDetails
    {
        public int EntityID { get; set; }

        public string Entity { get; set; }
    }

    public class GetEntityDetailsList : List<GetEntityDetails> { }

    public class UOMCode
    {
        public string SourceUOM { get; set; }

        public string TargetUOM { get; set; }

        public string UOM { get; set; }

        public int ParamKey { get; set; }
    }
    public class UOMCodeList : List<UOMCode> { }

    public class GetUOMDenomination
    {
        public string SourceUOM { get; set; }

        public string TargetUOM { get; set; }

        public int MaterialID { get; set; }
    }

    public class MatBO
    {
        public int MatID { get; set; }

        public string ChemicalName { get; set; }

        public string ChemicalCode { get; set; }

        public string MatFormatt { get { return string.Format("{0} ({1})", ChemicalName, Convert.ToString(ChemicalCode)); } }
    }

    public class ChemicalBatch
    {
        public DateTime? UseBeforeDate { get; set; }

        public string Manufacture { get; set; }

        public string StdAvg { get; set; }

        public bool? InHouse { get; set; }

        public int? MatID { get; set; }

        public string MatName { get; set; }

        public int SubCatID { get; set; }

        public string SubCatCode { get; set; }

        public string CategoryCode { get; set; }

        public string Density { get; set; }

        public string Purity { get; set; }

        public string PurityType { get; set; }

        public string PotPurAssy { get; set; }

    }


    public class Columns
    {
        public string ColumnName { get; set; }

        public string FriendlyColumnName { get; set; }

        public string ColumnType { get; set; }
    }

    public class ExportBO
    {
        public string Columns { get; set; }

        public string Condition { get; set; }

        public string EntityCode { get; set; }

        public bool PlantFilter { get; set; }

        public short PlantID { get; set; }
    }


    public class ManageRs232IntegrationBO
    {
        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public string SourceCode { get; set; }

        public int InstrumentID { get; set; }

        public string OccupancyCode { get; set; }

        public string RSPostFlag { get; set; }

        public string ReqCode { get; set; }

        public string SourceName { get; set; }

        public string ChemicalName { get; set; }

        public string BatchNumber { get; set; }

        public bool IsComingLabchamical { get; set; }

        public string ConditionCode { get; set; }

        public string RS232Mode { get; set; }

        public string OccSource { get; set; }

        public string ParentID { get; set; }

        public string SectionCode { get; set; }

        public int RefEqpOccID { get; set; }
    }

    public class GetRs232IntegrationBO
    {
        public int EntityActID { get; set; }

        public string ReturnFlag { get; set; }

        public int RSIntegrationID { get; set; }

        public string EntityName { get; set; }

        public string RefNumber { get; set; }

        public string Purpose { get; set; }

        public string MaterialName { get; set; }

        public string BatchNumber { get; set; }

        public string EquipmentUserCode { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string PostFlag { get; set; }

        public string Equipment { get; set; }

        public DateTime AnalysisDate { get; set; }

        public SDMSRSIntegrationBOList SDMSList { get; set; }

        public string EqpCode { get; set; }

        public int? OccupancyID { get; set; }

        public string EncOccupancyID { get { if (OccupancyID > default(int)) return CommonStaticMethods.Encrypt(OccupancyID.ToString()); else return string.Empty; } }

        public int? SpecTestID { get; set; }

        public string RS232Mode { get; set; }

        public string EncRSIntegrationID { get { return CommonStaticMethods.Encrypt(RSIntegrationID.ToString()); } }

        public string sUserCode { get; set; }
    }

    public class SDMSRSIntegrationBO
    {
        public string DataProcessed { get; set; }

        public DateTime DateReceived { get; set; }

        public int SdmsID { get; set; }
    }
    public class SDMSRSIntegrationBOList : List<SDMSRSIntegrationBO> { }

    public class GetSysConfigurationData
    {
        public short ConfigID { get; set; }

        public string EncConfigID { get { return CommonStaticMethods.Encrypt(ConfigID.ToString()); } }

        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

        public string Description { get; set; }

        public string DataType { get; set; }

        public string Options { get; set; }
    }

    public class UpdateSysConfiguration
    {
        public int ConfigID { get { return CommonStaticMethods.Decrypt<int>(EncConfigID); } }

        public string EncConfigID { get; set; }

        public string ConfigValue { get; set; }
    }

    public class GetSpecHeaderInfo
    {
        public string MatCode { get; set; }

        public string MatName { get; set; }

        public string SpecType { get; set; }

        public string Category { get; set; }

        public string SpecNumber { get; set; }

        public string SupersedNumber { get; set; }

        public DateTime? SupersedDate { get; set; }

        public decimal? CompositSampleQty { get; set; }

        public string CsUom { get; set; }

        public string CompositSampleQtyUom { get { return string.Format("{0} ({1})", CompositSampleQty, CsUom); } }

        public decimal? AnalysisSampleQty { get; set; }

        public string AsUom { get; set; }

        public string AnalysisSampleQtyUom { get { return string.Format("{0} ({1})", AnalysisSampleQty, AsUom); } }

        public string Specification { get; set; }

        public string StatusCode { get; set; }

        public string Status { get; set; }

        public string Stage { get; set; }

        public string ProdName { get; set; }

        public string ProdCode { get; set; }

        public DateTime? TargetReviewDate { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public string PharmReference { get; set; }

        public string SubCategory { get; set; }

        public string SampleType { get; set; }

        public string ViewRemarks { get; set; }

        public string ViewResult { get; set; }

        public string Title { get; set; }

        public string InstrUserCodes { get; set; }

        public string InstrumentType { get; set; }

        public string RequestCode { get; set; }

        public string ManualReferenceNumber { get; set; }

        public List<GetSpecificationData> GetSpecificationData { get; set; }

    }

    public class GetSpecificationData
    {
        public string SrNum { get; set; }

        public string TestName { get; set; }

        public string SpecDesc { get; set; }

        public string RowType { get; set; }

        public int SpecCatID { get; set; }

        public string TestType { get; set; }

        public decimal? LimitFrom { get; set; }

        public decimal? LimitTo { get; set; }

        public string UnitSymbol { get; set; }

        public bool IsGroupTest { get; set; }

        public string TestCode { get; set; }

        public string LimitType { get; set; }

        public string STPTitle { get; set; }

        public string LimitResult { get { if (LimitFrom != null && LimitTo != null) return string.Format("{0} {1} To {2} {3}", LimitFrom, CommonStaticMethods.GetSymbol(UnitSymbol), LimitTo, CommonStaticMethods.GetSymbol(UnitSymbol)); else return string.Empty; } }

        public string TestNameCode { get { return RowType == "TEST" ? string.Format("{0} ({1})", TestName, TestCode) : TestName; } }

        public int? SpecTestID { get; set; }

    }

    public class CAPAActionsBySourceRefType
    {
        public string EncSourceRefID { get; set; }

        public int SourceRefID { get { return CommonStaticMethods.Decrypt<int>(EncSourceRefID); } }

        public string SourceRefCode { get; set; }

        public string CapaType { get; set; }

        public string CapaModuleCode { get; set; }
    }

    public class CAPAGetCAPAActionsBySourceRefType
    {
        public int CapaID { get; set; }

        public string Capa { get; set; }

        public string CapaType { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDelete { get; set; }

        public bool IsUpdate { get; set; }

        public string CapaOwnerName { get; set; }

        public string ResponsibleRerson { get; set; }

        public int? CapaOwnerID { get; set; }

        public int? Type { get; set; }

        public string TypeText { get; set; }

        public int? AreaOfImplementation { get; set; }

        public string AreaOfImplementationText { get; set; }

        public int? ScopeOfCapa { get; set; }

        public string ScopeOfCapaText { get; set; }

        public string Status { get; set; }

        public DateTime? TargetDate { get; set; }
    }

    public class CAPAInsertUpdateCAPA
    {
        public int CapaID { get; set; }

        public string Capa { get; set; }

        public DateTime TargetDate { get; set; }

        public string CapaType { get; set; }

        public int CapaSourceID { get; set; }

        public string CapaSourceCode { get; set; }

        public string CapaSrcOthers { get; set; }

        public string EncSourceReferenceID { get; set; }

        public int SourceReferenceID { get { return CommonStaticMethods.Decrypt<int>(EncSourceReferenceID); } }

        public string SourceReference { get; set; }

        public string InitTime { get; set; }

        public string RetVal { get; set; }

        public string CapaOwner { get; set; }

        public int BulidID { get; set; }

        public string CapaNature { get; set; }

        public string QualityIssDesc { get; set; }

        public string DoctString { get; set; }

        public int Type { get; set; }

        public int AreaOfImplementation { get; set; }

        public int ScopeOfCapa { get; set; }

        public int CapaOwnerID { get; set; }

        public bool IsNewCapa { get; set; }

        public bool IsFromUC { get; set; }

        public string ModuleCode { get; set; }
    }

    public class CAPAInsertUpdateCAPAResult
    {
        public string InitTime { get; set; }

        public int RetVal { get; set; }
    }

    public class ManageRS232RequestModeBO
    {
        public string ConditionCode { get; set; }

        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public string Type { get; set; }

        public string ReqCode { get; set; }

        public string SectionCode { get; set; }
    }

    public class ManageRS232IntegrationOtherBO
    {
        public string EncRs232IntegrationID { get; set; }

        public int Rs232IntegrationID { get { return CommonStaticMethods.Decrypt<int>(EncRs232IntegrationID); } }

        public string EntityActID { get; set; }

        public int EntityActualID { get { return CommonStaticMethods.Decrypt<int>(EntityActID); } }

        public string ConditionCode { get; set; }

        public string ReqCode { get; set; }

        public string KeyTitle { get; set; }

        public string KeyActualValue { get; set; }

        public decimal KeyValue { get; set; }

    }

    public class GetRS232IntegrationOtherBO
    {
        public int IntegrationOtherID { get; set; }

        public string EncIntegrationOtherID { get { return CommonStaticMethods.Encrypt(IntegrationOtherID.ToString()); } }

        public string KeyTitle { get; set; }

        public decimal? KeyValue { get; set; }
    }
    public class GetRS232IntegrationOtherBOList : List<GetRS232IntegrationOtherBO> { }

    [XmlType("ITEM")]
    public class PlaceholderData
    {
        public int ValueID { get; set; }

        public string Placeholder { get; set; }

        public string PlaceholderValue { get; set; }

        public string ColumnName { get; set; }
    }

    [XmlType("RT")]
    public class PlaceholderList : List<PlaceholderData> { }

    public class CommonPlaceholderValues
    {
        public string CheckedBy { get; set; }

        public string CheckedOn { get; set; }

        public string AnalysedBy { get; set; }

        public string AnalysedOn { get; set; }

        public string SpecLimit { get; set; }

        public string MobilePhaseBatchNo { get; set; }

        public string ArdsNo { get; set; }

        public string ColumnID { get; set; }

        public string ReviewdBy { get; set; }

        public string ReviewdOn { get; set; }
    }

    public class SampleAnalysisPlaceholderValues : CommonPlaceholderValues
    {
        public string ProductName { get; set; }

        public string BatchNumber { get; set; }

        public string ProductStage { get; set; }

        public string ArNumber { get; set; }

        public string SampleType { get; set; }

        public string StpNumber { get; set; }

        public string PrimaryInstrument { get; set; }

        public int SpecTestID { get; set; }

        public string TestName { get; set; }

        public string OperataionNo { get; set; }

        public string SecondaryInstrument { get; set; }

        public string TestSolutionBatchNo { get; set; }

        public string VolSolutionBatchNo { get; set; }
    }

    public class CalibrationPlaceholderValues : CommonPlaceholderValues
    {
        public string InstrumentTitle { get; set; }

        public string InstrumentCode { get; set; }

        public string ManualReferenceNumber { get; set; }

        public string ParameterName { get; set; }

        public string CalibReferenceNumber { get; set; }

        public string InstrumentModel { get; set; }

        public string InstrumentMake { get; set; }

        public string CalibSolutionRefNo { get; set; }

        public string PrimaryInstrument { get; set; }

        public string SecondaryInstrument { get; set; }
    }

    public class GetARDSPlaceholderData
    {
        public PlaceholderList PlaceholderList { get; set; }

        public SampleAnalysisPlaceholderValues SampleAnalysisValues { get; set; }

        public CalibrationPlaceholderValues CalibrationValues { get; set; }
    }

    public class UploadReports
    {
        public string EntRptCode { get; set; }

        public string VersionCode { get; set; }

        public string ReportTitle { get; set; }

        public string ReferenceCode { get; set; }

        public string EntityCode { get; set; }

        public int EntityActID { get; set; }
    }

    public class UploadReportList : List<UploadReports> { }

    public class ReportUploadDMS
    {
        public string AppCode { get; set; }

        public string EntityCode { get; set; }

        public string Section { get { return "REPORT"; } }

        public int EntActID { get; set; }

        public string ReferenceNumber { get; set; }

        public string PlantCode { get; set; }

        public string DeptCode { get; set; }

        public string LoginID { get; set; }

        public string RoleName { get; set; }

        public UploadReportList List { get; set; }

        public int DmsID { get; set; }

        public byte[] Buffer { get; set; }

        public byte[] InvalidBuffer { get; set; }

        public string Type { get; set; }

        public string TypeCode { get; set; }

        public string WaterMark { get; set; }
    }

    public class RdlcUploadedBO
    {
        public string ReturnFlag { get; set; }

        public UploadedRdlcList Lst { get; set; }
    }

    [XmlType("ITEM")]
    public class UploadedRdlcBO
    {
        public int DMSID { get; set; }

        public string EntRptCode { get; set; }

        public string VersionCode { get; set; }

        public int EntActID { get; set; }
    }

    [XmlType("RT")]
    public class UploadedRdlcList : List<UploadedRdlcBO> { }

    public class ChecklistReportBo
    {
        public string ReturnFlag { get; set; }

        public UploadReportList Lst { get; set; }
    }

    public class ResetRS232IntegrationBO
    {
        public string EncRS232IntegrationID { get; set; }

        public int RS232IntegrationID { get { return CommonStaticMethods.Decrypt<int>(EncRS232IntegrationID); } }

        public string Remarks { get; set; }
    }

}