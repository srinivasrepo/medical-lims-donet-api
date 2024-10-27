using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using System;


namespace MedicalLIMSApi.Core.Entities.RinsingSolutions
{
    public class ManageRinsingSolutions
    {
        public string EncSolutionID{ get; set; }

        public int SolutionID { get { return CommonStaticMethods.Decrypt<int>(EncSolutionID); } }

        public int MaterialID { get; set; }

        public int UsageTypeID { get; set; }

        public string SolutionName { get; set; }

        public string StpNumber { get; set; }

        public string BriefDescription { get; set; }

        public string PreparationRemarks { get; set; }

        public string InitTime { get; set; }

        public SIngleBOList UploadedID { get; set; }

        public string UploadedIDXMl { get { if (UploadedID.Count > 0) return CommonStaticMethods.Serialize<SIngleBOList>(UploadedID); else return string.Empty; } }

        public string Type { get; set; }
    }

    public class GetRinsingSolutionsDetails
    {
        public string EncSolutionID{ get { return CommonStaticMethods.Encrypt(SolutionID.ToString()); } }

        public int SolutionID { get; set; }

        public string SolutionName { get; set; }

        public string StpNumber { get; set; }

        public string BriefDescription { get; set; }

        public string PreparationRemarks { get; set; }

        public decimal? FinalVolume { get; set; }

        public DateTime? UseBeforeDate { get; set; }

        public string OutputRemarks { get; set; }

        public int UsageTypeID { get; set; }

        public string UsageType { get; set; }

        public int? ValidityPeriodID { get; set; }

        public string ValidityPeriod { get; set; }

        public string Status { get; set; }

        public string RequestCode { get; set; }

        public int MaterialID { get; set; }

        public string Material { get; set; }

        public int MatCategoryID { get; set; }

        public string Uom { get; set; }

        public DateTime? ActionDate { get; set; }

        public string UsageTypeCode { get; set; }

        public SolventsList SolventsList { get; set; }

        public RecordActionDetails RecordActions { get; set; }
    }

    public class ManageRinsingSolutionsOutputDetails
    {
        public string EncSolutionID { get; set; }

        public int SolutionID { get { return CommonStaticMethods.Decrypt<int>(EncSolutionID); } }

        public string InitTime { get; set; }

        public decimal FinalVolume { get; set; }

        public int ValidityPeriodID { get; set; }

        public DateTime UseBeforeDate { get; set; }

        public string OutputRemarks { get; set; }
    }

    public class SearchRinsingSolutionsData : PagerBO
    {
        public int StatusID { get; set; }

        public int UsageType{ get; set; }

        public string NameOfTheSolution { get; set; }

        public int BatchNumberID { get; set; }

        public string StpNumber { get; set; }

        public int SolutionIDFrom { get; set; }

        public int SolutionIDTo { get; set; }

        public DateTime ValidityFrom { get; set; }

        public DateTime ValidityTo { get; set; }

        public DateTime InitiatedOn { get; set; }

        public int InitiatedBy { get; set; }

        public int SolutionID { get; set; }
    }

    public class GetRinsingSolutions
    {
        public int SolutionID{ get; set; }

        public string EncSolutionID { get { return CommonStaticMethods.Encrypt(SolutionID.ToString()); } }

        public string RequestCode { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SolutionName { get; set; }

        public decimal? FinalVolume { get; set; }

        public DateTime? UseBeforeDate { get; set; }

        public string BatchNumber { get; set; }

        public string FinalStatusCode { get; set; }

        public string UsageType { get; set; }

        public string Uom { get; set; }

        public string  StpNumber { get; set; }

        public string FinalVolumeUom { get { return FinalVolume > default(decimal) ? string.Format("{0} {1}", CommonStaticMethods.ConvertToFriendlyDecimal(FinalVolume), Uom) : string.Empty; } }

    }
}
