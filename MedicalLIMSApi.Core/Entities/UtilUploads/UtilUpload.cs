using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.UtilUploads
{
    public class UtilUpload
    {
        public int FileUploadID { get; set; }

        public string DocumentName { get; set; }

        public string DocumentActualName { get; set; }

        public string DeptName { get; set; }

        public string UploadedBy { get; set; }

        public DateTime? UploadedOn { get; set; }

        public string EntityCode { get; set; }

        public int? DmsID { get; set; }

        public string DocumentType { get; set; }

        public int DeptID { get; set; }

        public int UserRoleID { get; set; }

        public string Section { get; set; }

        public int? DoctTrackID { get; set; }

        public string IsSystemDoc { get; set; }

        public string EntRptCode { get; set; }

        public string VersionCode { get; set; }

        public int EntActID { get; set; }
        
        public bool? IsPermanentRptGenerated { get; set; }

        public bool IsShowChkbox { get; set; }

        public string Remarks { get; set; }

    }

    public class UploadFileID
    {
        public int FileUploadID { get; set; }

        public string ResultFlag { get; set; }

        public int DMSTempID { get; set; }

        public string ReturnFlag { get; set; }
    }

    public class getUplodedFiles
    {
        public string InputType { get; set; }

        public string EntityCode { get; set; }

        public int EntityActID { get; set; }

        public string Section { get; set; }

        public string EncryptedKey { get; set; }

        public SIngleBOList FileUploadedIDs { get; set; }

        public SIngleBOList DMSTempIDLst { get; set; }

        //public string XMLstring { get { return fileUploadedIDs.Count > 0 ? CommonStaticMethods.Serialize<SIngleBOList>(fileUploadedIDs) : string.Empty; } }

        public int DMSReportID { get; set; }

        public string RefNumber { get; set; }

        public string Type { get; set; }

        public string Role { get; set; }

        public string LoginID { get; set; }

        public string DeptCode { get; set; }

        public string AppCode { get; set; }

        public string PlantCode { get; set; }

        public string FileName { get; set; }

        public int DocumentTrackID { get; set; }

        public int EntActID { get; set; }

        public string SectionCode { get; set; }

    }

    public class BaseDocumentBO
    {
        public int UtilID { get; set; }

        public string AppCode { get; set; }

        public string PlantCode { get; set; }

        public string EntityCode { get; set; }

        public string DeptCode { get; set; }

        public string SectionCode { get; set; }

        public string RefNumber { get; set; }

        public string Role { get; set; }

        public string LoginID { get; set; }

        public int EntActID { get; set; }

        public string EntityActID { get; set; }

        public SIngleBOList DMSTempIDLst { get; set; }

        public string Remarks { get; set; }
    }

    public class DownloadDoc
    {
        public string Path { get; set; }
    }
    public class FileDownload
    {
        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public string EntityCode { get; set; }

        public string Action { get; set; }

        public string Section { get; set; }

        public int FileUploadID { get; set; }

        public string DocumentName { get; set; }

        public string DocumentActualName { get; set; }

        public int ReportID { get; set; }

        public string Type { get; set; }

    }

    public class UploadedFileDetails
    {
        public string DocumentActualName { get; set; }

        public string DocumentName { get; set; }

    }
    [XmlType("ITEM")]
    public class PlaceHoldersDetails
    {
        public string PlaceHolder { get; set; }

        public string PlaceHolderKey { get; set; }

        public string VisiblePlaceHolder { get; set; }

        public int DMSPlaceHolderID { get; set; }

        public string PlaceHolderValue { get; set; }
    }

    [XmlType("RT")]
    public class PlaceHoldersDetailsList : List<PlaceHoldersDetails> { }

    public class DocumentRecord : getUplodedFiles
    {
        public int DMSId { get; set; }

        public int DocumentID { get; set; }

        public string EntityReportCode { get; set; }

        public bool FinalFlag { get; set; }

        public string FileName { get; set; }

        public PlaceholderList PlaceholderList { get; set; }

        public ARDSPrintDocHeaderBO PrintObj { get; set; }
    }

    public class ARDSPrintDocHeaderBO
    {
        public string EntityCode { get; set; }

        public string InitialDocPath { get; set; }

        public string FolderName { get; set; }

        public string PlantCode { get; set; }

        public short PlantID { get; set; }

        public int? DocTrackID { get; set; }

        public string PlantOrgCode { get; set; }

        public GetARDSPlaceholderData Placeholders { get; set; }
    }

    public class ViewReesetReportDetailsBO : ArdsBO
    {
        public string Type { get; set; }

        public string AppCode { get { return "MEDICAL_LIMS"; } }

        public string Section { get; set; }

        public string Role { get; set; }
    }

    public class UploadFileBO
    {
        public string Section { get; set; }

        public int? DMSID { get; set; }

        public int EntityActID { get; set; }

        public string AppCode { get; set; }

        public string LoginID { get; set; }

        public string Role { get; set; }

        public string Type { get; set; }

        public List<SingleBO> Lst { get; set; }

        public string EntityCode { get; set; }
    }

    public class UploadedDocBo
    {
        public int DMSTempID { get; set; }

        public string DocPath { get; set; }

        public string ReturnFlag { get; set; }
    }

    public class UpdateARDSPlaceholder
    {
        public PlaceholderList Lst { get; set; }

        public string PlaceholderXml { get { return CommonStaticMethods.Serialize<PlaceholderList>(Lst); } }

        public string EntityCode { get; set; }

        public string EncEntActID { get; set; }

        public string Type { get; set; }

        public int EntActID { get {return CommonStaticMethods.Decrypt<int>(EncEntActID); } }
    }

    public class DocumentCount
    {
        public int ID { get; set; }

        public int? NoOfFiles { get; set; }
    }

    public class DocumentCountList : List<DocumentCount> { }
}
