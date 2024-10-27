using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Invalidations;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.VolumetricSolution
{
    public class VolumetricSolIndexData
    {
        public GetVolumetricSolIndexList List { get; set; }

        public string ResultFlag { get; set; }

        public int? TotalRecords { get; set; }
    }


    public class GetVolumetricSolIndex
    {
        public short IndexID { get; set; }

        public string EncIndexID { get { return CommonStaticMethods.Encrypt(IndexID.ToString()); } }

        public string MatName { get; set; }

        public string MatCode { get; set; }

        public string PSMatName { get; set; }

        public string PSMatCode { get; set; }

        public decimal MolecularWeight { get; set; }

        public string FormulaType { get; set; }

        public int MaterialID { get; set; }

        public int? PSMaterialID { get; set; }

        public string Status { get; set; }

        public bool IsActive { get; set; }

        public string ChangeStatusComment { get; set; }

        public string Uom { get; set; }

        public string PreparationProcedure { get; set; }

        public string StpRefNumber { get; set; }

    }

    public class GetVolumetricSolIndexList : List<GetVolumetricSolIndex> { }

    public class VolumetricSolIndexFilter
    {
        public string Type { get; set; }

        public short IndexID { get { if (!string.IsNullOrEmpty(EncIndexID)) return CommonStaticMethods.Decrypt<short>(EncIndexID); else return default(short); } }

        public string EncIndexID { get; set; }

        public int MaterialID { get; set; }

        public int PSMaterialID { get; set; }

        public decimal MolecularWeight { get; set; }

        public string FormulaType { get; set; }

        public string Comments { get; set; }

        public string StpRefNumber { get; set; }

        public string EntityCode { get; set; }

        public string Status { get; set; }
    }

    public class AddSolution
    {
        public int MaterialID { get; set; }

        public int SolutionID { get { return CommonStaticMethods.Decrypt<int>(EncSolutionID); } }

        public string EncSolutionID { get; set; }

        public int FormulaID { get; set; }

        public string InitTime { get; set; }

        public string BrefDesc { get; set; }

        public decimal PreparationVolume { get; set; }

        public string Type { get; set; }

        public int ValidityPeriodID { get; set; }

        public int RestandardizationPeriodID { get; set; }

        public string PsDryingTem { get; set; }

        public string CoolingDuration { get; set; }

        public string DryingDuration { get; set; }
    }

    public class GetVolumetricSol
    {
        public short IndexID { get; set; }

        public string SolutionName { get; set; }

        public string SolutionCode { get; set; }

        public decimal? MolecularWeight { get; set; }

        public string FormulaType { get; set; }

        public string BriefDescription { get; set; }

        public string SolutionRefCode { get; set; }

        public string Status { get; set; }

        public string EncStandardizationID { get { if (StandardizationID > default(int)) return CommonStaticMethods.Encrypt(StandardizationID.ToString()); else return string.Empty; } }

        public int? StandardizationID { get; set; }

        public bool IsFinalPrep { get; set; }

        public string UOM { get; set; }

        public short? FormulaID { get; set; }

        public string FormulaTitle { get; set; }

        public SolventsList List { get; set; }

        public RecordActionDetails Act { get; set; }

        public GetStandardizationList StandList { get; set; }

        public int? UomID { get; set; }

        public decimal? PreparationVolume { get; set; }

        public string Comment { get; set; }

        public DateTime? DiscardedOn { get; set; }

        public string DiscardedBy { get; set; }

        public int MaterialID { get; set; }

        public int? ValidityPeriodID { get; set; }

        public int? ReStandardizationPeriodID { get; set; }

        public string ValidityTitle { get; set; }

        public string RestdPeriodTitle { get; set; }

        public string DryingDuration { get; set; }

        public string CoolingDuration { get; set; }

        public string PrimaryStandardDryingTern { get; set; }

        public string MaterialName { get {  return string.Format("{0} ({1})", SolutionName, SolutionCode); } }

        public bool IsFromAnalystQualification { get; set; }

        public bool CanAccess { get; set; }
    }

    public class GetStandardization
    {
        public int StandardizationID { get; set; }

        public string EncStandardizationID { get { return CommonStaticMethods.Encrypt(StandardizationID.ToString()); } }

        public DateTime? StandardStartDate { get; set; }

        public string StdAvg { get; set; }

        public string StdRSD { get; set; }

        public string StatusCode { get; set; }

        public string Status { get; set; }

        public bool ISFinal { get; set; }

        public string StdType { get; set; }

        public string Type { get; set; }

    }

    public class GetStandardizationList : List<GetStandardization> { }


    public class GetStandardizationInfo
    {
        public string PSDryingTern { get; set; }

        public decimal? FinalVolume { get; set; }

        public string StdAvg { get; set; }

        public string StdRSD { get; set; }

        public decimal? MolecularWeight { get; set; }

        public string Remarks { get; set; }

        public string StandardizationProcedure { get; set; }

        public string CoolingDuration { get; set; }

        public string DryingDuration { get; set; }

        public string PreparationProcedure { get; set; }

        public string BlankValue { get; set; }

        public SolventsList List { get; set; }

        public StandardizationDetailsList StdList { get; set; }

        public RecordActionDetails Act { get; set; }

        public VolumetricSolFormulasBOList FormulaList { get; set; }

        public string FormulaDef { get; set; }

        public byte MaxAppLevel { get; set; }

        public ReviewInvalidationsBO InvReviewDetails { get; set; }

    }

    public class StandardizationDetails
    {
        public int DetailID { get; set; }

        public string EncDetailID { get { return CommonStaticMethods.Encrypt(DetailID.ToString()); } }

        public string PSWeight { get; set; }

        public string SolutionConsumed { get; set; }

        public string Result { get; set; }

        public decimal? MolecularWeight { get; set; }

        public string StdType { get; set; }

        public string PassOrFail { get; set; }
    }
    public class StandardizationDetailsList : List<StandardizationDetails> { }

    public class ManageVolStdDetails
    {
        public string EncStandardID { get; set; }

        public int StandardID { get { return CommonStaticMethods.Decrypt<int>(EncStandardID); } }

        public string InitTime { get; set; }

        public decimal FinalVol { get; set; }

        public string PSDrying { get; set; }

        public VolStdDetailsList StdList { get; set; }

        public string StdXMLstring { get { if (StdList.Count > 0) return CommonStaticMethods.Serialize<VolStdDetailsList>(StdList); else return ""; } }

        public decimal Avg { get; set; }

        public string Remarks { get; set; }

        public string StdProcedure { get; set; }

        public string CoolingDuration { get; set; }

        public string DryingDuration { get; set; }

        public string BlankValue { get; set; }

        public string previousMolarity { get; set; }

        public string FormulaDef { get; set; }

        public List<VolumetricFormulaBO> FormulaList { get; set; }
    }

    public class VolumetricFormulaBO
    {
        public string InputCode { get; set; }

        public string Value { get; set; }
    }

    [XmlType("ITEM")]
    public class VolStdDetails
    {
        public int DetailID { get; set; }

        public string PSWeight { get; set; }

        public string VolConsumed { get; set; }

        public string Result { get; set; }
    }

    [XmlType("RT")]
    public class VolStdDetailsList : List<VolStdDetails> { }

    public class SearchVolumetricSOl
    {
        public int SolutionID { get; set; }

        public string EncSolutionID { get { return CommonStaticMethods.Encrypt(SolutionID.ToString()); } }

        public string SolutionCode { get; set; }

        public string Status { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SolutionName { get; set; }

        public int? StandardizationID { get; set; }

        public bool ISZeroth { get; set; }

        public string FormulaType { get; set; }

        public bool ISFinalApp { get; set; }

        public string BatchNumber { get; set; }

        public string FinalStatusCode { get; set; }

        public string StatusCode { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? UseBeforeDate { get; set; }

        public DateTime? ReStandardizationDate { get; set; }

        public bool IsAccessRest { get; set; }

        public bool IsRestd { get; set; }
    }

    public class SearchVolumetricSolFilter : PagerBO
    {
        public int StatusID { get; set; }

        public int MaterialID { get; set; }

        public string FormulaType { get; set; }

        public int BatchNumberID { get; set; }

        public DateTime ValidityFrom { get; set; }

        public DateTime ValidityTo { get; set; }

        public int SolutionIDFrom { get; set; }

        public int SolutionIDTo { get; set; }

        public DateTime InitiatedOn { get; set; }

        public int InitiatedBy { get; set; }

        public int SolutionID { get; set; }
    }

    public class ReStandardization
    {
        public string EncSolutionID { get; set; }

        public string StdType { get; set; }

        public int SolutionID { get { return CommonStaticMethods.Decrypt<int>(EncSolutionID); } }

        public string InitTime { get; set; }
    }

    public class ProcedureUpdate
    {
        public string EncIndexID { get; set; }

        public short IndexID { get { return CommonStaticMethods.Decrypt<short>(EncIndexID); } }

        public string PreparationProcedure { get; set; }

        public string StandardizationProcedure { get; set; }

        public string Type { get; set; }
    }

    public class GetSolutionFormulae
    {
        public short FormulaID { get; set; }

        public string FormulaTitle { get; set; }

        public Boolean IsSelect { get; set; }

        public int IndexFormulaID { get; set; }

        public decimal ? StrengthRangeFrom { get; set; }

        public decimal ? StrengthRangeTo { get; set; }
    }

    public class ManageSolutionFormula
    {

        public string EncIndexID { get; set; }

        public short IndexID { get { return CommonStaticMethods.Decrypt<short>(EncIndexID); } }

        public int formulaID { get; set; }

        public decimal StrengthRangeFrom { get; set; }

        public decimal StrengthRangeTo { get; set; }

        public string Type { get; set; }

        public int FormulaIndexID { get; set; }

    }

    public class GetSolutionFormulasByIndexID
    {
        public short FormulaID { get; set; }

        public string FormulaTitle { get; set; }
    }

    public class VolumetricSolFormulasBO
    {
        public string InputCode { get; set; }
    }
    public class VolumetricSolFormulasBOList : List<VolumetricSolFormulasBO> { }

    public class VolSolutionIndex
    {
        public string CreatedBy { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string Comment { get; set; }

        public string PreparationProcedure { get; set; }

        public string StandardizationProcedure { get; set; }

    }

    public class SolMgmtFormulae
    {
        public string FormulaTitle { get; set; }

        public decimal StrengthRangeFrom { get; set; }

        public decimal StrengthRangeTo { get; set; }
    }

    public class GetVOLViewSolIndexDetailsByIDResp
    {
        public VolSolutionIndex VolSolIndex { get; set; }

        public List<SolMgmtFormulae> SolMgmtFormulaeList { get; set; }
    }

}
