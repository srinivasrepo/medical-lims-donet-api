using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using System;

namespace MedicalLIMSApi.Core.Entities.StockSolutions
{
    public class StockManageStockSolutionsRequest
    {
        public string EncStockSolutionID { get; set; }

        public int StockSolutionID { get { return CommonStaticMethods.Decrypt<int>(EncStockSolutionID); } }

        public int SolutionIndexID { get; set; }

        public int TestID { get; set; }

        public int InstID { get; set; }

        public string DryingTemp { get; set; }

        public string DryingDuration { get; set; }

        public string CoolingDuration { get; set; }

        public SIngleBOList UploadedID { get; set; }

        public string XMLuploadedID { get { if (UploadedID.Count > 0) return CommonStaticMethods.Serialize<SIngleBOList>(UploadedID); else return string.Empty; } }

        public string Manual { get; set; }

        public int CalibrationReferenceID { get; set; }

    }

    public class STOCKStockSolutionsDetails
    {
        public int SolutionIndexID { get; set; }

        public string SolutionName { get; set; }

        public int? TestID { get; set; }

        public string TestTitle { get; set; }

        public int InstID { get; set; }

        public string Instrument { get; set; }

        public int? InstrumentTypeID { get; set; }

        public string InstrumentCode { get; set; }

        public string InstrumentType { get; set; }

        public string DryingTemp { get; set; }

        public string DryingDuration { get; set; }

        public string CoolingDuration { get; set; }

        public string Description { get; set; }

        public string OtherInfo { get; set; }

        public decimal? FinalVolume { get; set; }

        public DateTime? UseBefore { get; set; }

        public int? Validity { get; set; }

        public string ValidityTitle { get; set; }

        public string RequestCode { get; set; }

        public string Status { get; set; }

        public string InstrumentTypeCode { get; set; }

        public string Uom { get; set; }

        public bool IsPreparationUpd { get; set; }

        public DateTime? ActionDate { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialName { get { return string.Format("{0} ({1})", SolutionName, MaterialCode); } }

        public string BatchNumber { get; set; }

        public string Manual { get; set; }

        public int? CalibrationReferenceID { get; set; }

        public string CalibrationReference { get; set; }

        public int? CalibPramID { get; set; }
        
        public decimal? SolPH { get; set; }

        public decimal? Weight { get; set; }

    }

    public class GetSTOCKStockSolutionsDetails
    {
        public STOCKStockSolutionsDetails STOCKStockSolutionsDetails { get; set; }

        public RecordActionDetails RecordActions { get; set; }

        public SolventsList SolventsList { get; set; }
    }

    public class STOCKManageStockSolutionsPreparation
    {
        public string EncStockSolutionID { get; set; }

        public int StockSolutionID { get { return CommonStaticMethods.Decrypt<int>(EncStockSolutionID); } }

        public string Description { get; set; }

        public string OtherInfo { get; set; }

        public string InitTime { get; set; }

        public decimal SolPH { get; set; }

        public decimal Weight { get; set; }

    }

    public class STOCKManageStockSolutionsOutput
    {
        public string EncStockSolutionID { get; set; }

        public int StockSolutionID { get { return CommonStaticMethods.Decrypt<int>(EncStockSolutionID); } }

        public decimal FinalVolume { get; set; }

        public int Validity { get; set; }

        public DateTime UseBefore { get; set; }

        public string InitTime { get; set; }


    }

    public class STOCKSearchStockSolutions : PagerBO
    {
        public string EncStockSolutionID { get; set; }

        public int StockSolutionID { get { return CommonStaticMethods.Decrypt<int>(EncStockSolutionID); } }

        public int StatusID { get; set; }

        public int SolutionID { get; set; }

        public int StockSolutionIDFrom { get; set; }

        public int StockSolutionIDTo { get; set; }

        public int BatchNumberID { get; set; }

        public DateTime ValidityFrom { get; set; }

        public DateTime ValidityTo { get; set; }

        public int InstrumentType { get; set; }

        public int InstrumentID { get; set; }

        public int ParameterID { get; set; }

        public DateTime InitiatedOn { get; set; }

        public int InitiatedBy { get; set; }

        public int StockSolutionCode { get; set; }

    }

    public class GetSTOCKSearchStockSolutions
    {
        public int StockSolutionID { get; set; }

        public string EncStockSolutionID { get { return CommonStaticMethods.Encrypt(StockSolutionID.ToString()); } }

        public string RequestCode { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Uom { get; set; }

        public decimal? FinalVolume { get; set; }

        public string FinalVolumeUom { get { return FinalVolume > default(decimal) ? string.Format("{0} {1}", CommonStaticMethods.ConvertToFriendlyDecimal(FinalVolume), Uom) : string.Empty; } }

        public string SolutionName { get; set; }


        public DateTime? UseBefore { get; set; }

        public string BatchNumber { get; set; }

        public string FinalStatusCode { get; set; }

        public string InstrumentID { get; set; }
    }
}
