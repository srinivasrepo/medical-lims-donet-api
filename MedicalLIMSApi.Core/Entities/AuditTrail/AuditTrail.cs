using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.AuditTrail
{
    public class AuditTrail
    {
    }

    public class SearchAuditData:PagerBO
    {
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public short? Action { get; set; }

        public int? ActionBy { get; set; }

        public int? EntityID { get; set; }

        public string EntityRef { get; set; }

        public short PlantID { get; set; }
    }

    public class AuditData
    {
        public long AuditID { get; set; }

        public string EntityName { get; set; }

        public string EntityRefNum { get; set; }

        public DateTime AuditEntryDate { get; set; }

        public string AuditAction { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string encAuditID { get { if (AuditID > default(long)) return CommonStaticMethods.Encrypt(AuditID.ToString()); else return string.Empty; } }

        public int UserRoleID { get; set; }
    }

    public class AuditDataList : List<AuditData> { }

    public class AuditDataTitle
    {
        public long AuditTableID { get; set; }

        public string AuditSourceTable { get; set; }

        public string AuditDmlType { get; set; }

        public string encAuditTableID { get { if (AuditTableID > default(long)) return CommonStaticMethods.Encrypt(AuditTableID.ToString()); else return string.Empty; } }
    }

    public class AuditDataTitleList : List<AuditDataTitle> { }


    public class AuditDataByID
    {
        public string ColumnName { get; set; }

        public string OldData { get; set; }

        public string NewData { get; set; }

        public bool IsModified { get; set; }
    }

    public class AuditDataByIDList : List<AuditDataByID> { }


    [XmlType("ITEM")]
    public class AudObject
    {
        public int TableID { get; set; }

        public string ObjName { get; set; }

        public string ObjFriendlyName { get; set; }
    }

    [XmlType("RT")]
    public class AudObjectList : List<AudObject> { }

    public class ManageAudObj
    {
        public int TableID { get; set; }

        public AudObjectList list { get; set; }

        public string ObjectXml { get { return CommonStaticMethods.Serialize<AudObjectList>(list); } }

    }
}
