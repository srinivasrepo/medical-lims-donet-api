using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using System;

namespace MedicalLIMSApi.Core.Entities.Indicators
{

    public class ManageIndicatorsInfo
    {
        public string EncIndicatorID { get; set; }

        public int IndicatorID { get { return CommonStaticMethods.Decrypt<int>(EncIndicatorID); } }

        public int IndicatorType { get; set; }

        public int IndicatorSol { get; set; }

        public string BriefDescription { get; set; }

        public decimal VolumePrepared { get; set; }

        public string Type { get; set; }

        public DateTime UseBefore { get; set; }

        public string InitTime { get; set; }

        public SIngleBOList UploadedID { get; set; }

        public string XMLuploadedID { get { if (UploadedID.Count > 0) return CommonStaticMethods.Serialize<SIngleBOList>(UploadedID); else return string.Empty; } }

        public string OtherInfo { get; set; }

        public int ValidatePeriodID { get; set; }

        public string EntityCode { get; set; }

        public string Role { get; set; }

        public decimal SolutionPH { get; set; }

        public string Weight { get; set; }

        public string DryingTemp { get; set; }

        public string DryingDuration { get; set; }

        public string CoolingDuration { get; set; }
    }

    public class GetIndicatorsInfo
    {
        public int IndicatorType { get; set; }

        public int SolutionID { get; set; }

        public string Name { get; set; }

        public string UOM { get; set; }

        public decimal? VolumePrepared { get; set; }

        public DateTime ? UseBefore { get; set; }

        public string BreafDescription { get; set; }

        public string IndicatorCode { get; set; }

        public string Status { get; set; }

        public SolventsList SolventsList { get; set; }

        public RecordActionDetails Tran { get; set; }

        public string OtherInfo { get; set; }

        public int ? ValidatePeriodID { get; set; }

        public DateTime? ActionDate { get; set; }

        public string MaterialCode { get; set; }

        public decimal? SolutionPH { get; set; }

        public string weight { get; set; }

        public string DryingTemp { get; set; }

        public string DryingDuration { get; set; }

        public string CoolingDuration { get; set; }

        public string IndicatorTypeCode { get; set; }

        public string IndicatorTypeName { get; set; }

    }

    public class SearchIndicatorsBO : PagerBO
    {
        public int StatusID { get; set; }

        public int IndicatorType { get; set; }

        public int IndicatorID { get; set; }

        public int SolutionID { get; set; }

        public short PlantID { get; set; }

        public int BatchNumberID { get; set; }

        public DateTime ValidityFrom { get; set; }

        public DateTime ValidityTo { get; set; }

        public int IndicatorIDTo { get; set; }

        public int InitiatedBy { get; set; }

        public DateTime InitiatedOn { get; set; }

        public int IndicatorCodeID { get; set; }
    }

    public class SearchIndicatorData
    {
        public int IndicatorID { get; set; }

        public string EncIndicatorID { get { return CommonStaticMethods.Encrypt(IndicatorID.ToString()); } }

        public string IndicatorCode { get; set; }

        public string Status { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Uom { get; set; }

        public string ReqType { get; set; }

        public string SolutionName { get; set; }

        public decimal? VolumePrepared { get; set; }

        public string VolPrep { get { if (VolumePrepared > default(decimal)) return string.Format("{0} {1}", CommonStaticMethods.ConvertToFriendlyDecimal(VolumePrepared), Uom); else return string.Empty; } }

        public DateTime ? UseBefore { get; set; }

        public string BatchNumber { get; set; }

        public string FinalStatusCode { get; set; }

        public string StatusCode { get; set; }
    }

    public class ViewIndicatorInfo
    {
        public string IndicatorType { get; set; }

        public string SolutionName { get; set; }

        public decimal? VolumePrepared { get; set; }

        public string UOM { get; set; }

        public DateTime ? UseBefore { get; set; }

        public string BreafDescription { get; set; }

        public SolventsList SolventList { get; set; }

        public string IndicatorCode { get; set; }

        public string Status { get; set; }

        public string Comment { get; set; }

        public DateTime ? DiscardedOn { get; set; }

        public string DiscardedBy { get; set; }

        public string OtherInfo { get; set; }

        public string ValidityPeriod { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialName { get { return string.Format("{0} ({1})", SolutionName, MaterialCode); } }

        public decimal? SolutionPH { get; set; }

        public string Weight { get; set; }

        public string DryingTemp { get; set; }

        public string DryingDuration { get; set; }

        public string CoolingDuration { get; set; }
    }

    public class GetINDMasterData
    {
        public string ReturnFlag { get; set; }

        public string PreparationText { get; set; }

        public int? PreparationMasterID { get; set; }
    }

    public class ManageMasterData
    {
        public string Type { get; set; }

        public int PreparationTypeID { get; set; }

        public int SolutionID { get; set; }

        public string Description { get; set; }

        public int PreparationMasterID { get; set; }

        public string EntityCode{ get; set; }

        public string EncEntActID { get; set; }

        public int EntActID { get { return CommonStaticMethods.Decrypt<int>(EncEntActID); } }
    }

}
