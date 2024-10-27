using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.AnalystQualification
{

    public class AnalystQualificationBo
    {
        public int AnalystID { get { return CommonStaticMethods.Decrypt<int>(EncAnalystID); } }

        public string EncAnalystID { get; set; }

        public int UserRoleID { get; set; }

        public string Reason { get; set; }

        public SIngleBOList list { get; set; }

        public string XmlString { get { return CommonStaticMethods.Serialize<SIngleBOList>(list); } }

    }

    public class SearchAnalyst
    {
        public int AnalystID { get; set; }

        public string EncAnalystID { get { return CommonStaticMethods.Encrypt(AnalystID.ToString()); } }

        public string AnalystNumber { get; set; }

        public string UserRoleName { get; set; }

        public string UserName { get; set; }

        public string Reason { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Status { get; set; }

    }

    public class SearchAnalystBo : PagerBO
    {

        public int UserID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int StatusID { get; set; }

    }

    public class SearchAnalystDet
    {
        public AnalystDet Analyst { get; set; }

        public List<AnalystQualificationDet> AnalystList { get; set; }

        public RecordActionDetails Act { get; set; }
    }

    public class AnalystDet
    {
        public int AnalystID { get; set; }

        public string EncAnalystID { get { return CommonStaticMethods.Encrypt(AnalystID.ToString()); } }

        public string AnalystNumber { get; set; }

        public int UserRoleID { get; set; }

        public string UserName { get; set; }

        public string UserRoleName { get; set; }

        public string Reason { get; set; }

        public string Qualifications { get; set; }

        public string Status { get; set; }

    }

    public class AnalystQualificationDet
    {
        public int AnalystQualificationID { get; set; }

        public int QualificationID { get; set; }

    }

    public class ManageQualificationRequest
    {
        public int QualificationID { get { return CommonStaticMethods.Decrypt<int>(EncQualificationID); } }

        public string EncQualificationID { get; set; }

        public int TechniqueID { get; set; }

        public int QualificationTypeID { get; set; }

        public TestIDList List { get; set; }

        public string TestXml { get { return CommonStaticMethods.Serialize<TestIDList>(List); } }

        public int MatID { get; set; }

        public int ArID { get; set; }

        public int AnalystID { get; set; }

        public string InitTime { get; set; }

        public int ReQualificationPurposeID { get; set; }

        public string TypeCode { get; set; }

        public string ReferenceNo { get; set; }

        public SIngleBOList FileUploadedIDs { get; set; }

        public string EntityCode { get; set; }

        public string Role { get; set; }

        public int RequestTypeID { get; set; }

        public short VolumetricIndexID { get; set; }

    }

    [XmlType("TEST")]
    public class TestID
    {
        public int QualificationTestID { get; set; }

        public int SpecTestID { get; set; }

    }

    [XmlType("TESTS")]
    public class TestIDList : List<TestID> { }


    public class GetQualificationType
    {
        public int QualificationTypeID { get; set; }

        public string QualificationType { get; set; }

        public string QualificationTypeCode { get; set; }
    }

    public class GetQualificationTypeList : List<GetQualificationType> { }

    public class GetAnalysisType
    {
        public int AnalysisTypeID { get; set; }

        public string AnalysisType { get; set; }
    }

    public class GetAnalysisTypeList : List<GetAnalysisType> { }

    public class SearchTestsByTechniqueAndArID
    {
        public int TechniqueID { get; set; }

        public int ArID { get; set; }

        public string EncQualificationID { get; set; }

        public int QualificationID { get { return CommonStaticMethods.Decrypt<int>(EncQualificationID); } }

    }

    public class GetTestsByTechniqueAndArID
    {
        public int SpecTestID { get; set; }

        public int QualificationTestID { get; set; }

        public string SpecDesc { get; set; }

        public string TestTitle { get; set; }

        public string TestCode { get; set; }

        public Boolean isSelected { get; set; }

        public string GroupTest { get; set; }

        public int ? GroupSpecCatID { get; set; }

        public string DisplayName { get { return !string.IsNullOrEmpty(GroupTest) ? string.Format("{0} ({1})", TestTitle, GroupTest) : TestTitle; } }

    }

    public class GetTestsByTechniqueAndArIDList : List<GetTestsByTechniqueAndArID> { }

    public class GetQualificationRequestDetails
    {
        public int? TechniqueID { get; set; }

        public string Technique { get; set; }

        public string AnalystName { get; set; }

        public string ChemicalName { get; set; }

        public string CategoryCode { get; set; }

        public string ArNumber { get; set; }

        public int UserRoleID { get; set; }

        public int QualificationTypeID { get; set; }

        public string QualificationType { get; set; }

        public int MatID { get; set; }

        public int ? ArID { get; set; }

        public string Remarks { get; set; }

        public int? ConclusionID { get; set; }

        public int ? AnalysisTypeID { get; set; }

        public string Status { get; set; }

        public string RequestCode { get; set; }

        public string FinalStatus { get; set; }

        public string AR_NUM { get; set; }

        public int? AR_ID { get; set; }

        public string Justification { get; set; }

        public int? ReQualificationPurpose { get; set; }

        public bool IsNeedJustification { get; set; }

        public string QualificationTypeCode { get; set; }

        public string TypeCode { get; set; }

        public string MaterialCode { get; set; }

        public string ReferenceNo { get; set; }

        public int ? RequestTypeID { get; set; }

        public string RequestTypeCode { get; set; }

        public short ? VolumetricIndexID { get; set; }

        public string RequestType { get; set; }

        public int? VolSolutionID { get; set; }

        public string VersionCode { get; set; }

        public string BatchNumber { get; set; }

        public string BatchStatus { get; set; }

        public string SelectedTests { get; set; }

        public string Type { get; set; }

        public int? TypeID { get; set; }

        public string ReQualPurposeCode { get; set; }

        public string ReQualPurposeType { get; set; }

        public string Conclusion { get; set; }

        public string ConclusionCode { get; set; }
    }

    [XmlType("ITEM")]
    public class GetQualificationTestsDetails
    {
        public int QualificationTestID { get; set; }

        public string TestTitle { get; set; }

        public string PreparationResult1 { get; set; }

        public string PreparationResult2 { get; set; }

        public string PreparationResult3 { get; set; }

        public string VariationObserved { get; set; }

        public string AcceptanceCriteria { get; set; }

        public string AverageResult { get; set; }

        public string OriginalResult { get; set; }

        public string OriginalVariationObserved { get; set; }

        public string OriginalAcceptanceCriteria { get; set; }

        public string TestMethodType { get; set; }

        public string Result { get; set; }

        public string ResultTo { get; set; }

        public string OriginalTestResult { get { if (!string.IsNullOrEmpty(ResultTo)) return Result + " - " + ResultTo; else return Result; } }

        public int? AcceptanceCriteriaID { get; set; }

        public int? OriginalAcceptanceCriteriaID { get; set; }
    }


    [XmlType("RT")]
    public class QualificationTestsDetailsList : List<GetQualificationTestsDetails> { }

    public class AcceptenceCriteria
    {
        public int QualificationCriteriaID { get; set; }

        public string AcceptanceCriteria { get; set; }

    }

    public class GetQualificationDetails
    {
        public RecordActionDetails recordActionResults { get; set; }

        public GetQualificationRequestDetails GetQualificationRequestDetails { get; set; }

        public List<GetTestsByTechniqueAndArID> GetTestsByTechniqueAndArIDList { get; set; }

        public List<GetQualificationTestsDetails> GetQualificationTestsDetailsList { get; set; }

        public List<AcceptenceCriteria> GetAcceptenceCriteriaDetailsList { get; set; }

        public AnalysisResultList AnalysisResultList { get; set; }

        public SelectedCriteiaList TestCriteriaLst { get; set; }

    }

    public class AnalysisResult
    {
        public string ArNumber { get; set; }

        public string SioCode { get; set; }

        public string Result { get; set; }

        public string TestName { get; set; }

        public string ResultTo { get; set; }

        public string FinalResult { get { if (!string.IsNullOrEmpty(ResultTo)) return Result + " - " + ResultTo; else return Result; } }

        public string SpecDesc { get; set; }

        public int? ArdsExecID { get; set; }

        public int? DmsReportID { get; set; }
    }

    public class AnalysisResultList : List<AnalysisResult> { }

    public class QualificationRequest
    {
        public string Technique { get; set; }

        public string AnalystName { get; set; }

        public string QualificationType { get; set; }

        public string ChemicalName { get; set; }

        public string ArNumber { get; set; }

        public string Remarks { get; set; }

        public string Conclusion { get; set; }

        public string AnalysisType { get; set; }

        public string Status { get; set; }

        public string RequestCode { get; set; }

        public string Justification { get; set; }

        public string ReQualificationPurpose { get; set; }

        public string Type { get; set; }

        public bool ShowArNum { get; set; }

        public string MaterialCode { get; set; }

        public string Material { get { return ChemicalName + " (" + MaterialCode + ")"; } }

        public string Category { get; set; }

        public string ReferenceNo { get; set; }

        public string RequestTypeCode { get; set; }

        public string RequestType { get; set; }

        public int? VolSolutionID { get; set; }

        public string VersionCode { get; set; }

        public string BatchNumber { get; set; }

        public string BatchStatus { get; set; }

        public string ConculsionCode { get; set; }

        public string DisQualifyComments { get; set; }
    }

    public class GetQualificationDetailsForView
    {

        public QualificationRequest QualificationRequest { get; set; }

        public List<GetQualificationTestsDetails> GetQualificationTestsDetails { get; set; }

        public List<AcceptenceCriteria> GetAcceptenceCriteriaDetailsList { get; set; }

        public AnalysisResultList AnalysisResultList { get; set; }

    }



    public class SearchQualificationDetails : PagerBO
    {
        public int TechniqueID { get; set; }

        public int ConclusionID { get; set; }

        public int AnalystID { get; set; }

        public int StatusID { get; set; }

        public int MatID { get; set; }

        public int AnalysisType { get; set; }

        public int ActivityType { get; set; }

        public int ArNumberID { get; set; }

        public int SpecTestID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string FormulaType { get; set; }

        public int SioID { get; set; }

        public int InitiatedID { get; set; }

        public DateTime InitiatedDate { get; set; }

        public int QualificationID { get; set; }

    }

    public class SearchResultsQualificationDetails
    {
        public int QualificationID { get; set; }

        public string EncQualificationID { get { return CommonStaticMethods.Encrypt(QualificationID.ToString()); } }

        public string RequestCode { get; set; }

        public string QualificationType { get; set; }

        public string AnalystName { get; set; }

        public string Technique { get; set; }

        public string Status { get; set; }

        public string ArNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public string MaterialName { get; set; }

        public string Conclusion { get; set; }

        public string UserName { get; set; }

        public bool ShowDisQuify { get; set; }
    }

    public class EditQualificationDetails
    {
        public string EncQualificationID { get; set; }

        public int QualificationID { get { return CommonStaticMethods.Decrypt<int>(EncQualificationID); } }

        public int UserRoleID { get; set; }
    }

    public class ManageQualificationEvaluation
    {
        public string EncQualificationID { get; set; }

        public int QualificationID { get { return CommonStaticMethods.Decrypt<int>(EncQualificationID); } }

        public int ConclusionID { get; set; }

        public string Remarks { get; set; }

        public string Justification { get; set; }

        public string InitTime { get; set; }

        public QualificationTestsDetailsList List { get; set; }

        public string TestXml { get { return CommonStaticMethods.Serialize<QualificationTestsDetailsList>(List); } }

        public SelectedCriteiaList SelectedAcceptanceCriteiaLst { get; set; }

        public string CriteriaXml { get { return CommonStaticMethods.Serialize<SelectedCriteiaList>(SelectedAcceptanceCriteiaLst); } }
    }

    [XmlType("ITEM")]
    public class SelectedCriteiaBO
    {
        public int QualificationTestID { get; set; }

        public int AcceptanceCriteriaID { get; set; }

        public string Type { get; set; }
    }

    [XmlType("RT")]
    public class SelectedCriteiaList : List<SelectedCriteiaBO> { }

    public class DisqualifyBO
    {
        public string EncQualificationID { get; set; }

        public string Comments { get; set; }

        public int QualificationID { get { return CommonStaticMethods.Decrypt<int>(EncQualificationID); } }
    }
}
