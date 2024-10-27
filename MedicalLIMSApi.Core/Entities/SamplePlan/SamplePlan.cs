using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.SamplePlan
{

    public class PlanAnalyst
    {
        public string InitTime { get; set; }

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public string EncPlanID { get; set; }

        public SIngleBOList list { get; set; }

        public string XmlString { get { return CommonStaticMethods.Serialize<SIngleBOList>(list); } }

    }

    public class AnalystDet
    {

        public string UserName { get; set; }

        public string EmployeeCode { get; set; }

        public string RoleName { get; set; }

        public string Qualifications { get; set; }

        public int UnderAnalysis { get; set; }

        public int ToBeStart { get; set; }

        public bool IsSelected { get; set; }

        public int UserRoleID { get; set; }

        public string EncUserRoleID { get { return CommonStaticMethods.Encrypt(UserRoleID.ToString()); } }

        public bool IsAvailable { get; set; }

        public int ? AnalystID { get; set; }

        public string encAnalystID { get { return CommonStaticMethods.Encrypt(Convert.ToString(AnalystID)); } }
    }

    public class AnalystDetList : List<AnalystDet> { }

    public class AnalystOccupancyDet
    {

        public string Activity { get; set; }

        public string TestTitle { get; set; }

        public string SioCode { get; set; }

        public string ArNum { get; set; }

        public string InvalidationCode { get; set; }

        public string TestStatus { get; set; }

        public string ActivityDesc { get; set; }

        public string SpecNumber { get; set; }

        public string PlanCode { get; set; }

        public string AssignBy { get; set; }

        public DateTime? AssignOn { get; set; }

        public string InstNumber { get; set; }

        public string MntReportCode { get; set; }

        public string InstType { get; set; }

        public DateTime? SchDate { get; set; }

        public string MaterialName { get; set; }

        public string MatCode { get; set; }

        public DateTime? InalidationDate { get; set; }

        public string Material { get { return string.Format("{0} ({1})", MaterialName, MatCode); } }

        public string BatchNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public DateTime? ReviewDate { get; set; }
    }

    public class SampleDetails
    {
        public int SioID { get; set; }

        public string ArNumber { get; set; }

        public string ProductName { get; set; }

        public string SampleNumber { get; set; }

        public string BatchNumber { get; set; }

        public string AnalysisType { get; set; }

        public bool IsSelected { get; set; }

        public bool IsIncludeOtherPlan { get; set; }

        public DateTime InwardDate { get; set; }

        public string Status { get; set; }

        public string SioCode { get; set; }
    }

    public class SampleDetailsList : List<SampleDetails> { }

    public class PlanSamples
    {
        public string InitTime { get; set; }

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public string EncPlanID { get; set; }

        public SIngleBOList list { get; set; }

        public string XmlString { get { return CommonStaticMethods.Serialize<SIngleBOList>(list); } }

    }

    public class SampleDet
    {
        public int SampleID { get; set; }

        public string EncSampleID { get { return CommonStaticMethods.Encrypt(SampleID.ToString()); } }

        public int SioID { get; set; }

        public string SioCode { get; set; }

        public string ArNumber { get; set; }

        public string AnalysisType { get; set; }

        public string ProductName { get; set; }

        public string SampleNumber { get; set; }

        public string BatchNumber { get; set; }

        public DateTime InwardDate { get; set; }

        public bool HasSpec { get; set; }

        public string SampleMode { get; set; }
    }

    public class SampleSpecifications
    {

        public int SampleID { get; set; }

        public int SpecificationID { get; set; }

        public string SpecNumber { get; set; }

        public bool IsSelected { get; set; }

        public bool CanDisable { get; set; }
    }

    public class SamSpecDet
    {
        public List<SampleDet> Sam { get; set; }

        public List<SampleSpecifications> SamSpe { get; set; }
    }

    public class SampleSpecificationBO
    {
        public string InitTime { get; set; }

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public string EncPlanID { get; set; }

        public SampleSpecList list { get; set; }

        public string XmlString { get { return CommonStaticMethods.Serialize<SampleSpecList>(list); } }

    }

    [XmlType("ITEM")]
    public class SampleSpec
    {
        public int SpecificationID { get; set; }

        public int SampleID { get; set; }
    }

    [XmlType("RT")]
    public class SampleSpecList : List<SampleSpec> { }

    public class PalnDet
    {

        public string RequestCode { get; set; }

        public string Status { get; set; }
    }

    public class PalnDetails
    {
        public PalnDet plan { get; set; }

        public RecordActionDetails rec { get; set; }

    }

    public class SamplingActivity
    {
        public int SioID { get; set; }

        public string ArNumbr { get; set; }

        public string SampleNumber { get; set; }

        public bool IsSelected { get; set; }

        public short? Minutes { get; set; }

        public string ProductName { get; set; }

        public string InvBatchNumber { get; set; }

        public bool IsIncludeOtherPlan { get; set; }
    }

    public class SamplingActivityList : List<SamplingActivity> { }

    public class WetInstrumentaion
    {
        public int SpecCatID { get; set; }

        public int SioID { get; set; }

        public string ArNumber { get; set; }

        public string TestTitle { get; set; }

        public string TestType { get; set; }

        public bool IsSelected { get; set; }

        public short? Minutes { get; set; }

        public string InvBatchNumber { get; set; }

        public string ProductName { get; set; }

        public string AnalysisType { get; set; }

        public int? MasterTestID { get; set; }

        public bool IsIncludeOtherPlan { get; set; }

        public string SampleNumber { get; set; }
    }

    public class WetInstrumentaionList : List<WetInstrumentaion> { }

    public class Invalidation
    {
        public int InvalidationID { get; set; }

        public string InvalidationCode { get; set; }

        public string Status { get; set; }

        public bool IsSelected { get; set; }

        public short? Minutes { get; set; }

        public string ArNumber { get; set; }

        public string InvBatchNumber { get; set; }

        public string ProductName { get; set; }

        public DateTime InvalidationDate { get; set; }

        public bool IsIncludeOtherPlan { get; set; }

    }

    public class InvalidationsList : List<Invalidation> { }

    public class MasterTestBO
    {
        public int MasterTestID { get; set; }

        public string TestName { get; set; }
    }

    public class MasterTestList : List<MasterTestBO> { }

    public class OOSTestBO
    {
        public int OOSTestID { get; set; }

        public string OOSNumber { get; set; }

        public string Status { get; set; }

        public string ArNumber { get; set; }

        public string InvBatchNumber { get; set; }

        public DateTime OOSDate { get; set; }

        public string ProdNameCode { get; set; }

        public string TestTitle { get; set; }

        public bool IsSelected { get; set; }

        public short? Minutes { get; set; }

        public bool IsIncludeOtherPlan { get; set; }
    }

    public class OOSTestList: List<OOSTestBO> { }

    public class DataReviewBO
    {
        public int ReviewID { get; set; }

        public string RequestCode { get; set; }

        public string Status { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool IsSelected { get; set; }

        public short? Minutes { get; set; }

        public string ProductName { get; set; }

        public string TestTitle { get; set; }

        public bool IsIncludeOtherPlan { get; set; }
    }

    public class DataReviewList: List<DataReviewBO> { }

    public class CalibrationBO
    {
        public int SpecCatID { get; set; }

        public string RptNumber { get; set; }

        public string EqpCode { get; set; }

        public DateTime NextDueDate { get; set; }

        public string Category { get; set; }

        public string TestTitle { get; set; }

        public short? Minutes { get; set; }

        public int MaintRptID { get; set; }

        public bool IsSelected { get; set; }

        public bool IsIncludeOtherPlan { get; set; }

        public string ConditionCode { get; set; }
    }

    public class CalibrationList : List<CalibrationBO> { }

    public class GetTestActivityDet
    {

        public SamplingActivityList Sam { get; set; }

        public WetInstrumentaionList WetIns { get; set; }

        public InvalidationsList inv { get; set; }

        public MasterTestList MasterList { get; set; }

        public OOSTestList OosTestList { get; set; }

        public DataReviewList DrList { get; set; }

        public CalibrationList CalibList { get; set; }
    }

    public class ManageSamplingTestModel
    {
        public string InitTime { get; set; }

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public string EncPlanID { get; set; }

        public ManageSampleTestModelList SamplTestList { get; set; }

        public string SamplesXml { get { if (SamplTestList.Count > 0) return CommonStaticMethods.Serialize<ManageSampleTestModelList>(SamplTestList); else return string.Empty; } }

    }

    [XmlType("ITEM")]
    public class ManageSampleTest
    {
        public int ID { get; set; }

        public int SIOID { get; set; }

        public string CODE { get; set; }

        public short MINUTES { get; set; }

        public int MaintRptID { get; set; }

    }

    [XmlType("RT")]
    public class ManageSampleTestModelList : List<ManageSampleTest> { }

    public class PlanDetails
    {
        public int? UserTestID { get; set; }

        public string EncUserTestID { get { return CommonStaticMethods.Encrypt(UserTestID.ToString()); } }

        public string ArNumber { get; set; }

        public string Activity { get; set; }

        public string TestTitle { get; set; }

        public string ActivityDesc { get; set; }

        public int? UserRoleID { get; set; }

        public string ActivityCode { get; set; }

        public int TestID { get; set; }

        public string ActivityDescription { get; set; }

        public string ReferenceNumber { get; set; }

        public string MaterialName { get; set; }

        public int? ActivityStatusID { get; set; }

        public string ActivityStatus { get; set; }

        public string Comment { get; set; }

        public string ActivityStatusCode { get; set; }
    }

    public class UserPalnDetails
    {
        public int UserRoleID { get; set; }

        public string EncUserRoleID { get { return CommonStaticMethods.Encrypt(UserRoleID.ToString()); } }

        public string UserName { get; set; }

        public int Planned { get; set; }

        public int Onging { get; set; }

        public int CurrentPlan { get; set; }

        public int Total { get; set; }

        public int? OpenPlanOCC { get; set; }

        public int? CurrentPlanOCC { get; set; }

        public int UserID { get; set; }
    }

    public class AutoPalnDetails
    {

        public List<PlanDetails> Plandet { get; set; }

        public List<UserPalnDetails> UserPlanDet { get; set; }
    }

    public class ChangeAnalyst
    {

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public string EncPlanID { get; set; }

        public int TestID { get; set; }

        public int UserRoleID { get; set; }

        public string InitTime { get; set; }

    }

    public class TestInfo
    {

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public string EncPlanID { get; set; }

        public string EncUserTestID { get; set; }

        public int UserTestID { get { return CommonStaticMethods.Decrypt<int>(EncUserTestID); } }

        public string InitTime { get; set; }

        public int TestID { get; set; }

        public ManageSampleTestModelList SampleLst { get; set; }

        public string SamplesXml { get { if (SampleLst.Count > 0) return CommonStaticMethods.Serialize<ManageSampleTestModelList>(SampleLst); else return string.Empty; } }

    }

    public class PlanActivities
    {
        public int UserTestID { get; set; }

        public string EncUserTestID { get { return CommonStaticMethods.Encrypt(UserTestID.ToString()); } }

        public string ArNumber { get; set; }

        public string Activity { get; set; }

        public string TestTitle { get; set; }

        public string ActivityDesc { get; set; }

        public int UserRoleID { get; set; }

        public string ActivityCode { get; set; }

        public string CanUnAssign { get; set; }

        public string ActivityDescription { get; set; }

        public string ReferenceNumber { get; set; }

        public string MaterialName { get; set; }

        public int TestID { get; set; }

    }

    public class PlanActivitiesList : List<PlanActivities> { }

    public class UserTestBo
    {

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public string EncPlanID { get; set; }

        public int UserTestID { get { return CommonStaticMethods.Decrypt<int>(EncUserTestID); } }

        public string EncUserTestID { get; set; }

        public string InitTime { get; set; }

    }

    public class PlanActivityBo
    {

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public string EncPlanID { get; set; }

        public int UserRoleID { get { return CommonStaticMethods.Decrypt<int>(EncUserRoleID); } }

        public string EncUserRoleID { get; set; }

    }

    public class AssignActivityBo
    {

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public string EncPlanID { get; set; }

        public int UserRoleID { get { return CommonStaticMethods.Decrypt<int>(EncUserRoleID); } }

        public string EncUserRoleID { get; set; }

        public string ActivityCode { get; set; }

        public int ActivityActualID { get; set; }

        public string ActivityDesc { get; set; }

        public string InitTime { get; set; }

        public short OccupancyMin { get; set; }

        public string RefNumber { get; set; }

        public string MaterialName { get; set; }

    }

    public class AssignActivityList
    {

        public PlanActivitiesList Act { get; set; }

        public RecordActionDetails Rec { get; set; }
    }

    [XmlType("ITEM")]
    public class PlanPendingActivities
    {
        public int UserTestID { get; set; }

        public string EncUserTestID { get { return CommonStaticMethods.Encrypt(UserTestID.ToString()); } }

        public string Activity { get; set; }

        public string ReferenceNumber { get; set; }

        public string ActivityDesc { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string TestTitle { get; set; }

        public int? StatusID { get; set; }

        public string Remarks { get; set; }

        public string EntityRefNumber { get; set; }

        public string MaterialName { get; set; }

        public string ArNumber { get; set; }

        public bool? IsTestCompleted { get; set; }

        public string OosNumber { get; set; }

        public string InvalidationCodes { get; set; }

        public int? PlanID { get; set; }

        public string ActivityStatus { get; set; }

        public string ActivityStatusCode { get; set; }

        public string DisplayStatus { get; set; }
    }

    [XmlType("RT")]
    public class PlanPendingActivitiesList : List<PlanPendingActivities> { }

    public class ShiftActivities
    {
        public PlanPendingActivitiesList List { get; set; }

        public RecordActionDetails Act { get; set; }

        public List<PlanBO> PlanLst { get; set; }

        public string RequestCode { get; set; }

        public string Status { get; set; }

        public string UserName { get; set; }

        public DateTime RequestDate { get; set; }

        public string Assessment { get; set; }

        public string Observations { get; set; }
    }

    public class PlanBO
    {
        public int ? PlanID { get; set; }

        public string PlanCode { get; set; }
    }

    public class TestCompletedComment
    {
        public string EncUserTestID { get; set; }

        public int UserTestID { get { return CommonStaticMethods.Decrypt<int>(EncUserTestID); } }

        public string Comment { get; set; }
    }

    public class ShiftCloseActivities
    {
        public string EncShiftID { get; set; }

        public int ShiftID { get { return CommonStaticMethods.Decrypt<int>(EncShiftID); } }

        public string InitTime { get; set; }

        public PlanPendingActivitiesList Lst { get; set; }

        public string ShiftCloseXml { get { return CommonStaticMethods.Serialize<PlanPendingActivitiesList>(Lst); } }

        public string InchargeAssessment { get; set; }

        public string Observation { get; set; }
    }


    public class ViewSamplePlanDetails
    {
        public AutoPalnDetails Planning { get; set; }

        public AnalystDetList Analysts { get; set; }

        public SampleDetailsList Sampling { get; set; }

        public SamSpecDet SampleSpec { get; set; }

        public GetTestActivityDet SampleTestAct { get; set; }

        public PalnDet PlanDetails { get; set; }

        public MasterTestList MasterList { get; set; }

        public OOSTestList OosTestList { get; set; }

        public DataReviewList DrList { get; set; }

        public CalibrationList CalibList { get; set; }
    }

    public class SearchPlanBO : PagerBO
    {
        public int SamplePlanID { get; set; }

        public int StatusID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public short PlantID { get; set; }

        public int ArID { get; set; }

        public int SampleID { get; set; }

        public int TestID { get; set; }

        public int MatID { get; set; }

        public int AnalystID { get; set; }

        public int SamplePlanIDTo { get; set; }

        public int SamplePlanIDFrom { get; set; }

        public int SpecificationID { get; set; }

        public int PlanCreatedUserRoleID { get; set; }

        public DateTime PlanCreatedOn { get; set; }

        public int ShiftID { get; set; }
    }

    public class SearchPlanresult
    {
        public int PlanID { get; set; }

        public string encPlanID { get { return CommonStaticMethods.Encrypt(PlanID.ToString()); } }

        public DateTime PlanCreatedOn { get; set; }

        public string Status { get; set; }

        public string RequestCode { get; set; }

        public string CreatedBy { get; set; }

        public string WorkShift { get; set; }
    }

    public class ChangeUserTestPlan
    {
        public string EncPlanID { get; set; }

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public int UserRoleID { get; set; }

        public int UserTestID { get; set; }

        public string InitTime { get; set; }

        public int TestID { get; set; }

        public string Type { get; set; }
    }

    public class TestOccupanyDetails
    {

        public TestOccXmlList TestList { get; set; }

        public string ReturnFlag { get; set; }


    }
    public class TestOccBO
    {
        public int TestID { get; set; }

        public short? OccMinutes { get; set; }
        
        public short? PrevOccMinutes { get { return OccMinutes; } }

        public string TestName { get; set; }

        public bool IsAssigned { get; set; }

    }

    public class TestOccXmlList : List<TestOccBO> { }
    public class SearchCloseShiftActivities : PagerBO
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int CreatedUserRoleID { get; set; }

        public int StatusID { get; set; }

        public int ShiftIDFrom { get; set; }

        public int ShiftIDTo { get; set; }

        public bool IsHODApp { get; set; }

    }

    public class GetSearchCloseShiftActivitiesDetails
    {
        public int ShiftID { get; set; }

        public string encShiftID { get { return CommonStaticMethods.Encrypt(ShiftID.ToString()); } }

        public string RequestCode { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }
    }

    public class GetCloseShiftActivitiesByID
    {
        public string RequestCode { get; set; }

        public string Status { get; set; }

        public string UserName { get; set; }

        public DateTime RequestDate { get; set; }

        public string Assessment { get; set; }

        public string Observations { get; set; }

        public List<GetShiftActivities> GetShiftActivitiesList { get; set; }

        public List<PlanBO> PlanLst { get; set; }
    }

    public class GetShiftActivities
    {
        public string ArNumber { get; set; }

        public string Activity { get; set; }

        public string TestTitle { get; set; }

        public string ActivityDesc { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        public string OosNumber { get; set; }

        public string InvalidationCodes { get; set; }

        public int? PlanID { get; set; }

        public string ActivityStatus { get; set; }
    }

    public class ManageActivityStatusBO
    {
        public string EncPlanID { get; set; }

        public int PlanID { get { return CommonStaticMethods.Decrypt<int>(EncPlanID); } }

        public string InitTime { get; set; }

        public ActivityStatusList Lst { get; set; }

        public string ActivityXml { get { return CommonStaticMethods.Serialize<ActivityStatusList>(Lst); } }
    }

    [XmlType("ITEM")]
    public class ActivityStatus
    {
        public int UserTestID { get; set; }

        public int ActivityStatusID { get; set; }
    }

    [XmlType("RT")]
    public class ActivityStatusList: List<ActivityStatus> { }
}
