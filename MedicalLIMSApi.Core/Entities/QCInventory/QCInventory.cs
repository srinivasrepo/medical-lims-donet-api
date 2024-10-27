using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.QCInventory
{
    public class SearchQCInventory : PagerBO
    {
        public int InvID { get; set; }

        public int MatID { get; set; }

        public int StatusID { get; set; }

        public int EntityID { get; set; }

        public DateTime BatchUseBeforeDate { get; set; }

        public DateTime UseBeforeDate { get; set; }

        public int ChemicalType { get; set; }

        public int CategoryID { get; set; }

        public int SubCategoryID { get; set; }

        public int ChemicalGrade { get; set; }

        public int BlockID { get; set; }

        public int ManufactureID { get; set; }

        public DateTime BatchExpDateTo { get; set; }

        public DateTime InwardDateFrom { get; set; }

        public DateTime InwardDateTo { get; set; }

        public bool ShowZeroQtyRecords { get; set; }

    }

    public class GetQCInventoryItems : MatBO
    {
        public int InvID { get; set; }

        public string EncInvID { get { return CommonStaticMethods.Encrypt(InvID.ToString()); } }

        public int? PackInvID { get; set; }

        public string RefNumber { get; set; }

        public string BatchNumber { get; set; }

        public string PackNo { get; set; }

        public decimal? PackReservedQty { get; set; }

        public decimal? BalanceQty { get; set; }

        public DateTime? UseBeforeDate { get; set; }

        public string Status { get; set; }

        public string Uom { get; set; }

        public string PackReservedQtyUom { get { if (PackReservedQty > default(decimal)) return string.Format("{0} {1}", CommonStaticMethods.ConvertToFriendlyDecimal(PackReservedQty), Uom); else return ""; } }

        public string BalQtyUom { get { return string.Format("{0} {1}", CommonStaticMethods.ConvertToFriendlyDecimal(BalanceQty), Uom); } }

        public bool IsDestroyed { get; set; }

        public string ChemicalNature { get; set; }

        public string ChemicalSource { get; set; }

    }

    public class OpenPack
    {
        public int PackInvID { get; set; }

        public int ValidityPeriodID { get; set; }

        public string StatusCode { get; set; }

        public string Remarks { get; set; }
    }

    public class ManageQCInventory
    {
        public int MatID { get; set; }

        public string BatchNumber { get; set; }

        public decimal Qty { get; set; }

        public string Retmsg { get; set; }

        public string EntityCode { get; set; }

        public int EntityActID { get; set; }

        public int StageID { get; set; }

        public int MfgID { get; set; }


    }

    public class InvtViewData
    {
        public string SubCategory { get; set; }

        public string ChemicalName { get; set; }

        public string ChemicalCode { get; set; }

        public string BatchNumber { get; set; }

        public decimal BatchQty { get; set; }

        public decimal BalanceQty { get; set; }

        public string Status { get; set; }

        public DateTime? MfgDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string RefNumber { get; set; }

        public string ChemicalSource { get; set; }

        public string Grade { get; set; }

        public string Block { get; set; }

        public DateTime? InvDate { get; set; }

        public string Uom { get; set; }

        public string BaseUom { get; set; }

        public int InvSourceID { get; set; }

        public string NatureofChemical { get; set; }

        public string EncInvSourceID { get { return CommonStaticMethods.Encrypt(InvSourceID.ToString()); } }

        public string ManufacturerName { get; set; }

        public bool? InHouse { get; set; }

        public string CategoryCode { get; set; }

        public string Purity { get; set; }

        public string Density { get; set; }

        public string SubCatCode { get; set; }
    }
    public class ViewInvtDetails
    {

        public InvtViewData InvViewData { get; set; }

        public ViewInvtsList ViewList { get; set; }
    }

    public class ViewInvtList
    {
        public int PackInvID { get; set; }

        public string PackNo { get; set; }

        public decimal PackQty { get; set; }

        public decimal BalanceQty { get; set; }

        public string Status { get; set; }

        public string PackOpenedBy { get; set; }

        public DateTime? PackOpenedOn { get; set; }

        public DateTime? UseBeforeDate { get; set; }

        public string Validity { get; set; }

        public string Uom { get; set; }

        public string PackQntyUOM { get { return string.Format("{0} {1}", CommonStaticMethods.ConvertToFriendlyDecimal(PackQty), Uom); } }

        public string BalanceQntyUOM { get { return string.Format("{0} {1}", CommonStaticMethods.ConvertToFriendlyDecimal(BalanceQty), Uom); } }

        public bool CanConsume { get; set; }

        public Boolean CanOpen { get; set; }

        public string StatusCode { get; set; }

        public Boolean IsDestroyed { get; set; }

        public string VersionCode { get; set; }
    }

    public class ViewInvtsList : List<ViewInvtList> { }

    [XmlType("Batch")]
    public class ChemicalDetials
    {
        public int InvID { get; set; }

        public string RefNumber { get; set; }

        public string ChemicalName { get; set; }

        public string ChemicalCode { get; set; }

        public string BatchNumber { get; set; }

        public decimal BatchQty { get; set; }

        public DateTime? MfgDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int? GradeID { get; set; }

        public int? BlockID { get; set; }

        public string Uom { get; set; }

        public string Chemical { get { return string.Format("{0} ({1})", ChemicalName, ChemicalCode); } }

        public string BatQty { get { return string.Format("{0} {1}", CommonStaticMethods.ConvertToFriendlyDecimal(BatchQty), Uom); } }

        public string MfgBatchNumber { get; set; }

        public int? ParentInvID { get; set; }

        public string Purity { get; set; }

        public string Density { get; set; }

        public string Grade { get; set; }

        public string GradeCode { get; set; }

        public string SubCatCode { get; set; }
    }

    [XmlType("Batches")]
    public class ChemicalList : List<ChemicalDetials> { }


    public class QCInventoryBatchDetails
    {
        public string ChemicalSource { get; set; }

        public string RefNumber { get; set; }

        public string Status { get; set; }

        public ChemicalList List { get; set; }

        public RecordActionDetails Act { get; set; }
    }

    public class PackDetails
    {
        public decimal BatchQty { get; set; }

        public string Uom { get; set; }

        public string NatureofChemical { get; set; }

        public PackList List { get; set; }
    }

    [XmlType("Pack")]
    public class Packs
    {
        public int PackInvID { get; set; }

        public string PackNo { get; set; }

        public decimal PackQty { get; set; }

        public decimal Qty { get; set; }
    }

    [XmlType("Packs")]
    public class PackList : List<Packs> { }

    public class ManagePacks
    {
        public int InvID { get; set; }

        public int NatureTypeID { get; set; }

        public PackList Lst { get; set; }

        public string PackXml { get { return CommonStaticMethods.Serialize<PackList>(Lst); } }
    }

    public class ManageBatches
    {
        public string encInvSourceID { get; set; }

        public string InitTime { get; set; }

        public int InvSourceID { get { return CommonStaticMethods.Decrypt<int>(encInvSourceID); } }

        public ChemicalList List { get; set; }

        public string BatchXml { get { return CommonStaticMethods.Serialize<ChemicalList>(List); } }
    }

    public class GetMiscConsumptionDetails
    {
        public string PackNo { get; set; }

        public decimal PackQty { get; set; }

        public decimal RemQty { get; set; }

        public int MatID { get; set; }

        public ConsumptionList List { get; set; }
    }

    public class Consumption
    {
        public decimal Qty { get; set; }

        public string Uom { get; set; }

        public string Remarks { get; set; }

        public string ActionBy { get; set; }

        public DateTime ActionOn { get; set; }

        public string Quantity { get { return string.Format("{0} {1}", Qty.ToString("G29"), Uom); } }
    }

    public class ConsumptionList : List<Consumption> { }

    public class MiscConsumption
    {
        public int PackInvID { get; set; }

        public decimal Qty { get; set; }

        public string Uom { get; set; }

        public string Remarks { get; set; }
    }

    public class GetPackInvDetails
    {
        public string EncInvPackID { get; set; }

        public string EncInvID { get; set; }

        public int InvID { get { return CommonStaticMethods.Decrypt<int>(EncInvID); } }

        public int InvPackID { get { return CommonStaticMethods.Decrypt<int>(EncInvPackID); } }
    }

    public class PackInvDetails
    {
        public string PreparationQuantityString { get; set; }

        public string UOM { get; set; }

        public string PreparationSource { get; set; }

        public string ReferenceCode { get; set; }

        public string PrepQntyUOM { get { return string.Format("{0} {1}", PreparationQuantityString, UOM); } }

        public DateTime? ReservedOn { get; set; }
    }
    public class PackInvDetailsList : List<PackInvDetails> { }

    public class GetQCInventorySources
    {
        public int EntityID { get; set; }

        public string Entity { get; set; }
    }

    public class GetQCInventorySourcesList : List<GetQCInventorySources> { }

    public class QCInvBatchSplitBO
    {
        public string EncInvID { get; set; }

        public int InvID { get { return CommonStaticMethods.Decrypt<int>(EncInvID); } }

        public string EncInvSourceID { get; set; }

        public int InvSourceID { get { return CommonStaticMethods.Decrypt<int>(EncInvSourceID); } }

        public decimal BatchQnty { get; set; }

    }

    public class GetBatchSplitDetailsBO
    {
        public string Retmsg { get; set; }

        public ChemicalList List { get; set; }

    }

    public class GetActionAndRptData
    {
        public string ReturnFlag { get; set; }

        public string refCode { get; set; }

        public UploadReportList RptList { get; set; }
    }
}
