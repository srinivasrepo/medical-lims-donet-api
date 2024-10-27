using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.CommonMethods;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.OOS
{

        public class SearchOOSTestDetails : PagerBO
    {
        public int CategoryID { get; set; }

        public int SubCatID { get; set; }

        public int MaterialID { get; set; }

        public int OosNumberFrom { get; set; }

        public int OosNumberTo { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int BatchNumber { get; set; }

        public int TestID { get; set; }

        public int SpecificationID { get; set; }

        public int ProjectID { get; set; }

        public short MoleculeType { get; set; }

        public int BuildID { get; set; }

        public int StageID { get; set; }

        public int StatusID { get; set; }
    }

    public class GetSearchOOSTestDetails
    {
        public int OosTestID { get; set; }

        public string EncOosTestID { get { return CommonStaticMethods.Encrypt(OosTestID.ToString()); } }

        public string OosNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public string MatName { get; set; }

        public string MatCode { get; set; }

        public string MaterialName { get { return string.Format("{0} ({1})", MatName, Convert.ToString(MatCode)); } }

        public string BatchNumber { get; set; }

        public string SpecNumber { get; set; }

        public string TestName { get; set; }

        public string SpecLimit { get; set; }

        public string Result { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string UnitSymbol { get; set; }
    }

    public class TestInfo
    {
        public int SamAnaTestID { get; set; }

        public int MaterialID { get; set; }

        public string Material { get; set; }

        public string MatCode { get; set; }

        public string BatchNumber { get; set; }

        public int SpecID { get; set; }

        public string SpecNumber { get; set; }

        public string TestTitle { get; set; }

        public string SpecDesc { get; set; }

        public string Result { get; set; }

        public string UnitSymbol { get; set; }

        public string Summary { get; set; }

        public int? InstID { get; set; }

        public string EquipmentTitle { get; set; }

        public string ArNum { get; set; }

        public string Stage { get; set; }

        public string AnalysisBy { get; set; }

        public DateTime AnalysisOn { get; set; }

        public string UpdatedBy { get; set; }

        public string ProductName { get; set; }

        public decimal? LowerLimit { get; set; }

        public decimal? UpperLimit { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string LimitType { get; set; }

        public int? SpecTestID { get; set; }

        public string AdnlSamples { get; set; }

        public int OosProcessID { get; set; }

        public string OosProcessName { get; set; }

        public string OosProcessCode { get; set; }

        public string CheckedBy { get; set; }

        public int IsMiscellaneous { get; set; }

        public bool IsApplicable { get; set; }

        public int? TemplateID { get; set; }

        public string TestStatus { get; set; }

        public string ProjectName { get; set; }

        public int? OosTestID { get; set; }

        public string EncOosTestID { get { return CommonStaticMethods.Encrypt(OosTestID.ToString()); } }

        public string MatCatCode { get; set; }

        public int DateDifference { get; set; }

        public string CustomerName { get; set; }

        public string DescNotification { get; set; }

        public bool IsCustInfFromOosApp { get; set; }

        public DateTime? NotificationDate { get; set; }

        public byte NoOfApprovals { get; set; }

        public int? DevID { get; set; }

        public string AppVersion { get; set; }

        public string OosNumber { get; set; }

        public bool ShowQAJustificationDelay { get; set; }

        public GetPhaseDetailsBOList PhaseList { get; set; }

        public RecordActionDetails Act { get; set; }
    }

    public class GetPhaseDetailsBO
    {
        public int OOSTestDetID { get; set; }

        public string EncOOSTestDetID { get { return CommonStaticMethods.Encrypt(OOSTestDetID.ToString()); } }


        public string PhaseTitle { get; set; }

        public string PhaseType { get; set; }
    }
    public class GetPhaseDetailsBOList : List<GetPhaseDetailsBO> { }

    public class UpdateSummary
    {
        public string EncOosTestID { get; set; }

        public int OosTestID { get { return CommonStaticMethods.Decrypt<int>(EncOosTestID); } }

        public string Summary { get; set; }
    }

    public class GetOOSPhase1CheckList
    {
        public int PhaseID { get; set; }

        public string PhaseTitle { get; set; }

        public List<GetQCOOSCheckList> GetQCOOSCheckList { get; set; }

        public List<GetCheckListQuestions> GetCheckListQuestions { get; set; }

        public QCOosTestDetails QCOosTestDetails { get; set; }

        public OosProcessCondition OosProcessCondition { get; set; }

        public List<OosProcessList> OosProcessList { get; set; }
    }

    public class GetQCOOSCheckList
    {
        public int? CheckCatID { get; set; }

        public string Category { get; set; }

        public byte CatOrderBy { get; set; }

        public int OOSDetID { get; set; }
    }

    public class GetCheckListQuestions
    {
        public int QuestionID { get; set; }

        public int? CheckCatID { get; set; }

        public string Question { get; set; }

        public string ShowResult { get; set; }

        public string Answer { get; set; }

        public string Remarks { get; set; }

        public long Sno { get; set; }
    }

    public class QCOosTestDetails
    {
        public string PhaseStatusID { get; set; }

        public string Condition { get; set; }

        public bool? PhaseComplited { get; set; }

        public string Remarks { get; set; }

        public string PhaseStatusCode { get; set; }

        public bool AppComplited { get; set; }

        public string JustificationError { get; set; }

        public string ObviousRootCause { get; set; }

        public string RootCauseRelatedTo { get; set; }

        public string PraposedCapa { get; set; }

        public bool CanShowCapa { get; set; }
    }

    public class OosProcessCondition
    {
        public string ConditionCode { get; set; }

        public string PhaseStatusIDCode { get; set; }
    }

    public class OosProcessList
    {
        public string ClOption { get; set; }

        public string ClopCode { get; set; }
    }

    public class OOSProcessItem
    {

        public string EncOOSTestID { get; set; }

        public int OOSTestID { get { return CommonStaticMethods.Decrypt<int>(EncOOSTestID); } }

        public string EncOOSTestDetailID { get; set; }

        public int OOSTestDetailID { get { return CommonStaticMethods.Decrypt<int>(EncOOSTestDetailID); } }

        public byte Count { get; set; }

        public string Status { get; set; }

        public string Validity { get; set; }

        public string Remarks { get; set; }

        public ChecklistAns Lst { get; set; }

        public string CheckList { get { return CommonStaticMethods.Serialize<ChecklistAns>(Lst); } }

        public bool IsMisc { get; set; }

        public string DocXml { get; set; }

        public string JustificationToSkip { get; set; }

        public string CorrectError { get; set; }

        public string CorrectiveAction { get; set; }

        public string AnalysisProposal { get; set; }

        public string AnalysisJustification { get; set; }

        public string RootCauseRelatedTo { get; set; }

        public string ObviousRootcause { get; set; }

        public int PhaseID { get; set; }

        public string ProposeCapa { get; set; }

    }

    [XmlType("ITEM")]
    public class ChecklistAnswer
    {
        public int QuestionID { get; set; }

        public string Answer { get; set; }

        public string Remarks { get; set; }
    }

    [XmlType("RT")]
    public class ChecklistAns : List<ChecklistAnswer> { }

    public class OOSGetHypoTestingInfo
    {
        public string EncOOSTestID { get; set; }

        public int OOSTestID { get { return CommonStaticMethods.Decrypt<int>(EncOOSTestID); } }

        public string EncOOSTestDetailID { get; set; }

        public int OOSTestDetailID { get { return CommonStaticMethods.Decrypt<int>(EncOOSTestDetailID); } }

        public string ConditionCode { get; set; }

    }

    public class GetOOSHypoTestingResult
    {
        public string Remarks { get; set; }

        public string PhaseTitle { get; set; }

        public bool PhaseCompleted { get; set; }

        public string PhaseType { get; set; }

        public string ActionStatus { get; set; }

        public string AnalysisProposal { get; set; }

        public string AnalysisJustification { get; set; }

        public bool ApprovalCompleted { get; set; }

        public string ActionValidity { get; set; }

        public string RootCauseRelatedTo { get; set; }

        public List<OOSHypoTestingPhasesDetails> OOSHypoTestingPhasesDetails { get; set; }

        public List<OOSPhasesMaster> OOSPhasesMaster { get; set; }

        public List<OOSPhasesCondition> OOSPhasesCondition { get; set; }
    }

    public class OOSHypoTestingPhasesDetails
    {
        public int OOSHypoTestingID { get; set; }

        public int PhaseID { get; set; }

        public string PhaseTitle { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

    }

    public class OOSPhasesMaster
    {
        public string PhaseTitle { get; set; }

        public int PhaseID { get; set; }
    }

    public class OOSPhasesCondition
    {
        public string Condition { get; set; }
    }

    public class OOSManageHypoTestingPhases : OOSGetHypoTestingInfo
    {
        public int HypoTestPhaseID { get; set; }

        public int PhaseID { get; set; }

        public string Action { get; set; }

    }

    public class OOSManageHypoResults
    {
        public string ResultFlag { get; set; }

        public List<OOSHypoTestingPhasesDetails> OOSHypoTestingPhasesDetails { get; set; }
    }

    public class TestingSameSample
    {
        public string PhaseStatus { get; set; }

        public string Condition { get; set; }

        public bool? PhaseCompleted { get; set; }

        public string Remarks { get; set; }

        public string OtherDeptName { get; set; }

        public bool AllComments { get; set; }

        public bool ApprovalCompleted { get; set; }

        public bool? IsMisc { get; set; }

        public List<OosProcessCondition> ConditionList { get; set; }
    }



    public class GetOOSSingleAndCatBDetails
    {

        public string Remarks { get; set; }

        public string JustificationToSkip { get; set; }

        public string CorrectError { get; set; }

        public string CorrectiveAction { get; set; }

        public string ActionValidity { get; set; }

        public string PhaseType { get; set; }

        public string PhaseStatus { get; set; }

        public bool PhaseCompleted { get; set; }

    }

    public class GetDeptReviewData
    {
        public string Remarks { get; set; }

        public string ActionValidity { get; set; }

        public string OtherDeptName { get; set; }

        public bool IsReviewCompleted { get; set; }

        public bool PhaseCompleted { get; set; }

        public List<OOSDeptList> DeptList { get; set; }
    }

    public class OOSDeptList
    {
        public int ModuleID { get; set; }

        public string ModuleName { get; set; }

        public bool IsSelected { get; set; }

        public bool DefaultSelection { get; set; }
    }

    public class OOSDeptReview
    {
        public string EncOOSTestDetailID { get; set; }

        public int OOSTestDetailID { get { return CommonStaticMethods.Decrypt<int>(EncOOSTestDetailID); } }

        public string Remarks { get; set; }

        public string OthDeptName { get; set; }

        public string Validity { get; set; }

        public string Status { get; set; }

        public SIngleBOList List { get; set; }

        public string DeptListXml { get { return CommonStaticMethods.Serialize<SIngleBOList>(List); } }
    }

    [XmlType("ITEM")]
    public class GetDepartmentWiseReviews
    {
        public int ReviewDeptID { get; set; }

        public string Comments { get; set; }

        public string DeptCode { get; set; }

        public bool IsConfirmed { get; set; }

        public string DeptName { get; set; }

        public string UserDeptCode { get; set; }
    }

    [XmlType("RT")]
    public class DeptReviewList : List<GetDepartmentWiseReviews> { }

    public class ManageDeptCommets
    {
        public string EncOOSTestDetailID { get; set; }

        public int OOSTestDetailID { get { return CommonStaticMethods.Decrypt<int>(EncOOSTestDetailID); } }

        public string InitTime { get; set; }

        public DeptReviewList List { get; set; }

        public string DeptReviewXml { get { return CommonStaticMethods.Serialize<DeptReviewList>(List); } }
    }

    public class ManufactureChecklist
    {
        public int CategoryID { get; set; }

        public string Category { get; set; }

        public int PhaseType { get; set; }
    }

    public class ManufactureCheckPoints
    {

        public List<GetQCOOSCheckList> GetQCOOSCheckList { get; set; }

        public List<GetCheckListQuestions> GetCheckListQuestions { get; set; }
    }

    public class MfgInvestigationDetails
    {
        public bool CanShowCAPA { get; set; }

        public int? PhaseID { get; set; }

        public string Remarks { get; set; }

        public string ProposedCapa { get; set; }

        public bool PhaseCompleted { get; set; }
    }

    public class ManageQASummaryInfo
    {
        public string EncOOSTestID { get; set; }

        public int OOSTestID { get { return CommonStaticMethods.Decrypt<int>(EncOOSTestID); } }

        public string InitTime { get; set; }

        public int CustomerID { get; set; }

        public string IsQualityAgreement { get; set; }

        public string IsOOSNotify { get; set; }

        public string QARemarks { get; set; }

        public string CustomerName { get; set; }

        public string DescNotification { get; set; }

        public string ReasonForDelay { get; set; }
    }

    public class GetQASummaryInfo
    {
        public int? CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string QARemarks { get; set; }

        public string IsQualityAgreement { get; set; }

        public string IsOOSNotify { get; set; }

        public bool? IsNotifyFromAPP { get; set; }

        public string DescNotification { get; set; }

        public DateTime? NotificationDate { get; set; }

        public string ReasonForDelay { get; set; }

        public string DevCode { get; set; }

        public RecordActionDetails recordActions { get; set; }

    }

     public class ManageOOSSummaryInfo
    {
        public string EncOOSTestID { get; set; }

        public int OOSTestID { get { return CommonStaticMethods.Decrypt<int>(EncOOSTestID); } }

        public string InitTime { get; set; }

        public string IsLabInvestReviewed { get; set; }

        public string OtherCause { get; set; }

        public string MfgInvstDone { get; set; }

        public string RootCauseMfgInvestigation { get; set; }

        public string MfgChkAttached { get; set; }

        public string RefDevInvestigation { get; set; }

        public int? RootCauseOOS { get; set; }

        public int? DevID { get; set; }

        public string SummaryOOS { get; set; }

        public string CommentsIfAny { get; set; }

        public string PraposePrevAction { get; set; }

        public string ProcViolationJustification { get; set; }

        public string CheckListObservation { get; set; }

        public string CheckListJustification { get; set; }

        public string ReAnaObservation { get; set; }

        public string ReAnaJustification { get; set; }

        public string ConfirmAnaObservation { get; set; }

        public string ConfirmAnaJustification { get; set; }

        public string OOSInvestObservation { get; set; }

        public string OOSInvestJustification { get; set; }

        public string JustificationForDelay { get; set; }

        public string DevNumber { get; set; }

        public string CatCode { get; set; }

        public string RootCauseDesc { get; set; }

        public bool ShowJustificationDelay { get; set; }

        public List<OOSDevReq> DevReqList { get; set; }

        public string RootCauseCode { get; set; }
    }

    public class OOSDevReq
    {
        public int DEV_ID { get; set; }

        public string DEV_NUMBER { get; set; }
    }

    public class NewDeviationReg
    {
        public List<OOSDevReq> DevReqList { get; set; }

        public int? DEV_ID { get; set; }

        public string RETVAL { get; set; }
    }

    public class NewSampleRequest
    {
        public int SioID { get; set; }

        public string SioCode { get; set; }
    }
}
