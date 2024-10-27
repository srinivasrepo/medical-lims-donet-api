using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace MedicalLIMSApi.Core.Entities.CalibrationArds
{
    public class CalibrationArdsHeader
    {
        public string EqpCategory { get; set; }

        public string EqpType { get; set; }

        public string EqpTitle { get; set; }

        public string EqpCode { get; set; }

        public string ScheduleType { get; set; }

        public DateTime SchDate { get; set; }

        public DateTime? SchStartDate { get; set; }

        public string Status { get; set; }

        public string PeriodType { get; set; }

        public bool HasDeviation { get; set; }

        public int EqpMinSchID { get; set; }

        public string EncEqpMinSchID { get { return CommonStaticMethods.Encrypt(EqpMinSchID.ToString()); } }

        public int? CalibPramID { get; set; }

        public string ArdsMode { get; set; }

        public DateTime? SchEndDate { get; set; }

        public string RequestCode { get; set; }

        public string RequestVersion { get; set; }

        public string Title { get; set; }

        public string SpecNumber { get { if (!string.IsNullOrEmpty(RequestCode)) return string.Format("{0}-{1}({2})", RequestCode, RequestVersion, Title); else return ""; } }

        public RecordActionDetails Act { get; set; }

        public bool SpecResetDeviation { get; set; }

        public string Analysis { get; set; }

        public bool HasPrimaryOccSubmitted { get; set; }

        public bool IsReviewApplicable { get; set; }

        public string DocStatus { get; set; }

        public string ConditionCode { get; set; }
    }

    public class SearchEquipmentMaintenance : PagerBO
    {
        public int Category { get; set; }

        public int Type { get; set; }

        public int EquipmentID { get; set; }

        public int SchType { get; set; }

        public int StatusID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo{ get; set; }

        public DateTime SchDate { get; set; }

        public int CalibParamID { get; set; }

        public int MaintanceRptID { get; set; }

        public int ExecutionMode { get; set; }

        public DateTime ExecutionOn { get; set; }

        public bool ShowDateCrossedRecords { get; set; }
    }

    public class GetEquipmentMaintenance
    {
        public int MaintRptID { get; set; }

        public string EncMainRptID { get { return CommonStaticMethods.Encrypt(MaintRptID.ToString()); } }

        public string Title { get; set; }

        public string EqpCode { get; set; }

        public string Category { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string ScheduleType { get; set; }

        public DateTime? MaintDate { get; set; }

        public string RptNumber { get; set; }

        public DateTime ScheduleDate { get; set; }

        public string AnalysisStatus { get; set; }

        public string Equipment { get { return string.Format("{0} ({1})", Title, Convert.ToString(EqpCode)); } }

        public bool HasCreateNewRequest { get; set; }

        public DateTime? CalibrationDoneOn { get; set; }
    }

    public class GetEquipmentCategories
    {
        public short CategoryID { get; set; }

        public string Category { get; set; }

        public string CategoryCode { get; set; }
    }

    public class EquipmentTypesByCategory
    {
        public int CategoryID { get; set; }

        public int SchTypeID { get; set; }
    }

    public class GetEquipmentTypesByCategory
    {
        public int CatItemID { get; set; }

        public string CatItem { get; set; }

        public string CatItemCode { get; set; }
    }

    public class GetScheduleTypesByDeptCode
    {
        public int SchTypeID { get; set; }

        public string SchType { get; set; }
    }

    public class RunCalibration
    {
        public string EncEntActID { get; set; }

        public int EntActID { get { return CommonStaticMethods.Decrypt<int>(EncEntActID); } }

        public string InitTime { get; set; }
    }

    public class ArdsBO
    {
        public int ArdsExecID { get; set; }

        public string EntityCode { get; set; }

        public int? ReportID { get; set; }

        public int? InvalidationID { get; set; }
    }

    public class ReportDetails
    {
        public int? ReportID { get; set; }

        public int EntActID { get; set; }

        public int? DocumentID { get; set; }

        public string CreatedUserCode { get; set; }

        public string EntityReportCode { get; set; }

        public int UserRoleID { get; set; }

        public string FileName { get; set; }

        public bool FinalFlag { get; set; }

        public string InputType { get; set; }

        public string AppCode { get { return "MEDICAL_LIMS"; } }

        public string PlantCode { get; set; }

        public string EntityCode { get; set; }

        public string DeptCode { get; set; }

        public string SectionCode { get; set; }

        public string RefNumber { get; set; }

        public int ? DMSId { get; set; }

        public string LoginID { get; set; }

        public GetARDSPlaceholderData PlaceHolders { get; set; }

        public PlaceholderList PlaceholderList { get; set; }

        public string InvalidationCode { get; set; }

        public string WaterMarker { get; set; }

        public ResultSetList ResultSets { get; set; }

        public PreparationList Preparations { get; set; }

        public TablePlaceholdersList TablePlaceholders { get; set; }

        public DataTable ImpurityData { get; set; }

        public ChemicalList ChemicalList { get; set; }

        public ChemicalList RefChemicalList { get; set; }

        public AsposeReportList TablePlaceholderLst { get; set; }
    }

    public class XmlPlaceHolder
    {
        public string name { get; set; }

        public string value { get; set; }
    }

    public class GetReportData
    {
        public int DMSReportID { get; set; }

        public string StatusCode { get; set; }

        public string Message { get; set; }
    }


    public class ResultSetBO
    {
        public int ResultSetID { get; set; }

        public string Title { get; set; }
    }

    public class ResultSetList : List<ResultSetBO> { }

    public class TablePlaceHolders
    {
        public int DetailID { get; set; }

        public string PlaceHolder { get; set; }
    }

    public class TablePlaceholdersList: List<TablePlaceHolders> { }

    public class PreparationBO
    {
        public int PreparationID { get; set; }

        public string PreparationName { get; set; }
    }

    public class PreparationList : List<PreparationBO> { }

    public class ChemicalBO
    {
        public long PreparationID { get; set; }

        public string ChemicalName { get; set; }

        public string BatchNumber { get; set; }

        public DateTime? Validity { get; set; }

        public string Purity { get; set; }

        public string Manufacturer { get; set; }

        public string Qty { get; set; }

        public string Normality { get; set; }
    }

    public class ChemicalList: List<ChemicalBO> { }

    public class AsposeReport
    {
        public string PlaceHolder { get; set; }
        
        public DataTable Table { get; set; }

    }
    public class AsposeReportList : List<AsposeReport> { }

    public class EQUPMAINInsertNewRequest
    {
        public int EqpMaintID { get; set; }

        public DateTime ScheduleDate { get; set; }
    }
}
