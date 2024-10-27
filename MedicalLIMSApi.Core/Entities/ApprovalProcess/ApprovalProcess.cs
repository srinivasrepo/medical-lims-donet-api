using MedicalLIMSApi.Core.Entities.Common;
using System.Collections.Generic;


namespace MedicalLIMSApi.Core.Entities.ApprovalProcess
{
    public class ApprovalDetails
    {
        public int DetailID { get; set; }
        public int RoleID { get; set; }

        public int DeptID { get; set; }

        public byte PlantID { get; set; }

        public int UserID { get; set; }

        public int EntActualID { get; set; }
    }

    public class ApprovalAction
    {
        public int ActionID { get; set; }

        public string Action { get; set; }

        public string ActionCode { get; set; }
    }

    public class ApprovalActions
    {
        public List<ApprovalAction> Actions { get; set; }

        public string Status { get; set; }
    }

    public class ApprovalConfirmDetails
    {
        public int EntActID { get { return CommonMethods.CommonStaticMethods.Decrypt<int>(EncryptedKey); } }

        public int DetailID { get; set; }

        public string InitTime { get; set; }

        public string Comment { get; set; }

        public int ActionID { get; set; }

        public string EncryptedKey { get; set; }

        public string EntityCode { get; set; }

        public byte AppLevel { get; set; }
    }

    public class SupplierLogin
    {
        public string ReturnFlag { get; set; }

        public int LoginUserID { get; set; }
    }

    public class ArdsReportInvalidBO
    {
        public SIngleBOList List { get; set; }

        public string ReturnFlag { get; set; }

        public string EntityCode { get; set; }

        public string PlantCode { get; set; }

        public string SectionCode { get; set; }

        public string DeptCode { get; set; }

        public string AppCode { get; set; }

        public string WaterMarkerText { get; set; }

    }

    public class ARDSInvalidation
    {
        public int? EntityActID { get; set; }

        public string EntityCode { get; set; }

        public string InvalidationNumber { get; set; }

        public string ReturnFlag { get; set; }
    }

    public class CumulativeInvalidationBO
    {
        public int InvalidationID { get; set; }

        public string InvalidationCode { get; set; }

        public string ImpactTypeCode { get; set; }

        public string ImpactType { get; set; }
    }

    public class ARDSMergedReport
    {
        public SIngleBOList List { get; set; }

        public string ReferenceNumber { get; set; }
    }
}
