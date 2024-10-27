using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.Utilities
{
    [XmlType("ITEM")]
    public class Role
    {
        public int ROLE_ID { get; set; }

        public string ROLE_NAME { get; set; }

    }

    [XmlType("RT")]
    public class RoleList : List<Role> { }

    public class FileUploadData
    {
        public string FileName { get; set; }

        public string ActFileName { get; set; }

        public string Section { get; set; }

        public string EntityCode { get; set; }

        public string DocType { get; set; }

        public int DmsID { get; set; }

        public string DocNumber { get; set; }

        public int VersionNo { get; set; }

        public string DocPlaceHoldersXML { get; set; }

    }
}
