using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SampleAnalysis;
using MedicalLIMSApi.Core.Entities.Utilities;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Interface.UtilUploads
{
    public interface IUtilUploads
    {
        UploadFileID UploadDocument(TransResults tran, FileUploadData upd);

        List<UtilUpload> GetDocuments(getUplodedFiles obj);

        string DeleteDocument(int docID, TransResults tr);
        UploadedFileDetails GetFileDetails(FileDownload obj);
       
        ARDSPrintDocHeaderBO GetDocumentHeaderData(ARDSManageRequest obj);

        string UpdateReportID(ArdsBO obj, TransResults tr);

        TransResultApprovals ARDSManageRequest(ARDSManageRequest obj, TransResults tr);

        TransResultApprovals SaveContainerArdsDetails(SaveContainerArdsDetails obj, TransResults tr);

        SIngleBOList GetCumulativeInvalidationInfo(int entActID, string type);
    }
}
