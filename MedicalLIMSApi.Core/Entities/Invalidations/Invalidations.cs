using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Entities.Invalidations
{
    public class SearchInvalidations
    {
        public int InvalidationID { get; set; }

        public string EncInvalidationID { get { return CommonStaticMethods.Encrypt(InvalidationID.ToString()); } }

        public string InvalidationNumber { get; set; }

        public string InvalidationCode { get; set; }

        public string ProductName { get; set; }

        public string Stage { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string BatchNumber { get; set; }

        public string InvalidationSource { get; set; }

        public string AnalysisUser { get; set; }

        public string InstCode { get; set; }

        public string TestName { get; set; }

    }

    public class SearchInvalidationsList : List<SearchInvalidations> { }

    public class InvSearchBO : PagerBO
    {
        public int InvalidationID { get; set; }

        public int StatusID { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int InvTypeID { get; set; }

        public int InstrumentTypeID { get; set; }

        public short PlantID { get; set; } 

        public int SourceTypeID { get; set; }

        public int ArID { get; set; }

        public int MatID { get; set; }

        public int InstrumentID { get; set; }

        public int AnalysisDoneBy { get; set; }

        public DateTime InitiatedOn { get; set; }

        public int InitiatedBy { get; set; }

        public int AnalysisType { get; set; }
    }

    public class InvalidationInfo : ManageInvalidationBO
    {
        public string ProductName { get; set; }

        public string Stage { get; set; }

        public string InvalidationNumber { get; set; }

        public string InvalidationCode { get; set; }

        public string BatchNumber { get; set; }

        public string ArNumber { get; set; }

        public string EqpTitle { get; set; }

        public string EqpCode { get; set; }

        public string TestTitle { get; set; }

        public string AnalysisUser { get; set; }

        public int? ImapactType { get; set; }

        public string ImpactTypeName { get; set; }

        public string RootCauseCategory { get; set; }

        public string STATUS { get; set; }

        public int Applevel { get; set; }

        public DateTime InvalidationDate { get; set; }

        public string InstTypeName { get; set; }

        public rootCauseList RootCause { get; set; }

        public RecordActionDetails Act { get; set; }

        public string EntitySource { get; set; }

        public string CanProcess { get; set; }

        public string EnitySourecDesc { get; set; }

        public int? PrevInvalidationID { get; set; }

        public string EqpType { get; set; }

        public string IntiatedUser { get; set; }

        public int? AnalysisUserRoleID { get; set; }

        public string OosNumber { get; set; }

        public bool IsPreviousRequestsPending { get; set; }

        public string InstTypeCode { get; set; }

        public string SpecStpNo { get; set; }

        public int? InstID { get; set; }

        public int? InstTypeID { get; set; }
    }

    public class rootCause
    {
        public int? RootCauseID { get; set; }

        public string RootCause { get; set; }

        public string RootCauseCode { get; set; }
    }

    public class rootCauseList : List<rootCause> { }

    public class ManageInvalidationBO
    {
        public string EncInvalidationID { get; set; }

        public int InvalidationID { get { return CommonStaticMethods.Decrypt<int>(EncInvalidationID); } }

        public string Type { get; set; }

        public string ImpactTypeCode { get; set; }

        public string SampleSetNo { get; set; }

        public string DataFileNo { get; set; }

        public string Description { get; set; }

        public string OtherReasons { get; set; }

        public string ActionsRecommended { get; set; }

        public string IsReAnalysisValid { get; set; }

        public string ReviewActRecommended { get; set; }

        public string ImplimantationSummary { get; set; }

        public string InitTime { get; set; }

        public int? InstType { get; set; }

        public string InitSSTResult { get; set; }

        public string InitAnaResult { get; set; }

        public string ReSSTResult { get; set; }

        public string ReAnaResult { get; set; }

        public SIngleBOList List { get; set; }

        public string RootXml { get { return CommonStaticMethods.Serialize<SIngleBOList>(List); } }

        public string OtherRootCause { get; set; }

        public string QARemarks { get; set; }

        public int AnalysisDone { get; set; }
    }

    public class GenericBO
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
    public class GenericBOList : List<GenericBO> { }

    public class SearchInvalidationList
    {
        public GenericBOList InsTypes { get; set; }

        public GenericBOList InvTypes { get; set; }
    }

    public class GetPreviousInvalidations
    {
        public string InvalidationCode { get; set; }

        public string SystemCode { get; set; }

        public string ImpactType { get; set; }

        public string InstrumentType { get; set; }

    }

    public class ReviewInvalidationsBO
    {
        public string InvalidationNumber { get; set; }

        public string ReviewedBy { get; set; }

        public DateTime? ReviewedOn { get; set; }
    }

    public class ManageInvalidationManualInfo
    {
        public string EncInvalidationID { get; set; }

        public int InvalidationID { get { return CommonStaticMethods.Decrypt<int>(EncInvalidationID); } }

        public string ProductName { get; set; }

        public string Stage { get; set; }

        public string BatchNumber { get; set; }

        public string ArNumber { get; set; }

        public int InstrumentTypeID { get; set; }

        public int InstrumentID { get; set; }

        public string TestName { get; set; }

        public string SpecStpNumber { get; set; }

        public string InitTime { get; set; }
    }
}
