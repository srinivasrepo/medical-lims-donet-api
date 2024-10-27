using MedicalLIMSApi.Core.CommonMethods;

namespace MedicalLIMSApi.Core.Entities.Common
{
    public class TransResults
    {
        public string InitTime { get; set; }

        public string ResultFlag { get; set; }

        public int TransKey { get; set; }

        public int UserID { get; set; }

        public short TransDocKey { get; set; }

        public string DeptCode { get; set; }

        public string PlantCode { get; set; }

        public string ApplicationType { get { return "MEDICAL_LIMS"; } }

        public int DeptID { get; set; }

        public short PlantID { get; set; }

        public string ResultReturn1 { get; set; }

        public string ResultReturn2 { get; set; }

        public short RoleID { get; set; }

        public int UserRoleID { get; set; }

        public string RoleType { get; set; }

        public string Status { get; set; }

        public string ReferenceNumber { get; set; }

        public int ? SupplierID { get; set; }

        public string EncryptedKey
        {
            get
            {
                if (TransKey > 0)
                    return CommonMethods.CommonStaticMethods.Encrypt(TransKey.ToString());
                else if (TransDocKey > 0)
                    return CommonMethods.CommonStaticMethods.Encrypt(TransDocKey.ToString());
                else
                    return string.Empty;

            }
        }

        public string LoginID { get; set; }

        public string RoleName { get; set; }

        public string UserToken { get; set; }
    }

    public class LookupData
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ExtColName { get; set; }
    }

    public class LookupInfo
    {
        public string LookupCode { get; set; }

        public string Purpose { get; set; }

        public string Condition { get; set; }

        public string ExtCondition { get; set; }

        public string SearchText { get; set; }

        public short PlantID { get; set; }

        public bool IsRs232Mode { get; set; }

        public DbInfo DbInfo { get { return CommonStaticMethods.GetLookUpDBData(LookupCode, Condition, Purpose, IsRs232Mode); } }

    }

    public class DbInfo
    {
        public string ViewName { get; set; }

        public string ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ExtColName { get; set; }

        public bool PlantFilter { get; set; }

        public string ExtCondition { get; set; }
    }

    public class TransResultApprovals
    {
        public string InitTime { get; set; }

        public string ReturnFlag { get; set; }

        public int? TransKey { get; set; }

        public short? TransDocKey { get; set; }

        public int? DetailID { get; set; }

        public bool? CanApprove { get; set; }

        public string Status { get; set; }

        public string ReferenceNumber { get; set; }

        public string EncryptedKey
        {
            get
            {
                if (TransKey > 0)
                    return CommonMethods.CommonStaticMethods.Encrypt(TransKey.ToString());
                else if (TransDocKey > 0)
                    return CommonMethods.CommonStaticMethods.Encrypt(TransDocKey.ToString());
                else
                    return string.Empty;

            }
        }

        public string OperationType { get; set; }

        public string Component { get; set; }

        public byte AppLevel { get; set; }
    }

}
