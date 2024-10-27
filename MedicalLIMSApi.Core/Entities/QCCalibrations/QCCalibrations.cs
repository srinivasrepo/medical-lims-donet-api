using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.QCCalibrations
{
    public class QCCalibrationsHeadersInfoBO
    {
        public string EncCalibParamID { get; set; }

        public int CalibParamID { get { return CommonStaticMethods.Decrypt<int>(EncCalibParamID); } }

        public int InstrumentTypeID { get; set; }

        public string Title { get; set; }

        public string InitTime { get; set; }

        public SIngleBOList UploadFiles { get; set; }

        public string XMLUpload { get { if (UploadFiles.Count > 0) return CommonStaticMethods.Serialize<SIngleBOList>(UploadFiles); else return string.Empty; } }

        public SIngleBOList InstrumentList { get; set; }

        public string InstrumentTypeXML { get { if (InstrumentList.Count > 0) return CommonStaticMethods.Serialize<SIngleBOList>(InstrumentList); else return string.Empty; } }

        public string InstrUserCodes { get; set; }

        public string ManualReferenceNumber { get; set; }

        public string EntityCode { get; set; }

        public string Role { get; set; }
    }

    public class GetQCCalibrationsHeadersInfoBO
    {
        public int InstrumentType { get; set; }

        public int InstrumentID { get; set; }

        public string Title { get; set; }

        public string InstrumentTypeDesc { get; set; }

        public string InstrumentName { get; set; }

        public string Status { get; set; }

        public string RequestCode { get; set; }

        public string InitTime { get; set; }

        public TransResultApprovals Trn { get; set; }

        public SIngleBOList SelectedInstList { get; set; }

        public string InstrumentTypeCode { get; set; }

        public string InstTypes { get; set; }

        public List<InstrumentCategoryItemsBO> InstrumentTypeIDList { get; set; }

        public string ManualReferenceNumber { get; set; }

        public string ObsoluteComments { get; set; }

        public string ActionBy { get; set; }

        public DateTime? ActionOn { get; set; }
    }

    public class AddNewSpecCategoryBO
    {
        public int CategoryID { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string EncCalibParamID { get; set; }

        public int CalibParamID { get { return CommonStaticMethods.Decrypt<int>(EncCalibParamID); } }
    }

    public class QCGetSingleTestMethod
    {
        public string InitTime { get; set; }

        public string ReturnFlag { get; set; }

        public int SpecTestID { get; set; }
    }

    public class ManageQCSingleTestMethod
    {
        public string EncCalibParamID { get; set; }

        public int CalibParamID { get { return CommonStaticMethods.Decrypt<int>(EncCalibParamID); } }

        public int TestID { get; set; }

        public string SpecLimit { get; set; }

        public string DescResultXML { get { return CommonStaticMethods.Serialize<CalibrationResultBOList>(List); } }

        public CalibrationResultBOList List { get; set; }

        public string LimitType { get; set; }

        public decimal LowerLimit { get; set; }

        public decimal UpperLimit { get; set; }

        public int CategoryID { get; set; }

        public int SubCategoryID { get; set; }

        public string InitTime { get; set; }

        public int SpecTestID { get; set; }

        public bool IsLowerLimitApp { get; set; }

        public bool IsUpperLimitApp { get; set; }
    }

    [XmlType("RESULT")]
    public class CalibrationResultBO
    {
        [XmlElement("RESULT_NAME")]
        public string ResultName { get; set; }


        [XmlElement("STATUS_CODE")]
        public string Result { get; set; }
    }

    [XmlType("RESULTS")]
    public class CalibrationResultBOList : List<CalibrationResultBO> { }


    public class GetCalibrationTestsBO
    {
        public int SpecCatID { get; set; }

        public string EncSpecCatID { get { return CommonStaticMethods.Encrypt(Convert.ToString(SpecCatID)); } }

        public string SrNum { get; set; }

        public int? TestCategoryID { get; set; }

        public string TestSubCategory { get; set; }

        public string TestCategory { get; set; }

        public string TestType { get; set; }

        public int? TestSubCatID { get; set; }

        public int? SpecTestID { get; set; }

        public string SpecDesc { get; set; }

        public string RowType { get; set; }

        public decimal? LimitFrom { get; set; }

        public decimal? LimitTo { get; set; }

        public string UnitSymbol { get; set; }

        public string TestName { get; set; }

        public string TestCode { get; set; }

        public string LimitResult
        {
            get
            {
                if (LimitFrom != null && LimitTo != null)
                    return string.Format("{0} {1} To {2} {3}", LimitFrom, CommonStaticMethods.GetSymbol(UnitSymbol), LimitTo, CommonStaticMethods.GetSymbol(UnitSymbol));
                else if (LimitFrom != null)
                    return string.Format("Greater than {0} {1}", LimitFrom, CommonStaticMethods.GetSymbol(UnitSymbol));
                else if (LimitTo != null)
                    return string.Format("Less than {0} {1}", LimitTo, CommonStaticMethods.GetSymbol(UnitSymbol));
                else return string.Empty;
            }
        }

        public string LimitType { get; set; }

        public string TestNameCode { get { return RowType == "TEST" ? string.Format("{0} ({1})", TestName, TestCode) : TestName; } }

        public bool IsGroupTest { get; set; }

        public string STPTitle { get; set; }

        public bool? IsLowerLimitApp { get; set; }

        public bool? IsUpperLimitApp { get; set; }
    }
    public class GetCalibrationTestsBOList : List<GetCalibrationTestsBO> { }


    public class GetCalibrationsTestDetailsBO
    {
        public int? TestCategoryID { get; set; }

        public string TestCategory { get; set; }

        public int? TestSubCatID { get; set; }

        public string TestSubCategory { get; set; }

        public int? TestID { get; set; }

        public string TestName { get; set; }

        public string SpecDesc { get; set; }

        public string TestType { get; set; }

        public decimal? LimitFrom { get; set; }

        public decimal? LimitTo { get; set; }

        public bool? IsLowerLimitApp { get; set; }

        public bool? IsUpperLimitApp { get; set; }

        public CalibrationResultBOList List { get; set; }
    }

    public class QCSPECDeleteTestMethodsBO
    {
        public int SpecTestID { get; set; }

        public string InitTime { get; set; }

        public string EncCalibParamID { get; set; }

        public int CalibParamID { get { return CommonStaticMethods.Decrypt<int>(EncCalibParamID); } }

        public int CategoryID { get; set; }

        public int SubCategoryID { get; set; }
    }

    public class ViewQCCalibrationDetailsBO
    {
        public GetQCCalibrationsHeadersInfoBO HeadersInfo { get; set; }

        public GetCalibrationTestsBOList List { get; set; }

    }

    public class SearchQCCalibrationsBO : PagerBO
    {
        public int InstrumentType { get; set; }

        public short StatusID { get; set; }

        public int InstrumentID { get; set; }

        public int CalibrationID { get; set; }

        public string Title { get; set; }

        public int CalibrationIDTo { get; set; }

        public int CalibrationIDFrom { get; set; }

        public DateTime InitiatedOn { get; set; }

        public int InitiatedBy { get; set; }
    }

    public class SearchQCCalibrationResultBO
    {
        public int CalibParamID { get; set; }

        public string EncCalibParamID { get { return CommonStaticMethods.Encrypt(Convert.ToString(CalibParamID)); } }

        public DateTime CreatedOn { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public string InstrumentTypeDesc { get; set; }

        public string RequestCode { get; set; }

        public string StatusCode { get; set; }

        public int? AssignedInstruments { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public string RequestCodeAndTitle { get { return string.Format("{0}-{1}", RequestCode, Version); } }

        public string HasChild { get; set; }

        public bool ShowAssignInst { get; set; }

        public bool ShowAssignPlant { get; set; }

        public bool ShowNewVersion { get; set; }

        public bool ShowManageArds { get; set; }
    }

    public class ManageAssignSTPGroupTestDetails
    {
        public string EncCalibParamID { get; set; }

        public int CalibParamID { get { return CommonStaticMethods.Decrypt<int>(EncCalibParamID); } }

        public int TemplateID { get; set; }

        public string InitTime { get; set; }

        public ManageAssignSTPGroupTestBOList List { get; set; }

        public string XMLTest { get { return CommonStaticMethods.Serialize<ManageAssignSTPGroupTestBOList>(List); } }

        public int SpecID { get; set; }

        public string EntityCode { get; set; }
    }

    [XmlType("ITEM")]
    public class ManageAssignSTPGroupTestBO
    {
        public int SpecCatID { get; set; }

        public bool IsSelected { get; set; }

    }

    [XmlType("RT")]
    public class ManageAssignSTPGroupTestBOList : List<ManageAssignSTPGroupTestBO> { }

    public class GetIntrumentsBO
    {
        public int EquipmentID { get; set; }

        public string EqpUserCode { get; set; }
    }

    public class GetIntrumentsBOList : List<GetIntrumentsBO> { }

    public class TestBo
    {
        public int SpecID { get; set; }

        public int CalibID { get; set; }
    }

    public class SpecTest
    {
        public string SrNum { get; set; }

        public string TestName { get; set; }

        public string RowType { get; set; }

        public string SpecDesc { get; set; }
    }

    public class SpecTestList : List<SpecTest> { }

    public class ManagePlantForCalibrationParameters
    {
        public string EncCalibParamID { get; set; }

        public int CalibParamID { get { return CommonStaticMethods.Decrypt<int>(EncCalibParamID); } }

        public string Type { get; set; }

        public SIngleBOList List { get; set; }

        public string PlantXml { get { return CommonStaticMethods.Serialize<SIngleBOList>(List); } }
    }

    public class GetPlantForCalibrationParameters
    {
        public short PlantID { get; set; }

        public string PlantName { get; set; }

        public bool isSelect { get; set; }
    }

    public class ManagePlantForCalibrationParametersResult
    {
        public List<GetPlantForCalibrationParameters> GetPlantForCalibrationParameters { get; set; }

        public string ReturnFlag { get; set; }
    }

    public class AssignInstrumentDetailsBO
    {
        public string EncCalibParamID { get; set; }

        public int CalibParamID { get { return CommonStaticMethods.Decrypt<int>(EncCalibParamID); } }

        public string Type { get; set; }

        public int? InstrumentTypeID { get; set; }

        public InstrumentList List { get; set; }

        public string InstrumentXml { get { return CommonStaticMethods.Serialize<InstrumentList>(List); } }

    }

    public class AssignInstrumentList
    {
        public InstrumentList List { get; set; }

        public string ReturnFlag { get; set; }

    }

    [XmlType("ITEM")]
    public class InstrumentData
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool IsSelect { get; set; }
    }

    [XmlType("RT")]
    public class InstrumentList : List<InstrumentData> { }

    public class ManageArdsDocuments
    {
        public string EncCalibParamID { get; set; }

        public int CalibParamID { get { return CommonStaticMethods.Decrypt<int>(EncCalibParamID); } }

        public int DocTrackID { get; set; }

        public string Mode { get; set; }
    }

    public class CALIBManageARDS
    {
        public string ReturnFlag { get; set; }

        public List<GetArdsDocuments> GetArdsDocuments { get; set; }
    }

    public class GetArdsDocuments
    {
        public int SpecArdsID { get; set; }

        public string DocNumber { get; set; }

        public string DocName { get; set; }

        public int DocTracID { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }
    }

    public class InstrumentCategoryItemsBO
    {
        public int ID { get; set; }

        public string CatItem { get; set; }

        public string CatItemCode { get; set; }
    }

    public class GetReferenceNumber
    {
        public string EncEntActID { get; set; }

        public int EntActID { get { return CommonStaticMethods.Decrypt<int>(EncEntActID); } }

        public string SectionCode { get; set; }

        public string EntityCode { get; set; }

        public SIngleBOList FileIds { get; set; }

        public string IDXml { get { return CommonStaticMethods.Serialize<SIngleBOList>(FileIds); } }
    }
}
