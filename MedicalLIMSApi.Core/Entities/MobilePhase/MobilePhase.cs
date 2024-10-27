using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.MobilePhase
{
    public class MobilePhaseBO
    {
        public string EncMobilePhaseID { get; set; }

        public int MobilePhaseID { get { return CommonStaticMethods.Decrypt<int>(EncMobilePhaseID); } }

        //public int StageMatID { get; set; }

        public int SpecificationID { get; set; }

        public string Purpose { get; set; }

        public string InitTime { get; set; }

        //public int BulidID { get; set; }

        public string PreparationType { get; set; }

        public int SpecTest { get; set; }

        public int ParameterType { get; set; }

        public string Manual { get; set; }

        public string CalibrationReference { get; set; }

        public SIngleBOList FileUploadedIDs { get; set; }

        public string XMLstring { get { return CommonStaticMethods.Serialize<SIngleBOList>(FileUploadedIDs); } }

        public int MaterialID { get; set; }

        public int StageID { get; set; }

        public int MaintenanceReportID { get; set; }

        public string EntityCode { get; set; }

        public string Role { get; set; }

    }

    public class GetMobilePhasePreparation : GetProductStageDetails
    {
        public string EncMobilePhaseID { get { return CommonStaticMethods.Encrypt(MobilePhaseID.ToString()); } }

        public int MobilePhaseID { get; set; }

        public int? MaterialID { get; set; }

        public string Material { get; set; }

        public int? SpecificationID { get; set; }

        public string Purpose { get; set; }

        public string InitTime { get; set; }

        public string Specification { get; set; }

        public string SpecNumber { get; set; }

        public string SpecificationName { get { return string.Format("{0} ({1})", Specification, SpecNumber); } }

        public int? ProductID { get; set; }

        public string ProductName { get; set; }

        public string BufferPrep { get; set; }

        public string PhaseA { get; set; }

        public string PhaseB { get; set; }

        public string DiluentPrep { get; set; }

        public int? PHMeterCount { get; set; }

        public decimal? FinalVol { get; set; }

        public int? ValidityPeriod { get; set; }

        public DateTime? ValidityDate { get; set; }

        public decimal? solPH { get; set; }

        public string PreparationType { get; set; }

        //public string Stage { get; set; }

        public string ValidityPeriodValue { get; set; }

        public string OtherInfo { get; set; }

        public string MobilePhaseCode { get; set; }

        public string Status { get; set; }

        public string PreparationTypeCode { get; set; }

        public int? SpecTest { get; set; }

        public string SpecTestName { get; set; }

        public string Manual { get; set; }

        public string CalibrationReference { get; set; }

        public int? ParameterType { get; set; }

        public string ParameterName { get; set; }

        public string Comment { get; set; }

        public DateTime? DiscardedOn { get; set; }

        public string DiscardedBy { get; set; }

        public string PurposeCode { get; set; }

        public DateTime? UseBeforeDateTime { get; set; }

        public string OutputOtherInfo { get; set; }

        public string Uom { get; set; }

        public int? MatCategoryID { get; set; }

        public string MaterialCategory { get; set; }

        public string MaterialCode { get; set; }

        public string BatchNumber { get; set; }

        public int? MaintenanceReportID { get; set; }

        public int? CalibPramID { get; set; }

        public DateTime? ActionDate { get; set; }

        public int? SpecID { get; set; }

        public int ? PreparationTypeID { get; set; }

        public string PurposeName { get; set; }

        public int PurposeID { get; set; }

    }

    public class UOM
    {
        public int UomID { get; set; }

        public string Uom { get; set; }

        public string UomCode { get; set; }
    }

    public class UOMList : List<UOM> { }

    public class SolventPreparation
    {
        public int PreparationID { get; set; }

        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public int LabChemicalTypeID { get; set; }

        public int MaterialID { get; set; }

        public int BatchID { get; set; }

        public decimal QuantityPreparation { get; set; }

        public int UomID { get; set; }

        public string InitTime { get; set; }

        public string EntityCode { get; set; }

        public string SourceType { get; set; }

        public int PackInvID { get; set; }

        public int RefInvID { get; set; }

        public string RequestFrom { get; set; }

        public bool IsPrimaryPreparationBatch { get; set; }
    }

    public class Solvents
    {
        public int PreparationID { get; set; }

        public int? LabChemicalTypeID { get; set; }

        public int MaterialID { get; set; }

        public int? BatchID { get; set; }

        public string QuantityPreparation { get; set; }

        public int? UomID { get; set; }

        public string ChemicalType { get; set; }

        public string MaterialName { get; set; }

        public string BatchNumber { get; set; }

        public int? InvID { get; set; }

        public string Uom { get; set; }

        public string Quantity { get { return string.IsNullOrEmpty(QuantityPreparation) ? "" : QuantityPreparation + " " + Uom; } }

        public string Manufacturer { get; set; }

        public string Usebefore { get; set; }

        public string ParamAlies { get; set; }

        public string Grade { get; set; }

        public string EquipmentUserCodes { get; set; }

        public string MaterialCode { get; set; }

        public string BaseUOM { get; set; }

        public string BaseUomName { get; set; }

        public decimal? Denomination { get; set; }

        public int? PackInvID { get; set; }

        public string PackNo { get; set; }

        public string PackBatchNumber { get { return string.Format("{0} ({1})", BatchNumber, PackNo); } }

        public string EncPackInvID { get { if (PackInvID > default(int)) return CommonStaticMethods.Encrypt(PackInvID.ToString()); else return string.Empty; } }

        public DateTime? UseBeforeDate { get; set; }

        public string Manufacture { get; set; }

        public string StatusCode { get; set; }

        public string RequestFrom { get; set; }

        public string BatchPackNo { get; set; }

        public string PreparationSource { get; set; }

        public bool? IsPrimaryPreparationBatch { get; set; }

        public string StdAvg { get; set; }

        public int? InvalidationID { get; set; }

        public decimal? BalanceQty { get; set; }

        public string BalanceQuantity { get { if (BalanceQty > default(decimal)) return Convert.ToString(BalanceQty) + " " + BaseUomName; else return "0 " + BaseUomName; } }

        public decimal? ReservedQty { get; set; }

        public string ReservedQuantity { get { if (ReservedQty > default(decimal)) return Convert.ToString(ReservedQty) + " " + BaseUomName; else return ""; } }

        public string ActBalanceQty { get { if (ReservedQty > default(decimal)) return Convert.ToString(BalanceQty - Convert.ToDecimal(ReservedQty)) + " " + BaseUomName; else return BalanceQuantity; } }

        public bool IsRs232Added { get; set; }

        public string Purity { get; set; }

        public string Density { get; set; }

        public string PurityType { get; set; }

        public string PotPurAssy { get; set; }

        public int? RefPackID { get; set; }
    }

    public class SolventsList : List<Solvents> { }

    public class Preparations
    {
        public SolventsList List { get; set; }

        public RecordActionDetails Act { get; set; }
    }

    public class MobilePhaseData
    {
        public GetMobilePhasePreparation MobilePhase { get; set; }

        public SolventsList Solvents { get; set; }

        public TransResultApprovals AppTran { get; set; }
    }

    public class MobilePhasePrepComments
    {
        public string EncEntActID { get; set; }

        public int EntActID { get { return CommonStaticMethods.Decrypt<int>(EncEntActID); } }

        public string InitTime { get; set; }

        public string OtherInfo { get; set; }

        public MobilePhasePrepList List { get; set; }

        public string PreparationXML { get { return CommonStaticMethods.Serialize<MobilePhasePrepList>(List); } }

        public string JsonSerial { get { return CommonStaticMethods.ToJson<MobilePhasePrepList>(List); } }

    }

    [XmlType("ITEM")]
    public class RS232OtherFieldsBO
    {
        public int IntegrationOtherID { get; set; }

        public decimal KeyValue { get; set; }

        public string KeyActualValue { get; set; }
    }
    [XmlType("RT")]
    public class RS232OtherFieldsBOList : List<RS232OtherFieldsBO> { }


    [XmlType("ITEM")]
    public class MobilePhasePrep
    {
        public int PreparationID { get; set; }

        public string Preparation { get; set; }

        public decimal SolutionPH { get; set; }

        public string Weight { get; set; }
    }

    [XmlType("RT")]
    public class MobilePhasePrepList : List<MobilePhasePrep> { }

    public class MobilePhaseOutput
    {
        public decimal FinalVol { get; set; }

        public int ValidityPeriod { get; set; }

        public DateTime ValidityDate { get; set; }

        public string EncEntActID { get; set; }

        public int EntActID { get { return CommonStaticMethods.Decrypt<int>(EncEntActID); } }

        public string InitTime { get; set; }

        public GetPreparationDetailsList List { get; set; }

        public string XMLStringOutPut { get { if (List.Count > 0) return CommonStaticMethods.Serialize<GetPreparationDetailsList>(List); else return string.Empty; } }

        public string OtherInfo { get; set; }
    }


    public class SearchMPBO
    {
        public int PageIndex { get; set; }

        public byte PageSize { get; set; }

        public int ProductID { get; set; }

        public int StageID { get; set; }

        public int StatusID { get; set; }

        public int Purpose { get; set; }

        public short PlantID { get; set; }

        public string AdvanceSearch { get; set; }

        public int PreparationType { get; set; }

        public int BatchNumber { get; set; }

        public int SpecificationID { get; set; }

        public int SpecTestID { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public int MobilePhaseIDFrom { get; set; }

        public int MobilePhaseIDTo { get; set; }

        public int InitiatedBy { get; set; }

        public DateTime InitiatedOn { get; set; }

        public int MobilePhaseID { get; set; }

    }

    public class MPData
    {
        public int MobilePhaseID { get; set; }

        public string EncMobilePhaseID { get { return CommonStaticMethods.Encrypt(MobilePhaseID.ToString()); } }

        public string MobilePhaseCode { get; set; }

        public string ProductName { get; set; }

        public string Stage { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string BatchNumber { get; set; }

        public decimal? FinalVolume { get; set; }

        public string Uom { get; set; }

        public string FinalStatusCode { get; set; }

        public string SpecTest { get; set; }

        public DateTime? UseBefore { get; set; }

        public string InstrumentType { get; set; }

        public string MPFinalVolume { get { if (FinalVolume != null) return string.Format("{0} {1}", CommonStaticMethods.ConvertToFriendlyDecimal(FinalVolume), Uom); else return string.Empty; } }

    }

    public class MPList : List<MPData> { }

    public class SearchMobilePhaseData
    {
        public int TotalNumberOfRows { get; set; }

        public MPList List { get; set; }
    }

    public class viewMobilePhase
    {
        public GetMobilePhasePreparation MobilePhase { get; set; }

        public SolventsList Solvents { get; set; }

        public GetPreparationDetailsList Preparation { get; set; }
    }

    [XmlType("ITEM")]
    public class GetPreparationDetails
    {
        public int PreparationID { get; set; }

        public string PreparationName { get; set; }

        public string Preparation { get; set; }

        public decimal? PreparationSolPH { get; set; }

        public decimal? PrepartionVolum { get; set; }

        public int? ValidationPeriodID { get; set; }

        public DateTime? UseBeforeDate { get; set; }

        public string PreparationCode { get; set; }

        public int PhUsedCount { get; set; }

        public bool IsVisible { get; set; }

        public bool IsPreparationMandatory { get; set; }

        public bool IsCalculateVol { get; set; }

        public string ValidityPeriod { get; set; }

        public bool IsEnable { get; set; }

        public string Weight { get; set; }
    }

    [XmlType("RT")]
    public class GetPreparationDetailsList : List<GetPreparationDetails> { }

    public class ManageMobilePreparationOutput
    {
        public decimal? FinalVolume { get; set; }

        public DateTime? UseBeforeDateTime { get; set; }

        public TransResultApprovals Trn { get; set; }
    }



    public class ParameterType
    {
        public int ParameterID { get; set; }

        public string ParameterName { get; set; }
    }


    public class ParameterTypeList : List<ParameterType> { }

    [XmlType("ITEM")]
    public class PreparationData
    {
        public string PreparationText { get; set; }

        public int PreparationTextTypeID { get; set; }

        public string PreparationTextType { get; set; }

        public string PreparationCode { get; set; }
    }

    [XmlType("RT")]
    public class PreparationDataList : List<PreparationData> { }

    public class GetMasterData
    {
        public PreparationDataList Lst { get; set; }

        public string ReturnFlag { get; set; }
    }

    public class MasterData
    {
        public string Type { get; set; }

        public string PreparationType { get; set; }

        public string TypeCode { get; set; }

        public int MaterialID { get; set; }

        public int TestID { get; set; }

        public PreparationDataList Lst { get; set; }

        public string PreparationXml { get { return CommonStaticMethods.Serialize<PreparationDataList>(Lst); } }

        public int MobilePhaseID { get { return CommonStaticMethods.Decrypt<int>(EncMobilePhaseID); } }

        public string EncMobilePhaseID { get; set; }
    }

    public class DiscardPreparationBatch
    {
        public int PreparationID { get; set; }

        public bool CanInvalidate { get; set; }
    }

    public class GetActionAndRptData
    {
        public RecordActionDetails Act { get; set; }

        public UploadReportList RptList { get; set; }
    }
}
