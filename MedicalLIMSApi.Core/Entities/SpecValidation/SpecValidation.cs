using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Entities.SpecValidation
{
    public class ManageCycleDetailsBO
    {
        public string EncSpecValidationID { get; set; }

        public int SpecValidationID { get { return CommonStaticMethods.Decrypt<int>(EncSpecValidationID); } }

        public string InitTime { get; set; }

        public string Type { get; set; }

        public int SpecValidCycleID { get; set; }

        public string EntityCode { get; set; }
    }

    public class SpecValidationCycle
    {
        public RecordActionDetails Act { get; set; }

        public List<GetSpecCycleDetails> SpecValidtions { get; set; }
    }

    public class ManageSpecValidation
    {
        public int SpecTestCatID { get; set; }

        public int Mode { get; set; }

        public string EntityCode { get; set; }

        public int StpTemplateID { get; set; }
    }


    public class GetSpecValidationDetails
    {
        public GetSpecTestDetails GetSpecTestDetailsBO { get; set; }

        public List<GetSpecCycleDetails> SpecCycleDetail { get; set; }

        public RecordActionDetails RecordActionResults { get; set; }

    }

    public class GetSpecTestDetails
    {
        public int SpecTestCatID { get; set; }

        public int Mode { get; set; }

        public string TestTitle { get; set; }

        public string ModeType { get; set; }

        public int SpecID { get; set; }

        public string Status { get; set; }

        public string RequestCode { get; set; }

        public string Specification { get; set; }

        public string Template { get; set; }

        public string ModeCode { get; set; }

        public string SpecNumber { get; set; }

        public string SpecificationName { get { return string.Format("{0} ({1})", Specification, SpecNumber); } }

        public string MatCode { get; set; }

        public int? DocumentID { get; set; }

        public string DocumentName { get; set; }

    }

    public class EditSpecValidation
    {
        public string EncSpecValidationID { get; set; }

        public int SpecValidationID { get { return CommonStaticMethods.Decrypt<int>(EncSpecValidationID); } }

        public int UserRoleID { get; set; }

        public string EntityCode { get; set; }
    }

    public class GetSpecCycleDetails
    {
        public int SpecValidCycleID { get; set; }

        public string Cycle { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        public bool IsSystemGenerated { get; set; }

    }


    public class SearchSpecValidations : PagerBO
    {
        public int TestID { get; set; }

        public int Mode { get; set; }

        public int StatusID { get; set; }

        public string EntityCode { get; set; }

        public short PlantID { get; set; }

        public int SpecID { get; set; }

        public int TemplateID { get; set; }

        public int SpecTypeID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int InstrumentTypeID { get; set; }

        public DateTime InitiatedOn { get; set; }

        public int InitiatedBy { get; set; }
    }

    public class SearchResultSpecValidations
    {
        public string EncSpecValidationID { get { return CommonStaticMethods.Encrypt(SpecValidationID.ToString()); } }

        public int SpecValidationID { get; set; }

        public string RequestCode { get; set; }

        public string TestTitle { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string UserName { get; set; }

        public string Mode { get; set; }

        public DateTime CreatedOn { get; set; }

        public string StpNumber { get; set; }

        public string SpecificationNumber { get; set; }

        public string SpecType { get; set; }

        public int EntActID { get; set; }
    }

    public class Validate
    {
        public int TestCatID { get; set; }

        public int EntActID { get; set; }

        public string EntityCode { get; set; }
    }

    public class GetSpecificationTestToAssignSTP
    {
        public int SpecCatID { get; set; }

        public string EncSpecCatID { get { return CommonStaticMethods.Encrypt(SpecCatID.ToString()); } }

        public string SrNum { get; set; }

        public int? TestCategoryID { get; set; }

        public int? TestSubCatID { get; set; }

        public int? SpecTestID { get; set; }

        public string SpecDesc { get; set; }

        public string ValidationStatus { get; set; }

        public string TestCode { get; set; }

        public string TestName { get; set; }

        public bool IsGroupTest { get; set; }

        public string ResultStatus { get; set; }

        public string TemplateCode { get; set; }

        public int? GroupSpecCatID { get; set; }

        public string RowType { get; set; }
    }

    public class AssignSTPToTest
    {
        public int SpecID { get; set; }

        public int TemplateID { get; set; }

        public int CalibID { get; set; }

        public string Type { get; set; }

        public SIngleBOList List { get; set; }

        public string TestXML { get { return CommonStaticMethods.Serialize<SIngleBOList>(List); } }

    }

    public class TestSTPHistory
    {
        public string TemplateCode { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime ? EffectiveTo { get; set; }
    }
}
