using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.SampleDestruction
{
    public class GetPacksForDestruction
    {
        public int MatID { get; set; }

        public int InvID { get; set; }

        public int StatusID { get; set; }

        public DateTime BatchUseBeforeDate { get; set; }

        public DateTime UseBeforeDate { get; set; }
    }

    public class GetResultPacksForDestruction : MatBO
    {
        public int PackInvID { get; set; }

        public string  EncPackInvID { get { return CommonStaticMethods.Encrypt(PackInvID.ToString()); } }

        public string RefNumber { get; set; }

        public string BatchNumber { get; set; }

        public decimal RemQty { get; set; }

        public string Uom { get; set; }

        public string RemQtyUom{ get { return string.Format("{0} {1}", CommonStaticMethods.ConvertToFriendlyDecimal(RemQty), Uom); } }

        public DateTime? UseBeforeDate { get; set; }

        public string Status{ get; set; }

    }

    public class GetPacksForDestructionDetails
    {
        public string Retcode{ get; set; }

        public List<GetResultPacksForDestruction> GetResultPacksForDestruction { get; set; }
    }

    public class GetResultPacksForDestructionList : List<GetResultPacksForDestruction> { }


    public class GetDestructionSamplesDetails
    {
        public int ContainerID { get; set; }

        public string EncContainerID { get { return CommonStaticMethods.Encrypt(ContainerID.ToString()); } }

        public DateTime RequestRaisedOn { get; set; }

        public string SampleSource { get; set; }

        public string ChemicalName { get; set; }

        public string BatchNumber { get; set; }

        public string SourceReferenceNumber { get; set; }

        public string DestructionQuantity { get; set; }

        public Boolean IsSelected { get; set; }

    }

    public class GetManageDestructionSamplesDetails
    {
        public int DestructionSource { get; set; }

        public int TypeOfWaste { get; set; }

        public int NatureOfWaste { get; set; }

        public int ModeOfDestruction { get; set; }

        public string Quantity { get; set; }

        public string DisposalRemarks { get; set; }

        public string RefNumber { get; set; }

        public string DestructionOfSource { get; set; }

        public string RequestCode { get; set; }

        public string Status { get; set; }

        public string DestructionSourceName { get; set; }

        public string DestructionSourceCode { get; set; }

        public string TypeOfWasteName { get; set; }

        public string TypeOfWasteCode { get; set; }

        public string NatureOfWasteName { get; set; }

        public string NatureOfWasteCode { get; set; }

        public string ModeOfDestructionName { get; set; }

        public string ModeOfDestructionCode { get; set; }
    }

    public class GetResultsDestructionSamplesDetails
    {
        public List<GetDestructionSamplesDetails> GetDestructionDetailsList { get; set; }

        public GetManageDestructionSamplesDetails GetManageDestructionDetails { get; set; }

        public RecordActionDetails RecordActionResults { get; set; }
    }

    public class GetResultDiscardSample
    {
        public string Retcode { get; set; }

        public List<GetDestructionSamplesDetails> GetDestructionDetailsList { get; set; }

        public GetManageDestructionSamplesDetails GetManageDestructionDetails { get; set; }

        public RecordActionDetails RecordActionResults { get; set; }
    }


    [XmlType("Sample")]
    public class SavePack
    {

        public int SourceActualID { get; set; }

        public string DestructionQuantity { get; set; }

        public int MatID { get; set; }

        public string SourceReferenceNumber { get; set; }

        public string BatchNumber { get; set; }

    }

    [XmlType("Samples")]
    public class SavePacksList : List<SavePack> { }

    public class SavePacksForDestruction
    {
        public string SourceCode { get; set; }

        public string Remarks { get; set; }

        public SavePacksList List { get; set; }

        public string PackXml { get { return CommonStaticMethods.Serialize<SavePacksList>(List); } }
    }

    public class ManageDestructionSamples
    {
        public int DestructionID{ get { return CommonStaticMethods.Decrypt<int>(EncDestructionID); } }

        public string EncDestructionID { get; set; }

        public int DestructionSource { get; set; }

        public int TypeOfWaste { get; set; }

        public int NatureOfWaste { get; set; }

        public int ModeOfDestruction { get; set; }

        public string Quantity { get; set; }

        public ContainerDateList List { get; set; }

        public string SampleXml{ get { return CommonStaticMethods.Serialize<ContainerDateList>(List); } }

        public string DisposalRemarks { get; set; }

        public string RefNumber { get; set; }

        public string DestructionOfSource { get; set; }

        public string InitTime { get; set; }
    }

    [XmlType("ITEM")]
    public class ContainerData
    {
        public string ContainerID { get; set; }
    }

    [XmlType("RT")]
    public class ContainerDateList : List<ContainerData> { }

    public class SearchSampleDestruction : PagerBO
    {
        public int DestructionID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int DestructionSource { get; set; }

        public int StatusID { get; set; }

        public int WasteType { get; set; }

        public int NatureOfWaste { get; set; }

        public int ModeOfDestruction { get; set; }

        public int MatID { get; set; }

        public int BatchNumberID { get; set; }

        public int CreatedUserRoleID { get; set; }

        public DateTime CreatedOn { get; set; }
    }

    public class GetSampleDestruction
    {
        public int DestructionID { get; set; }

        public string EncDestructionID { get { return CommonStaticMethods.Encrypt(DestructionID.ToString()); } }

        public DateTime RequestDate { get; set; }

        public string RequestCode { get; set; }

        public string RequestSource { get; set; }

        public string Quantity { get; set; }

        public string Status{ get; set; }

        public string RequestCreatedBy { get; set; }

        public string TypeOfWaste { get; set; }
    }

    public class DiscardSamples
    {
        public ContainerDateList List { get; set; }

        public string SampleXml { get { return CommonStaticMethods.Serialize<ContainerDateList>(List); } }
    }

    public class GetSampleDestructionDetailsForView
    {
 
        public Destructions Destructions { get; set; }

        public List<DestructionContainer> DestructionContainerList { get; set; }
    }

    public class DestructionContainer
    {
        public DateTime RequestRaisedOn { get; set; }

        public string SampleSource { get; set; }

        public string ChemicalName { get; set; }

        public string BatchNumber { get; set; }

        public string SourceReferenceNumber { get; set; }

        public string DestructionQuantity { get; set; }

    }

    public class Destructions
    {
        public string DestructionSource { get; set; }

        public string TypeOfWaste { get; set; }

        public string NatureOfWaste { get; set; }

        public string ModeOfDestruction { get; set; }

        public string Quantity { get; set; }

        public string DisposalRemarks { get; set; }

        public DateTime RequestRaisedOn { get; set; }

        public string SampleSource { get; set; }

        public string ChemicalName { get; set; }

        public string BatchNumber { get; set; }

        public string SourceReferenceNumber { get; set; }

        public string DestructionQuantity { get; set; }

        public string RequestCode { get; set; }

        public string Status { get; set; }

        public string RefNumber { get; set; }

        public string DestructionOfSource { get; set; }
    }

  


}
