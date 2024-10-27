using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.SampleAnalysis
{
    public class SearchSampleAnalysisBO : PagerBO
    {
        public int CatID { get; set; }

        public int SubcatID { get; set; }

        public int MatID { get; set; }

        public int SampleID { get; set; }

        public int ProductID { get; set; }

        public int StageID { get; set; }

        public int BatchID { get; set; }

        public int AnalysisTypeID { get; set; }

        public int ARID { get; set; }

        public int StatusID { get; set; }

        public int PlantAreaID { get; set; }

        public int ProjectID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public byte MoleclueType { get; set; }

    }

    public class AnalysisSearchResult
    {
        public int UPD { get; set; }

        public int SampleAnalysisID { get; set; }

        public string EncSampleAnalysisID { get { return CommonStaticMethods.Encrypt(SampleAnalysisID.ToString()); } }

        public int SioID { get; set; }

        public string EncSioID { get { return CommonStaticMethods.Encrypt(SioID.ToString()); } }

        public string MaterialName { get; set; }

        public string Stage { get; set; }

        public string BatchNumber { get; set; }

        public string SampleNumber { get; set; }

        public int ArID { get; set; }

        public string ArNumber { get; set; }

        public string AnalysisType { get; set; }

        public DateTime? RetestDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string OosStatus { get; set; }

        public int IsRawDataApp { get; set; }

        public string SampleAnalysisType { get; set; }

        public string AnalysisCode { get; set; }

        public string Atrversion { get; set; }

        public string RptVersionCode { get; set; }

        public DateTime SampleOn { get; set; }

        public string SioCode { get; set; }

    }

    public class AnalysisHeader
    {
        public string ProductName { get; set; }

        public string MatCode { get; set; }

        public string Stage { get; set; }

        public string InvBatchNumber { get; set; }

        public string SioCode { get; set; }

        public string ArNumber { get; set; }

        public string AnalysisType { get; set; }

        public string Status { get; set; }

        public decimal? BatchQty { get; set; }

        public string SpecNumber { get; set; }

        public string SampleNumber { get; set; }

        public int? SpecID { get; set; }

        public int MatID { get; set; }

        public string AnalsysMode { get; set; }

        public string SampleMode { get; set; }

        public string InitTime { get; set; }

        public string CheckListCategory { get; set; }

        public string Uom { get; set; }

        public string AnalysisTypeCode { get; set; }

        public string InwardType { get; set; }

        public bool CanShowPack { get; set; }

        public bool? IsResetPending { get; set; }

        public RecordActionDetails Act { get; set; }

        public string Analysis { get; set; }

        public int? SamAnaID { get; set; }

        public string EncSamAnaID { get { return CommonStaticMethods.Encrypt(SamAnaID.ToString()); } }

        public DateTime InwardDate { get; set; }

        public string RetCode { get; set; }

        public string StatusCode { get; set; }

        public string UpdRemarksStatus { get; set; }

        public string ScreenCode { get; set; }

        public string SamplingPoint { get; set; }

        public string IUSampleType { get; set; }

        public DateTime? IURequestDate { get; set; }

        public string SourceUnit { get; set; }

        public string ConditionName { get; set; }

        public byte? Frequency { get; set; }

        public decimal? SampleQty { get; set; }

        public DateTime? MrrDate { get; set; }

        public string MrrNumber { get; set; }

        public int? NoOfContainers { get; set; }

        public string ProjectName { get; set; }

        public DateTime? MfgDate { get; set; }

        public DateTime? SupSamMfgDate { get; set; }

        public string BatchSource { get; set; }

        public string SampleUom { get; set; }

        public int? ArID { get; set; }

        public string ExtraneousMatterApplicable { get; set; }

        public bool IsReviewApplicable { get; set; }

        public string Specification { get; set; }

        public string SpecVersion { get; set; }

        public string DisplaySpec { get { if (string.IsNullOrEmpty(SpecNumber)) return ""; else return SpecNumber + " (" + Specification + ") "; } }

        public int? InvID { get; set; }

        public bool CanAccess { get; set; }

        public string RefSioCode { get; set; }

        public string IsMixedSolvent { get; set; }
    }

    public class AnalysisType
    {
        public int SPEC_TYPE_ID { get; set; }

        public string SPEC_TYPE { get; set; }

        public string SPEC_TYPE_CODE { get; set; }
    }

    public class AnalysisTypeList : List<AnalysisType> { }

    public class Block
    {
        public int BLOCK_ID { get; set; }

        public string BLOCK_NAME { get; set; }
    }

    public class BlockList : List<Block> { }

    public class SupplierCOADetails
    {
        public string InitTime { get; set; }

        public string StorageCondition { get; set; }

        public string CoaSpec { get; set; }

        public DateTime? SupRetestExpiryDate { get; set; }

        public string SupRetestExpiryTypeValue { get; set; }

        public string SupRetestExpiryType { get; set; }

        public string SampledBy { get; set; }

        public string SampleResult { get; set; }

        public string Remarks { get; set; }

        public int SioID { get { return CommonStaticMethods.Decrypt<int>(EncSioID); } }

        public string EncSioID { get; set; }

        public string encSamAnalysisID { get; set; }

        public int SamAnaID { get { return CommonStaticMethods.Decrypt<int>(encSamAnalysisID); } }

        public RecordActionDetails Act { get; set; }
    }

    public class MangeSampleAnalysis
    {
        public string EncSioID { get; set; }

        public int SioID { get { return CommonStaticMethods.Decrypt<int>(EncSioID); } }

        public string Deviation { get; set; }

        public string QuestionList { get { return CommonStaticMethods.Serialize<QUESList>(List); } }

        public QUESList List { get; set; }

        public string SampleCollected { get; set; }

        public short NoofContainers { get; set; }

        public string SampledBy { get; set; }

        public decimal CompositeSampleQty { get; set; }

        public int Uom { get; set; }

        public decimal QtyAnalysis { get; set; }

        public decimal ReserveSampleQty { get; set; }

        public int ReserveSampleUom { get; set; }

        public DateTime SampleReceviedOn { get; set; }

        public int SamplerID { get; set; }

        public int ReviewedBy { get; set; }

        public Boolean IsSaveSend { get; set; }

        public string UPLOAD_DOCT { get; set; }

        public string QtyFrom { get; set; }

        public string SampleStorageTemp { get; set; }

        public string WorkBookNumber { get; set; }

        public string SamplingPoint { get; set; }

        public string InitTime { get; set; }

        public RS232OtherFieldsBOList OtherList { get; set; }

        public string OtherListXML { get { if (OtherList.Count > 0) return CommonStaticMethods.Serialize<RS232OtherFieldsBOList>(OtherList); else return string.Empty; } }

        public string ReferenceNumber { get; set; }

        public string SamplerTitle { get; set; }

        public string ChecklistTypeCode { get; set; }
    }

    [XmlType("NewDataSet")]
    public class QUES
    {
        public int QuestionID { get; set; }

        public string Answer { get; set; }
    }

    [XmlType("Table1")]
    public class QUESList : List<QUES> { }

    public class MangeSampleAnalysisResult
    {
        public string InitTime { get; set; }

        public string ReturnFlag { get; set; }

        public string SampleAnalysisID { get; set; }

        public string Status { get; set; }

        public string AnalysisStatus { get; set; }

        public string LoginID { get; set; }

        public string DeptCode { get; set; }

        public string RoleName { get; set; }

        public string PlantCode { get; set; }

        public string EntityCode { get; set; }

        public SIngleBOList List { get; set; }

        public UploadReportList RptList { get; set; }
    }

    public class GetAssignedDocsBySpecID
    {
        public int ArdsID { get; set; }

        public string DoctNum { get; set; }

        public string DoctName { get; set; }

        public int DocTrackID { get; set; }
    }

    public class GetAssignedDocsBySpecIDList : List<GetAssignedDocsBySpecID> { }

    public class ARDSGetAssignedDocs
    {
        public string DoctNum { get; set; }

        public string DoctName { get; set; }

        public int DocID { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string DocPath { get; set; }

        public int? DmsID { get; set; }

        public string PlantOrgCode { get; set; }
    }

    public class ARDSGetAssignedDocsList : List<ARDSGetAssignedDocs> { }

    public class ARDSManageRequest
    {
        public int SpecID { get; set; }

        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public int TrackID { get; set; }

        public string DocPath { get; set; }

        public string AnalysisMode { get; set; }

        public string EncArdsID { get; set; }

        public int ArdsID { get { return CommonStaticMethods.Decrypt<int>(EncArdsID); } }

        public string Type { get; set; }

        public string InitTime { get; set; }

        public string EntityCode { get; set; }

        public string ExtraneousAnalysis { get; set; }

        public ExtraneousTestList List { get; set; }

        public string ExtraneousTestsXml { get { return CommonStaticMethods.Serialize<ExtraneousTestList>(List); } }

        public string ContainerWiseAnalysisApp { get; set; }

        public string Role { get; set; }

        public string SectionCode { get; set; }

        public int DMSID { get; set; }

        public short PlantID { get; set; }
    }

    public class GetSamplingInfo
    {
        public short? PacksSampled { get; set; }

        public string SampledBy { get; set; }

        public string SampleApplicability { get; set; }

        public int? SamplerID { get; set; }

        public decimal? SampleAnaQty { get; set; }

        public decimal? CompositQty { get; set; }

        public string StorageCondition { get; set; }

        public string WorkBookNo { get; set; }

        public string StbQtyFrom { get; set; }

        public int? UomID { get; set; }

        public decimal? ReserveSamQty { get; set; }

        public string Remarks { get; set; }

        public DateTime? SampledOn { get; set; }

        public Boolean? isSampled { get; set; }

        public Boolean? IsReviewed { get; set; }

        public DateTime? ReviewedOn { get; set; }

        public string ReviewedBy { get; set; }

        public string InwardType { get; set; }

        public string SpecTypeCode { get; set; }

        public string SampleTypeCode { get; set; }

        public int? IoID { get; set; }

        public string EncIoID { get { return CommonStaticMethods.Encrypt(Convert.ToString(IoID)); } }

        public string ContainerAnalysisApplicable { get; set; }

        public string Equipment { get; set; }

        public bool ShowCompQty { get; set; }

        public string SamplingPoint { get; set; }

        public string ChecklistTypeCode { get; set; }
    }

    public class GetSpecificationsBO
    {
        public int SpecID { get; set; }

        public string SpecNumber { get; set; }

        public string Specification { get; set; }

        public string EncSpecID { get { return CommonStaticMethods.Encrypt(SpecID.ToString()); } }

        public bool HasStp { get; set; }

        public string Version { get; set; }

        public string DisplayName { get { return SpecNumber + " (" + Specification + ") "; } }
    }
    public class GetSpecificationsBOList : List<GetSpecificationsBO> { }

    public class DiscardPrintrequestBO
    {
        public string EncEntActID { get; set; }

        public int EntActID { get { return CommonStaticMethods.Decrypt<int>(EncEntActID); } }

        public int ReqDocID { get; set; }

        public string EntityCode { get; set; }

        public string RefNumber { get; set; }
    }


    public class SampleContainerDetails
    {
        public string MatCode { get; set; }

        public string MatName { get; set; }

        public string MatUom { get; set; }

        public string ReqCode { get; set; }

        public decimal? ReqQuantity { get; set; }

        public int? SecUOMID { get; set; }

        public SamplePackList Lst { get; set; }
    }

    public class SamplePacks
    {
        public string BatchNumber { get; set; }

        public int EntInvID { get; set; }

        public decimal? GrossWeight { get; set; }

        public int InvPackID { get; set; }

        public string PackNumber { get; set; }

        public decimal? RemainQty { get; set; }

        public decimal? RequestQty { get; set; }

        public decimal? ReserveQty { get; set; }

        public string SealNumber { get; set; }

        public decimal? TareWeight { get; set; }

        public decimal? PackIssueQty { get; set; }

        public string AvailableQty { get { return Convert.ToString(RemainQty - ReserveQty); } }

    }

    public class SamplePackList : List<SamplePacks> { }

    public class GetSamplePack
    {
        public string PurposeCode { get; set; }

        public string EncSioID { get; set; }

        public int SioID { get { return CommonStaticMethods.Decrypt<int>(EncSioID); } }

        public int SecUomID { get; set; }

        public decimal ReqQuantity { get; set; }
    }

    public class InventoryPackIssueBO
    {
        public int EntInvID { get; set; }

        public int InvPackID { get; set; }

        public decimal IssueQty { get; set; }

        public string SealNo { get; set; }
    }

    public class ManageSamplePack
    {
        public string PurposeCode { get; set; }

        public string EncSioID { get; set; }

        public int SioID { get { return CommonStaticMethods.Decrypt<int>(EncSioID); } }

        public int SecUomID { get; set; }

        public InventoryPackIssueList Lst { get; set; }
    }


    public class InventoryPackIssueList : List<InventoryPackIssueBO>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            SqlDataRecord ret = new SqlDataRecord(
                new SqlMetaData("EntInvID", SqlDbType.Int),
                new SqlMetaData("InvPackID", SqlDbType.Int),
                new SqlMetaData("IssueQty", SqlDbType.Decimal, 15, 5),
                new SqlMetaData("SealNo", SqlDbType.VarChar, 25)
                );

            foreach (InventoryPackIssueBO data in this)
            {
                ret.SetInt32(0, data.EntInvID);
                ret.SetInt32(1, data.InvPackID);
                ret.SetDecimal(2, data.IssueQty);
                ret.SetString(3, data.SealNo);
                yield return ret;
            }
        }
    }

    public class GetAnalysisTestBO
    {
        public string SrNum { get; set; }

        public int? SpecCatID { get; set; }

        public string TestName { get; set; }

        public string SpecDesc { get; set; }

        public string ResultFrom { get; set; }

        public string ResultTo { get; set; }

        public string PassOrFail { get; set; }

        public Boolean IsSelected { get; set; }

        public int SamAnaTestID { get; set; }

        public int? SpecTestID { get; set; }

        public string TestType { get; set; }

        public string TestUom { get; set; }

        public string CorrectedResult { get; set; }

        public string TestDesc { get { return !string.IsNullOrEmpty(CorrectedResult) ? CorrectedResult : TestType == "R" && !string.IsNullOrEmpty(ResultTo) ? string.Format("{0} - {1} ", ResultFrom, ResultTo) : !string.IsNullOrEmpty(ResultFrom) ? ResultFrom : ResultFrom; } }

        public string RowType { get; set; }

        public bool? IsRRT { get; set; }

        public bool HasOccSubmitted { get; set; }

        public string TestInitTime { get; set; }

        public DateTime? RawdataUpdOn { get; set; }

        public DateTime? RawdataConfOn { get; set; }

        public int? TemplateID { get; set; }

        public int? TestCategoryID { get; set; }

        public bool HasOOS { get; set; }

        public bool HasDocuments { get; set; }

        public int? InvalidationID { get; set; }

        public string EncInvalidationID { get { if (InvalidationID > default(int)) return CommonStaticMethods.Encrypt(InvalidationID.ToString()); else return string.Empty; } }

        public string Template { get; set; }

        public int? TestID { get; set; }

        public bool IsGroupTest { get; set; }

        public bool ShowOcc { get; set; }

        public int? TestSubCatID { get; set; }

        public bool? IsInvalidationRaised { get; set; }

        public string RowTypeCode { get; set; }

        public string StatusCode { get; set; }

        public int? DataReviewID { get; set; }

        public int? AnalyticalDataReviewID { get; set; }

        public string DrVersionCode { get; set; }

        public string AdrVersionCode { get; set; }

        public byte? OOSTestUID { get; set; }

        public int? DmsReportID { get; set; }

        public bool? ShowSpecTestID { get; set; }

        public bool CanEditTestValues { get; set; }

        public string AnalystName { get; set; }

        public string ArdsMode { get; set; }

        public bool SpecTestValid { get; set; }

        public bool StpValid { get; set; }

        public int? UserArdsExecID { get; set; }

        public string UpdTestStatus { get; set; }

        public string AnalysisTestType { get; set; }
    }
    public class GetAnalysisTestBOList : List<GetAnalysisTestBO> { }

    public class GetAnalysisTests
    {
        public GetAnalysisTestBOList Lst { get; set; }

        public string ResultRemarks { get; set; }

        public string SpecialPrecautions { get; set; }

        public string AnalysisStatus { get; set; }

        public string ContainerAnaStatus { get; set; }

        public bool? IsContainerAnaCompleted { get; set; }

        public bool? IsSkipedFromAnalysis { get; set; }

        public bool? IsComprehensiveChkApp { get; set; }

        public string ComprehensiveRemarks { get; set; }
    }

    public class SampleTestInfo
    {
        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string TestTitle { get; set; }

        public string Result { get; set; }

        public string ResultTo { get; set; }

        public string PassOrFail { get; set; }

        public string SpecDesc { get; set; }

        public string MethodType { get; set; }

        public string TestInitTime { get; set; }

        public bool? IsOOSRequred { get; set; }

        public string UpdTestStatus { get; set; }

        public string ResultType { get; set; }

        public string SpecPassOrFail { get; set; }

        public int? TemplateID { get; set; }

        public string ArdsMode { get; set; }

        public string TypeCode { get; set; }

        public string CorrValue { get; set; }

        public string Justification { get; set; }

        public TestResultList Lst { get; set; }

        public TestResultValues TestResult { get; set; }

        public string TestUom { get; set; }

        public string PhaseType { get; set; }

        public int? NewSampleRefID { get; set; }

        public string NewSampleRefCode { get; set; }

        public string Type { get; set; }

        public int? TypeID { get; set; }

        public int? MainArdsExecID { get; set; }

    }

    public class TestResult
    {
        public int MethodResultID { get; set; }

        public string ResultName { get; set; }

        public string Result { get; set; }
    }

    public class TestResultList : List<TestResult> { }

    public class TestValues
    {
        public string EncSampleAnaTestID { get; set; }

        public int sampleAnaTestID { get { return CommonStaticMethods.Decrypt<int>(EncSampleAnaTestID); } }

        public decimal Result { get; set; }

        public decimal ResultTo { get; set; }

        public string SourceCode { get; set; }

    }

    public class TestResultValues
    {
        public string PassOrFail { get; set; }

        public string LimtFrom { get; set; }

        public string LimtTo { get; set; }

        public string DescResult { get; set; }

        public byte? DecimalFrom { get; set; }

        public byte? DecimalTo { get; set; }

        public string FromSkipType { get; set; }

        public string ToSkipType { get; set; }
    }

    public class UpdTestResults
    {
        public string EncSampleAnaTestID { get; set; }

        public int SampleAnaTestID { get { return CommonStaticMethods.Decrypt<int>(EncSampleAnaTestID); } }

        public string Result { get; set; }

        public string PassOrFail { get; set; }

        public string ResultTo { get; set; }

        public bool SendOss { get; set; }

        public string InitTime { get; set; }

        public string TestInitTime { get; set; }

        public string ResultType { get; set; }

        public string EntityCode { get; set; }

        public string SpecPassOrFail { get; set; }

        public string TypeCode { get; set; }

        public string CorrValue { get; set; }

        public string Justification { get; set; }

        public int NewSampleRefID { get; set; }
    }

    public class TestRetVal
    {
        public string ReturnFlag { get; set; }

        public string TestInitTime { get; set; }

        public string AnalysisResult { get; set; }

        public int? UserArdsExecID { get; set; }

        public bool HasOOS { get; set; }
    }

    public class AnalysisRemarks
    {
        public string Remarks { get; set; }

        public string InitTime { get; set; }

        public string specPrecautions { get; set; }

        public string EncSioID { get; set; }

        public int SioID { get { return CommonStaticMethods.Decrypt<int>(EncSioID); } }

        public string EntityCode { get; set; }

        public string SourceCode { get; set; }

        public int ContainerAnaID { get; set; }

        public string AnalysisStatus { get; set; }

        public string ReferenceNumber { get; set; }

    }

    public class Deviation
    {
        public string DcActionCode { get; set; }

        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public string EntityCode { get; set; }

        public string RefCode { get; set; }

        public string DevType { get; set; }

        public string Remarks { get; set; }

        public NUmberList Lst { get; set; }

        public string NumList { get { return CommonStaticMethods.Serialize<NUmberList>(Lst); } }
    }

    public class GetInstrumentTitlesBO
    {
        public int EqpID { get; set; }

        public string EqpCode { get; set; }

        public string EqpName { get; set; }

        public string EqpType { get; set; }
    }
    public class GetInstrumentTitlesBOList : List<GetInstrumentTitlesBO> { }

    public class GetEQPUGetEqpTypeCode
    {
        public string TypeCode { get; set; }

        public string ActTypeCode { get; set; }
    }

    public class GetTestInstrumentsBO
    {
        public int? SamInstID { get; set; }

        public string InstName { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        public string Remarks { get; set; }

        public bool? IsOccReq { get; set; }

        public bool? IsPrimary { get; set; }

        public string PrimaryOcc { get { return Convert.ToBoolean(IsPrimary) ? "Yes" : "No"; } }

        public string DataSeqFile { get; set; }

        public string MobilePhase { get; set; }

        public int? NoOfInjections { get; set; }

        public int? CumulativeInjection { get; set; }

        public string HplcGcColumn { get; set; }

        public string Duration { get; set; }

        public string AnalystName { get; set; }

        public string EncSamInstID { get { return CommonStaticMethods.Encrypt(SamInstID.ToString()); } }

        public int? InvalidationID { get; set; }

        public string StatusCode { get; set; }
    }
    public class GetTestInstrumentsBOList : List<GetTestInstrumentsBO> { }

    public class GetOccupancyData
    {
        public GetTestInstrumentsBOList Lst { get; set; }

        public DateTime SampleReceivedOn { get; set; }

        public bool IsOccRequired { get; set; }

        public string Remarks { get; set; }

        public int? SamInstID { get; set; }

        public bool HasCatLevelOcc { get; set; }

        public string EncSamInstID { get { if (SamInstID > default(int)) return CommonStaticMethods.Encrypt(Convert.ToString(SamInstID)); else return ""; } }
    }

    public class InsertNUpdateInstrumentsBO
    {
        public bool OccupancyRequired { get; set; }

        public string OccupancyType { get; set; }

        public int InstrumentTitleID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int ColumnID { get; set; }

        public string DataSeqFile { get; set; }

        public string EncSamAnaInstID { get; set; }

        public int SamAnaInstID { get { return CommonStaticMethods.Decrypt<int>(EncSamAnaInstID); } }

        public string EncSamAnalTestID { get; set; }

        public int SamAnaTestID { get { return CommonStaticMethods.Decrypt<int>(EncSamAnalTestID); } }

        public string Remarks { get; set; }

        public string TestInitTime { get; set; }

        public string EntityCode { get; set; }

        public ColumnsList List { get; set; }

        public string ColumnXml { get { return CommonStaticMethods.Serialize<ColumnsList>(List); } }

        public int RefEqpOccID { get; set; }
    }

    public class GetAnaOccuInstrumentsBO
    {
        public int? EquipmentID { get; set; }

        public string EqpTitle { get; set; }

        public string EqpCode { get; set; }

        public int? ColumnID { get; set; }

        public string ColumnCode { get; set; }

        public string ColTitle { get; set; }

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        public int? NoOfInjections { get; set; }

        public int? CumulativeInjections { get; set; }

        public bool? IsPrimaryOcc { get; set; }

        public string Remarks { get; set; }

        public bool? IsOccReq { get; set; }

        public string DataSeqFile { get; set; }

        public string MobilePhase { get; set; }

        public string TypeCode { get; set; }

        public bool IsParameterOcc { get; set; }

        public int? MobilePhaseID { get; set; }

        public string BatchNumber { get; set; }

        public DateTime? UseBeforeDate { get; set; }

        public int? EpqOccID { get; set; }

        public ColumnsList lst { get; set; }

        public int? RefEqpOccID { get; set; }

        public bool IsRefOcc { get { return RefEqpOccID > default(int); } }

        public string RefInvBatch { get; set; }
    }

    public class GetSampleRRTValuesBO
    {
        public string EncSamTestID { get; set; }

        public int SamTestID { get { return CommonStaticMethods.Decrypt<int>(EncSamTestID); } }

        public string Type { get; set; }

        public string TestDesc { get; set; }

        public string Result { get; set; }

        public string AcceptenceCriteria { get; set; }

        public string EncRRtID { get; set; }

        public int RRtID { get { return CommonStaticMethods.Decrypt<int>(EncRRtID); } }

        public string InitTime { get; set; }
    }

    public class GetSampleTestRRTValuesBO
    {
        public int RRTID { get; set; }

        public string EncRRTID { get { return CommonStaticMethods.Encrypt(RRTID.ToString()); } }

        public string TestDesc { get; set; }

        public string Result { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AcceptanceCriteria { get; set; }
    }
    public class GetSampleTestRRTValuesBOList : List<GetSampleTestRRTValuesBO> { }

    public class DynamicValues
    {
        public int ResultID { get; set; }

        public string Value { get; set; }

        public string ActualValue { get; set; }

        public string ItemDescription { get; set; }

        public int ImpurityValueID { get; set; }
    }

    public class GetARDSInputDetails
    {
        public string TestTitle { get; set; }

        public string STPTitle { get; set; }

        public string InitTime { get; set; }

        public string RawdataUpdatedBy { get; set; }

        public DateTime? RawdataUpdOn { get; set; }

        public string RawDataConfirmedBy { get; set; }

        public DateTime? RawdataConfOn { get; set; }

        public TabList TabList { get; set; }

        public SectionList SectionList { get; set; }

        public SectionDetailList SectionDetailsList { get; set; }

        public InvalidationBOList InvList { get; set; }

        public SolventsList SolList { get; set; }

        public ArdsReviewList ReviewList { get; set; }

        public string UpdTestStatus { get; set; }

        public string ChemicalConsumeComments { get; set; }

        public int? ChemicalConsumeRefArID { get; set; }

        public string ChemicalConsumeRefArNumber { get; set; }

        public int? ChemicalConsumeRefArdsExecID { get; set; }

        public string ChemialConsumeTestTitle { get; set; }

        public List<DynamicValues> DynamicValueLst { get; set; }

        public TableResultSetsList TableResultLst { get; set; }

        public bool IsNewReq { get; set; }

        public string EntityCode { get; set; }

        public bool? IsUnknownMapping { get; set; }

        public bool? IsknownMapping { get; set; }
    }

    public class InvalidationBO
    {
        public string InvalidationCode { get; set; }

        public int InvalidationID { get; set; }

        public string ActionBy { get; set; }

        public DateTime? ActionDate { get; set; }

        public int? DmsReportID { get; set; }
    }
    public class InvalidationBOList : List<InvalidationBO> { }

    public class ARDSSection
    {
        public int? TabID { get; set; }

        public int SectionID { get; set; }

        public string Section { get; set; }

        public string SectionSubject { get; set; }
    }

    public class SectionList : List<ARDSSection> { }

    public class TabBo
    {
        public int TabID { get; set; }

        public string Tab { get; set; }

        public string SectionSubject { get; set; }
    }

    public class TabList : List<TabBo> { }

    public class SectionData
    {
        public string InputType { get; set; }

        public string InputCode { get; set; }

        public string InputDescription { get; set; }

        public int? SectionID { get; set; }

        public short? ItemOrder { get; set; }

        public int DetailID { get; set; }

        public string Value { get; set; }

        public string PrevValue { get { return Value; } }

        public string UpdateFlag { get; set; }

        public bool IsDisable { get { return (!string.IsNullOrEmpty(Value) || InputType == "UNKN_PAREA" || InputType == "KNOWN_PAREA" || InputType == "UNKN_AREA" || InputType == "KNOWN_AREA"); } }

        public string KeyName { get; set; }

        public int? InvalidationID { get; set; }

        public string PassOrFail { get; set; }

        public string FormulaResultFlag { get; set; }

        public int? TabID { get; set; }

        public int? SpecTestID { get; set; }

        public int? SdmsID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string InitialValue { get; set; }

        public string SkipType { get; set; }

        public bool? IsCommonData { get; set; }

        public int? ResultID { get; set; }

        public bool? IsFormulaDependent { get; set; }

        public bool IsAccCriteriaApp { get; set; }

        public string FormulaType { get; set; }
    }

    public class SectionDetailList : List<SectionData> { }

    public class InputValues
    {
        public string ArdsSourceCode { get; set; }

        public int SamAnaTestID { get; set; }

        public int DetailID { get; set; }

        public string Value { get; set; }

        public string InitTime { get; set; }

        public bool IsFormulaEval { get; set; }

        public string ActValue { get; set; }

        public FormulaDependentList ImpurityValues { get; set; }

        public string ImpurityXML { get; set; }
    }

    public class AdditionalTest
    {
        public int AdditionalTestID { get; set; }

        public int TestID { get; set; }

        public string TestTitle { get; set; }

        public string SpecLimit { get; set; }

        public string Result { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CretedOn { get; set; }
    }

    public class AdditionalTestList : List<AdditionalTest> { }

    public class AdditionalTestBODetails
    {
        public string EncSamAnaID { get; set; }

        public int SamAnaID { get { return CommonStaticMethods.Decrypt<int>(EncSamAnaID); } }

        public string EntityCode { get; set; }
    }

    public class MngAdditionalTest
    {
        public string EncSamAnaID { get; set; }

        public int SamAnaID { get { return CommonStaticMethods.Decrypt<int>(EncSamAnaID); } }

        public int TestID { get; set; }

        public string SpecLimit { get; set; }

        public string Result { get; set; }
    }

    public class GetAddTests
    {
        public string ReturnFlag { get; set; }

        public AdditionalTestList Lst { get; set; }
    }


    [XmlType("NS")]
    public class NumberID
    {
        public int DID { get; set; }
    }

    [XmlType("RT")]
    public class NUmberList : List<NumberID> { }

    public class UpdFinalRemarks
    {
        public string EncSamAnaID { get; set; }

        public int SamAnaID { get { return CommonStaticMethods.Decrypt<int>(EncSamAnaID); } }

        public string InitTime { get; set; }

        public string Remarks { get; set; }

        public string EntityCode { get; set; }
    }

    public class FormulaDepenDetails
    {
        public string InputDescription { get; set; }

        public short? ItemOrder { get; set; }

        public int DetailID { get; set; }

        public string Value { get; set; }

        public string UpdateFlag { get; set; }

        public string InputCode { get; set; }

        public string ActualValue { get; set; }

        public int? UnknownImpurityID { get; set; }

        public string CalcFormula { get; set; }

        public int? ImpurityValueID { get; set; }

        public string Type { get; set; }

        public bool IsDisable { get { if (string.IsNullOrEmpty(Type) || Type == "REG") return false; else return true; } }

        public string FormulaResultFlag { get; set; }

        public int? SpecTestID { get; set; }

        public string Comments { get; set; }
    }

    public class FormulaDependentList : List<FormulaDepenDetails> { }

    public class ManageMultFormulaValueBO
    {
        public int SamAnaTestID { get; set; }

        public string ArdsSourceCode { get; set; }

        public string InitTime { get; set; }

        public FormulaDependentList Lst { get; set; }

        public string FormulaXML { get { return CommonStaticMethods.Serialize<FormulaDependentList>(Lst); } }

    }

    public class GetMulitpleValueExecBO
    {
        public string ReturnFlag { get; set; }
    }

    public class ExecuteFormula
    {
        public string Code { get; set; }

        public string Formula { get; set; }

        public string ActualValue { get; set; }

        public string Value { get; set; }

        public string IsAcceptanceCriteriaApp { get; set; }

        public string AcceptanceCriteria { get; set; }

        public decimal? AcceptanceCriteriaFrom { get; set; }

        public decimal? AcceptanceCriteriaTo { get; set; }

        public string PlaceHolder { get; set; }

        public string ResultFlag { get; set; }

        public FormulaDependentList Lst { get; set; }

        public FormulaDependentList ImpurityLst { get; set; }

        public string FormulaType { get; set; }

        public string FormulaValue { get; set; }

        public string Description { get; set; }
    }

    public class GetFormualDetails
    {
        public int SamAnaTestID { get; set; }

        public string SourceCode { get; set; }

        public int DetailID { get; set; }

        public string InitTime { get; set; }
    }

    public class EffectedInputFileds
    {
        public int detailID { get; set; }

        public string Value { get; set; }

        public string UpdateFlag { get; set; }
    }

    public class EffectedInputFiledLists : List<EffectedInputFileds> { }

    public class GetSavedInputData
    {
        public string InitTime { get; set; }

        public string ReturnFlag { get; set; }

        public string Value { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string InitialValue { get; set; }

        public string PassOrFail { get; set; }

        public EffectedInputFiledLists Lst { get; set; }

        public TableResultExecList SysFormulas { get; set; }
    }

    public class ConfirmRawData
    {
        public int SamAnaTestID { get; set; }

        public string SourceCode { get; set; }

        public string InitTime { get; set; }

        public string EntityCode { get; set; }
    }

    public class GetConfData
    {
        public string ReturnFlag { get; set; }

        public string InitTime { get; set; }

        public string ReferenceNumber { get; set; }

        public string LoginID { get; set; }

        public string DeptCode { get; set; }

        public string RoleName { get; set; }

        public string PlantCode { get; set; }

        public string EntityCode { get; set; }

        public DateTime RawConfOn { get; set; }

        public SIngleBOList List { get; set; }
    }

    public class GetTestInstrumentDetails
    {
        public string ReturnFlag { get; set; }

        public bool? HasPrimaryOcc { get; set; }

        public int? UserArdsExecID { get; set; }

        public string InitTime { get; set; }
    }

    public class GetRRTValuesBO
    {
        public GetTestInstrumentDetails Trn { get; set; }

        public GetSampleTestRRTValuesBOList List { get; set; }
    }

    public class DeleteInstrumentBO
    {
        public string EncSamInstrID { get; set; }

        public int SamInstrID { get { return CommonStaticMethods.Decrypt<int>(EncSamInstrID); } }

        public string InitTime { get; set; }

        public string EntityCode { get; set; }

        public string Remarks { get; set; }

    }

    [XmlType("ITEM")]
    public class IncludeExcludeTestBO
    {
        public int ID { get; set; }

        public string TestInitTime { get; set; }

        public string PassOrFail { get; set; }
    }

    [XmlType("RT")]
    public class IncludeExcludeTestBOList : List<IncludeExcludeTestBO> { }

    public class IncludeExcludeTestBOItems
    {
        public IncludeExcludeTestBOList List { get; set; }

        public string XMLString { get { return CommonStaticMethods.Serialize<IncludeExcludeTestBOList>(List); } }

        public string EntityCode { get; set; }

        public string SourceCode { get; set; }
    }

    public class GetIncludeExcludeTestBO
    {
        public string ReturnFlag { get; set; }

        public IncludeExcludeTestBOList List { get; set; }
    }




    [XmlType("RT")]
    public class SDMSDetailsBO
    {
        public int SpecTestID { get; set; }

        public string DataSource { get { return "FILE"; } }

        public string ReferenceNumber { get; set; }

        public string BatchNumber { get; set; }

        public string InstrumentID { get; set; }

        public string AnalystName { get; set; }

        public string DateAnalyzed { get; set; }

        public string SampleName { get; set; }

        public string Preparation { get; set; }

        public string MethodName { get; set; }

        public string ReportTitle { get; set; }

        public List<SDMSKeyValue> DetailsList { get; set; }

        public string DateProcessed { get; set; }

        public DataTable jsonTableInfo { get; set; }

        public TransResults Trn { get; set; }

    }

    [XmlType("ITEM")]
    public class SDMSKeyValue
    {
        public string Key { get; set; }

        public string Value { get; set; }

    }
    public class SDMSInputValuesBO
    {
        public DateTime DateReceived { get; set; }

        public string XMLData { get; set; }

        public SDMSDetailsBO obj { get; set; }

        public int? SamAnaTestID { get; set; }

        public string DataProcessed { get; set; }
    }

    public class SDMSInputValuesBOList : List<SDMSInputValuesBO> { }

    public class ManageSDMSInputDetailsBO
    {
        public string EncSamAnaTestID { get; set; }

        public string InitTime { get; set; }

        public int SamAnaTestID { get { return CommonStaticMethods.Decrypt<int>(EncSamAnaTestID); } }

        public ManageSDMSInputValuesBOList List { get; set; }

        public string XMLValues { get { return CommonStaticMethods.Serialize<ManageSDMSInputValuesBOList>(List); } }

        public string Source { get; set; }
    }

    [XmlType("ITEM")]
    public class ManageSDMSInputValuesBO
    {
        public string KeyName { get; set; }

        public string Value { get; set; }

        public int DetailID { get; set; }

        public int? SdmsID { get; set; }

        [XmlIgnore]
        public string UpdateFlag { get; set; }
    }
    [XmlType("RT")]
    public class ManageSDMSInputValuesBOList : List<ManageSDMSInputValuesBO> { }

    public class GetSDMSInputDetailsBO
    {
        public TransResultApprovals Trn { get; set; }

        public ManageSDMSInputValuesBOList List { get; set; }
    }

    public class GetInstrumentsForTestBO
    {
        public string EncSamAnalTestID { get; set; }

        public int SamAnalTestID { get { return CommonStaticMethods.Decrypt<int>(EncSamAnalTestID); } }

        public string EntityCode { get; set; }
    }

    public class GetSampleAnaTestBO
    {
        public string EncSampleAnaTestID { get; set; }

        public int SampleAnaTestID { get { return CommonStaticMethods.Decrypt<int>(EncSampleAnaTestID); } }

        public string EntityCode { get; set; }

        public string SourceCode { get; set; }
    }

    public class GetInstrumentDetailsByIDBO
    {
        public string EncSamAnaInstID { get; set; }

        public int SamAnaInstID { get { return CommonStaticMethods.Decrypt<int>(EncSamAnaInstID); } }

        public string EntityCode { get; set; }

        public string EncCalibMainID { get; set; }

        public int CalibMainID { get { return CommonStaticMethods.Decrypt<int>(EncCalibMainID); } }
    }

    public class GetMappingInfo
    {
        public int CurrentSamAnaTestID { get; set; }

        public int SamAnaTestID { get; set; }

        public int SdmsID { get; set; }

        public bool IsGetARDSExecDetails { get; set; }
    }

    public class TestDetails
    {
        public string ProductName { get; set; }

        public string InvBatchNumber { get; set; }

        public string Stage { get; set; }

        public string ArNumber { get; set; }

        public string SampleNumber { get; set; }

        public string SpecNumber { get; set; }

        public string ArdsNo { get; set; }

        public string ResultTo { get; set; }

        public string Result { get; set; }

        public string SampleType { get; set; }

        public string DataProcessed { get; set; }

    }

    public class GetAnalysisTypes
    {
        public int AnalysisTypeID { get; set; }

        public string AnalysisType { get; set; }
    }

    public class GetAnalysisTypesList : List<GetAnalysisTypes> { }

    public class GetSampleSources
    {
        public int SourceID { get; set; }

        public string SourceCode { get; set; }

        public string SourceValue { get; set; }
    }

    public class GetSampleSourcesList : List<GetSampleSources> { }

    public class ContainerWiseMaterials : PagerBO
    {
        public int ContainerWiseMatID { get; set; }

        public int MaterialID { get; set; }

        public int AnalysisTypeID { get; set; }

        public string SampleSourceCode { get; set; }

        public string Type { get; set; }

        public string ResultFlag { get; set; }
    }

    public class GetContainerWiseMaterials
    {
        public int ContainerWiseMatID { get; set; }

        public int MatID { get; set; }

        public string MaterialName { get; set; }

        public int AnalysisTypeID { get; set; }

        public string AnalysisType { get; set; }

        public string SampleSource { get; set; }

        public string SampleSourceCode { get; set; }

        public int StatusID { get; set; }

        public string Status { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool IsActive { get; set; }


    }

    public class ContainerWiseMaterialsResults
    {
        public SearchResults<GetContainerWiseMaterials> List { get; set; }

        public string ResultFlag { get; set; }
    }

    public class ManageIsFinalFormulaBO
    {
        public int DetailID { get; set; }

        public int SamAnaTestID { get; set; }

        public string ARDSSourceCode { get; set; }

        public string InitTime { get; set; }

        public string Formula { get; set; }

        public int SpecTestID { get; set; }

        public string Type { get; set; }

        public string Comments { get; set; }

        public int ImpurityValueID { get; set; }
    }

    public class ManageUpdateFormulaResultFlagBO
    {
        public string ReturnFlag { get; set; }

        public string InitTime { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public SIngleBOList Lst { get; set; }

    }

    public class GetTestByCategory
    {
        public int? SpecTestID { get; set; }

        public string FormulaResultFlag { get; set; }

        public GetAnalysisTestBOList TestList { get; set; }
    }

    public class TestCatBO
    {
        public int CatID { get; set; }

        public int DetailID { get; set; }

        public int ARDSSourceRefKey { get; set; }

        public string ARDSSourceCode { get; set; }

        public int SubCatID { get; set; }
    }

    public class GetContainerAnalysisDetails
    {
        public int SpecID { get; set; }

        public string AnalysisMode { get; set; }

        public int? SpecCatID { get; set; }

        public string TypeOfAnalysis { get; set; }

        public string AnalysisType { get; set; }

        public string TestName { get; set; }

        public string Specification { get; set; }

        public List<PackDetails> PackList { get; set; }
    }

    public class PackDetails
    {
        public int InvPackID { get; set; }

        public string PackNumber { get; set; }

        public bool IsSelected { get; set; }

        public int? ContinerAnalysisID { get; set; }

        public string StatusCode { get; set; }

        public bool? IsSkipedFromAnalysis { get; set; }

        public bool? ShowIsSkipIcon { get { return IsSkipedFromAnalysis; } }

        public string PackAnalysisStatus { get; set; }
    }

    public class SaveContainerArdsDetails
    {
        public int SpecID { get; set; }

        public string ArdsMode { get; set; }

        public string Type { get; set; }

        public int TestID { get; set; }

        public SIngleBOList List { get; set; }

        public string PackXml { get { return CommonStaticMethods.Serialize<SIngleBOList>(List); } }

        public string InitTime { get; set; }

        public string EncSioID { get; set; }

        public int SioID { get { return CommonStaticMethods.Decrypt<int>(EncSioID); } }

        public int TrackID { get; set; }

        public string DocPath { get; set; }

        public int DmsID { get; set; }

        public string SectionCode { get; set; }

        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public string EntityCode { get; set; }

        public string EncArdsID { get; set; }

        public int ArdsID { get { return CommonStaticMethods.Decrypt<int>(EncArdsID); } }

        public string ReqType { get; set; }

        public string Role { get; set; }
    }

    [XmlType("ITEM")]
    public class ExtraneousTest
    {
        public int TestID { get; set; }

        public int TemplateID { get; set; }
    }

    [XmlType("RT")]
    public class ExtraneousTestList : List<ExtraneousTest> { }

    public class SamplerDetails
    {
        public int? ReviewedBy { get; set; }

        public int SamplerID { get; set; }

        public string EqpCode { get; set; }

        public DateTime SampledReceivedOn { get; set; }

        public string InvBatchNumber { get; set; }

        public string ProductName { get; set; }

        public string SampledBy { get; set; }

        public int SioID { get; set; }

        public int UserRoleID { get; set; }

        public int DeptID { get; set; }

        public short PlantID { get; set; }

        public int RoleID { get; set; }

        public int UserID { get; set; }

        public string PlantCode { get; set; }

        public int BlockID { get; set; }

        public string BlockName { get; set; }

        public string SamplerTitle { get; set; }
    }

    public class SendForReview
    {
        public int ArdsExecID { get; set; }

        public string EntityCode { get; set; }

        public string TestInitTime { get; set; }

        public IncludeExcludeTestBOList Lst { get; set; }

        public string Packlist { get { return CommonStaticMethods.Serialize<IncludeExcludeTestBOList>(Lst); } }
    }

    public class GetArdsAssignDoc
    {
        public string EncEntActID { get; set; }

        public int EntActID { get { return CommonStaticMethods.Decrypt<int>(EncEntActID); } }

        public int SpecID { get; set; }

        public int CalibParamID { get; set; }

        public string SourceCode { get; set; }
    }

    public class UploadMergeFile
    {
        public int EntActID { get; set; }

        public string EntityCode { get; set; }

        public string PlantCode { get; set; }

        public string SectionCode { get; set; }

        public string DeptCode { get; set; }

        public string LoginID { get; set; }

        public string Role { get; set; }

        public string AppCode { get; set; }

        public string ReferenceNumber { get; set; }

        public SIngleBOList List { get; set; }

        public string FileName { get; set; }

        public string InsertSection { get; set; }

        public string TypeCode { get; set; }

        public string EncEntActID { get; set; }
    }

    public class ArdsReviewBO
    {
        public int ReviewID { get; set; }

        public string ReviewedBy { get; set; }

        public DateTime ReviewedOn { get; set; }

        public int? InvalidationID { get; set; }

        public int TabID { get; set; }

        public string ReviewComment { get; set; }

        public string TestInitTime { get; set; }

        public string ReturnFlag { get; set; }
    }

    public class ArdsReviewList : List<ArdsReviewBO> { }

    public class ManageArdsReview
    {
        public int ArdsExecID { get; set; }

        public string ArdsSourceCode { get; set; }

        public int TabID { get; set; }

        public string Commnet { get; set; }

        public string InitTime { get; set; }
    }

    public class SkipPacksBO
    {
        public string EncSioID { get; set; }

        public int sioID { get { return CommonStaticMethods.Decrypt<int>(EncSioID); } }

        public string InitTime { get; set; }

        public SIngleBOList List { get; set; }

        public string PackXml { get { return CommonStaticMethods.Serialize<SIngleBOList>(List); } }
    }

    public class PacksSendToReview
    {
        public int ArdsExecID { get; set; }

        public string PackNumber { get; set; }

        public string TestInitTime { get; set; }
    }

    public class ManageSTPCommonData
    {
        public int SourceArdsExecID { get; set; }

        public int ArdsExecID { get; set; }

        public string EntityCode { get; set; }

        public string InitTime { get; set; }

    }

    public class ManageImpurityBasicInfoBO
    {
        public string ImpurityName { get; set; }

        public string EncImpurityID { get; set; }

        public int ImpurityID { get; set; }

        public int ArdsExecID { get; set; }
    }

    public class GetUnknownImpuritiesBO
    {
        public ImpurityBOList ImpList { get; set; }

        public InjectionsBOList InjList { get; set; }

        public UnknownImpuritiesDataBOList UnKIList { get; set; }

        public InputTypeList InputTypeList { get; set; }
    }

    public class ImpurityBO
    {
        public int ImpurityID { get; set; }

        public string ImpurityName { get; set; }

        public int? KnownImpID { get; set; }
    }
    public class ImpurityBOList : List<ImpurityBO> { }

    public class InjectionsBO
    {
        public int InjectionID { get; set; }

        public string InjectionName { get; set; }
    }
    public class InjectionsBOList : List<InjectionsBO> { }

    public class UnknownImpuritiesDataBO
    {
        public int UnknownImpurityID { get; set; }

        public int ImpurityID { get; set; }

        public int InjectionID { get; set; }

        public string UnknownImpurityName { get; set; }

        public string ImpurityType { get; set; }

        public decimal? ImpurityArea { get; set; }

        public decimal? ImpurityPArea { get; set; }

        public int? KnownImpID { get; set; }

        public decimal? RtRatio { get; set; }
    }
    public class UnknownImpuritiesDataBOList : List<UnknownImpuritiesDataBO> { }

    public class InputTypeBO
    {
        public string InputType { get; set; }
    }

    public class InputTypeList : List<InputTypeBO> { }

    [XmlType("item")]
    public class ManageUnKnownImpSDMSDetailsBO
    {
        public int UnknownImpurityID { get; set; }

        public decimal ImpurityArea { get; set; }

        public decimal ImpurityPArea { get; set; }

        public string ImpurityName { get; set; }

        public string ImpurityType { get; set; }

        public decimal RtRatio { get; set; }

    }
    [XmlType("rt")]
    public class ManageUnKnownImpSDMSDetailsBOList : List<ManageUnKnownImpSDMSDetailsBO> { }

    public class DynamicFormulaExecBO
    {
        public DynamicFormulaLst FormulaLst { get; set; }

        public FormulaDependentList FormulaDepenLst { get; set; }

        public ImpurityDetailsList ImpList { get; set; }

        public string Code { get; set; }

        public string Formula { get; set; }

        public string ActualValue { get; set; }

        public string Value { get; set; }

        public string IsAcceptanceCriteriaApp { get; set; }

        public string AcceptanceCriteria { get; set; }

        public decimal? AcceptanceCriteriaFrom { get; set; }

        public decimal? AcceptanceCriteriaTo { get; set; }

        public string PlaceHolder { get; set; }

        public string FormulaType { get; set; }

        public string ResultFlag { get; set; }
    }

    public class DynamicFormulaBO
    {
        public int DetailID { get; set; }

        public string Description { get; set; }
    }

    public class DynamicFormulaLst : List<DynamicFormulaBO> { }

    [XmlType("item")]
    public class ImpurityDetailsBO
    {
        public int FormulaID { get; set; }

        public int? InputID { get; set; }

        public string Value { get; set; }

        public string ItemDescription { get; set; }

        public int? SourceRefKey { get; set; }

        public int DetailsID { get; set; }

        public string Code { get; set; }

        public string InputType { get; set; }
    }

    [XmlType("rt")]
    public class ImpurityDetailsList : List<ImpurityDetailsBO> { }

    public class PostExecDynamicFormula
    {
        public int ImpurityValueID { get; set; }

        public ImpurityDetailsList ValueLst { get; set; }

        public string ValueXml { get { return CommonStaticMethods.Serialize<ImpurityDetailsList>(ValueLst); } }
    }

    public class SwitchArdsMode
    {
        public int ArdsExecID { get; set; }

        public string ArdsMode { get; set; }

        public string TestInitTime { get; set; }

        public string EntityCode { get; set; }

        public string SourceCode { get; set; }
    }

    public class GetBolckList
    {
        public string Type { get; set; }

        public string DeptCode { get; set; }
    }

    [XmlType("ITEM")]
    public class ManageColumnsBO
    {
        public int ColumnID { get; set; }

        public short? NoOfInjections { get; set; }

        public int? CumulativeInj { get; set; }

        public string Remarks { get; set; }

        public string DataSeqFile { get; set; }

        public int ColumnInjectionID { get; set; }

        public string EqpCode { get; set; }

        public string EntityCode { get; set; }

        public string EncEnityActID { get; set; }

        public int EnityActID { get { return CommonStaticMethods.Decrypt<int>(EncEnityActID); } }

        public string RefNo { get; set; }

        public int EpqOccID { get; set; }
    }

    [XmlType("RT")]
    public class ColumnsList : List<ManageColumnsBO> { }

    public class ConfirmImpMapping
    {
        public string MappingType { get; set; }

        public bool IsConfirm { get; set; }

        public int ArdsExecID { get; set; }
    }

    public class GetTableResultSets
    {
        public int ResultSetID { get; set; }

        public string Title { get; set; }

        public string ResultSetType { get; set; }

    }

    public class TableResultSetsList : List<GetTableResultSets> { }

    public class TableResultExecBO
    {
        public long RowID { get; set; }

        public int DetailID { get; set; }

        public string Code { get; set; }

        public string SysCode { get; set; }

        public string Description { get; set; }

        public string CalcFormula { get; set; }

        public string InputValue { get; set; }

        public string Status { get { if (string.IsNullOrEmpty(InputValue)) return "Pending"; else return "Success"; } }

        public string FormulaResultFlag { get; set; }

        public string Comments { get; set; }

        public string Type { get; set; }

        public string PrevType { get { return Type; } }

        public int? SpecTestID { get; set; }

        public int? ImpurityValueID { get; set; }
    }

    public class TableResultExecList : List<TableResultExecBO> { }

    public class GetTableResultSetExec
    {
        public string ResultFlag { get; set; }

        public TableResultExecList Table1 { get; set; }

        public TableResultExecList Table2 { get; set; }
    }

    public class GetSDMSDataBO
    {
        public int SDMSID { get; set; }

        public DateTime DateReceived { get; set; }

        public string ReportTitle { get; set; }

        public string DataSource { get; set; }

        public string PrepRunNo { get; set; }
    }
    public class GetSDMSDataBOList : List<GetSDMSDataBO> { }

    public class ManageSDMSDataBO
    {
        public string EncSamAnaTestID { get; set; }

        public int SamAnaTestID { get { return CommonStaticMethods.Decrypt<int>(EncSamAnaTestID); } }

        public SIngleBOList List { get; set; }

        public string Type { get; set; }

        public string Remarks { get; set; }

        public string XMLList { get { if (List.Count > 0) return CommonStaticMethods.Serialize<SIngleBOList>(List); else return string.Empty; } }

        public string EntityCode { get; set; }

        public string ReferenceNumber { get; set; }

        public int SdmsID { get; set; }

    }

    public class GetSDMSDataDetailsBO
    {
        public string ResultFlag { get; set; }

        public string ReferenceNumber { get; set; }

        public GetSDMSDataBOList List { get; set; }

        public SIngleBOList InvalidList { get; set; }
    }



    public class SDMSFileBO
    {
        public int SdmsID { get; set; }

        public int SpecTestID { get; set; }

        public string FileName { get; set; }

        public string AppCode { get; set; }

        public string PlantCode { get; set; }

        public string EntityCode { get; set; }

        public string DeparmentCode { get; set; }

        public string LoginID { get; set; }

        public string ReferenceNumber { get; set; }

        public string Role { get; set; }

        public string SectionCode { get; set; }

        public int EntityActID { get; set; }

        public string XMLSDMSID { get; set; }

        public string Type { get; set; }

        public string Remarks { get; set; }
    }

    public class ChangeTestType
    {
        public int ArdsExecID { get; set; }

        public string TestInitTime { get; set; }

        public string TestType { get; set; }
    }
}
