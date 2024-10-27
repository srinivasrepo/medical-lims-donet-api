using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.DataReview
{
    public class GetReviewTests
    {
        public string EntityCode { get; set; }

        public string RequestType { get; set; }

        public int RequestID { get; set; }
    }

    public class DataReviewTest
    {
        public int ArdsExecID { get; set; }

        public string TestName { get; set; }

        public int? ReviewItemID { get; set; }

        public string SrNum { get; set; }

        public int? ChecklistTypeID { get; set; }

        public string ChecklistCode { get; set; }

        public string ChecklistType { get; set; }

        public int? EntActID { get; set; }

        public string CorrectedResult { get; set; }

        public string TestUom { get; set; }

    }

    public class DataReviewTestList : List<DataReviewTest> { }

    public class DataReviewData
    {
        public string EncReviewID { get; set; }

        public int ReviewID { get { return CommonStaticMethods.Decrypt<int>(EncReviewID); } }

        public string EntityCode { get; set; }

        public int RequestType { get; set; }

        public int RequestID { get; set; }

        public string InitTime { get; set; }

        public string Observations { get; set; }

        public string Recommendations { get; set; }

        public string Remarks { get; set; }

        public string DataFileNo { get; set; }

        public string ApplicationSoftware { get; set; }

        public SIngleBOList Lst { get; set; }

        public string TestXml { get { return CommonStaticMethods.Serialize<SIngleBOList>(Lst); } }

        public CheckList CheckLst { get; set; }

        public string ChecklistXml { get { return CommonStaticMethods.Serialize<CheckList>(CheckLst); } }

    }

    [XmlType("ITEM")]
    public class CheckListBO
    {
        public int ReviewItemID { get; set; }

        public string RawDataVerified { get; set; }

        public string ElectronicDataVerified { get; set; }

        public string Remarks { get; set; }
    }

    [XmlType("RT")]
    public class CheckList : List<CheckListBO> { }

    public class GetDataReviewDetails
    {
        public string Observations { get; set; }

        public string Recommendations { get; set; }

        public string Remarks { get; set; }

        public string RequestCode { get; set; }

        public string Status { get; set; }

        public int RequestID { get; set; }

        public string RequestTypeCode { get; set; }

        public string TypeOfReview { get; set; }

        public string ApplicationSoftware { get; set; }

        public string DataFileNo { get; set; }

        public string SelectedPacks { get; set; }

        public bool HasReviewAccess { get; set; }

        public string RequestType { get; set; }

        public RecordActionDetails Act { get; set; }

        public DataReviewTestList ReviewLst { get; set; }

        public List<ContainerList> PackLst { get; set; }
    }

    public class SearchDataReview : PagerBO
    {
        public int RequestType { get; set; }

        public int StatusID { get; set; }


        public string EntityCode { get; set; }

        public int ArID { get; set; }

        public int MaintanceRptID { get; set; }

        public int MatID { get; set; }

        public int InstrumentID { get; set; }

        public int InvID { get; set; }

        public int AnalysisType { get; set; }

        public int ScheduleType { get; set; }

        public int MatCategoryID { get; set; }

        public DateTime DateOfReviewFrom { get; set; }

        public DateTime DateOfReviewTo { get; set; }

        public int SampleID { get; set; }

        public int InitiatedBy { get; set; }

        public DateTime InitiatedOn { get; set; }

    }

    public class GetDataReviewDetailsBySearch
    {
        public string EncReviewID { get { return CommonStaticMethods.Encrypt(ReviewID.ToString()); } }

        public int ReviewID { get; set; }

        public string RequestType { get; set; }

        public string RequestCode { get; set; }

        public int? TotalTests { get; set; }

        public string Status { get; set; }

        public string ReferenceNum { get; set; }

        public string MatOrInstumnetName { get; set; }

        public string BatchNumber { get; set; }

        public string AnalysisOrScheduleType { get; set; }

        public string UserName { get; set; }

        public string TestName { get; set; }

        public DateTime CreatedOn { get; set; }

    }

    public class ContainerList
    {
        public int ContinerAnalysisID { get; set; }

        public string PackNumber { get; set; }
    }

}
