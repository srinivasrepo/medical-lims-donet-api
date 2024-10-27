using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SampleAnalysis;
using MedicalLIMSApi.Core.Entities.Utilities;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using MedicalLIMSApi.Core.Interface.UtilUploads;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.UtilUploads
{
    public class UtilUploadsRepository : IUtilUploads
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public UploadFileID UploadDocument(TransResults tran, FileUploadData upd)
        {
            UploadFileID file = new UploadFileID();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspUPDInsertUserFileUploadDetails");
            ctx.AddInParameter<string>(cmd, "@DocumentName", upd.FileName);
            ctx.AddInParameter<int>(cmd, "@EntityActualID", tran.TransKey);
            ctx.AddInParameter<string>(cmd, "@DocumentActualName", upd.ActFileName);
            ctx.AddInParameter<string>(cmd, "@DocumentType", upd.DocType);
            ctx.AddInParameter<string>(cmd, "@Section", upd.Section);
            ctx.AddInParameter<short>(cmd, "@PlantID", tran.PlantID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tran.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tran.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserID", tran.UserID);

            if (upd.DmsID > 0)
                ctx.AddInParameter<int>(cmd, "@DMSID", upd.DmsID);

            ctx.AddInParameter<string>(cmd, "@EntityCode", upd.EntityCode);

            using (var reader = cmd.ExecuteReader())
            {
                var upload = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadFileID>(reader);
                foreach (var item in upload)
                    file = item;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return file;
        }

        public List<UtilUpload> GetDocuments(getUplodedFiles fileData)
        {
            var lst = new List<UtilUpload>();
            UtilUpload obj = null;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspUPDGetUserFileUploadDetails");
            ctx.AddInParameter<string>(cmd, "@Section", fileData.Section);
            ctx.AddInParameter<string>(cmd, "@EntityCode", fileData.EntityCode);
            if (fileData.EntityActID > 0)
                ctx.AddInParameter<int>(cmd, "@EntityActualID", fileData.EntityActID);
            using (var reader = cmd.ExecuteReader())
            {
                var uploads = ((IObjectContextAdapter)context).ObjectContext.Translate<UtilUpload>(reader);
                foreach (var item in uploads)
                {
                    obj = new UtilUpload()
                    {
                        FileUploadID = item.FileUploadID,
                        DocumentName = item.DocumentName,
                        DocumentActualName = item.DocumentActualName,
                        DocumentType = item.DocumentType,
                        UploadedOn = item.UploadedOn,
                        UploadedBy = item.UploadedBy,
                        DmsID = item.DmsID
                    };
                    lst.Add(obj);
                }
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string DeleteDocument(int docID, TransResults tr)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspUPDDeleteUserFileUploads");
            ctx.AddInParameter<int>(cmd, "@FileUploadID", docID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@DocumentName", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@DocumentName");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public UploadedFileDetails GetFileDetails(FileDownload obj)
        {
            var lst = new UploadedFileDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspUPDGetFileDetails");
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@EntityActualID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@Section", obj.Section);

            using (var reader = cmd.ExecuteReader())
            {
                var upload = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadedFileDetails>(reader);

                foreach (var item in upload)
                    lst = item;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ARDSPrintDocHeaderBO GetDocumentHeaderData(ARDSManageRequest obj)
        {
            var lst = new ARDSPrintDocHeaderBO();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetPrintDocHeaderInfo");
            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            ctx.AddInParameter<int>(cmd, "@SPECID", obj.SpecID);
            ctx.AddInParameter<int>(cmd, "@SPEC_ARDSID", obj.ArdsID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<short>(cmd, "@PlantID", obj.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var upload = ((IObjectContextAdapter)context).ObjectContext.Translate<ARDSPrintDocHeaderBO>(reader);

                foreach (var item in upload)
                    lst = item;

                reader.NextResult();
                lst.Placeholders = new GetARDSPlaceholderData();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<PlaceholderData>(reader);
                lst.Placeholders.PlaceholderList = new PlaceholderList();
                foreach (var rr in rrLst)
                    lst.Placeholders.PlaceholderList.Add(rr);

                reader.NextResult();
                if (obj.EntityCode == "QCSAMPASYS")
                {
                    var rrVal = ((IObjectContextAdapter)context).ObjectContext.Translate<SampleAnalysisPlaceholderValues>(reader);
                    lst.Placeholders.SampleAnalysisValues = new SampleAnalysisPlaceholderValues();
                    foreach (var rr in rrVal)
                        lst.Placeholders.SampleAnalysisValues = rr;
                }
                else if (obj.EntityCode == "ENGGMNT")
                {
                    var rrVal = ((IObjectContextAdapter)context).ObjectContext.Translate<CalibrationPlaceholderValues>(reader);
                    lst.Placeholders.CalibrationValues = new CalibrationPlaceholderValues();
                    foreach (var rr in rrVal)
                        lst.Placeholders.CalibrationValues = rr;
                }
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string UpdateReportID(ArdsBO obj, TransResults tr)
        {
            string retCode = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "calib.uspUpdateReportID");

            ctx.AddInParameter<int>(cmd, "@ArdsExecID", obj.ArdsExecID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int?>(cmd, "@ReportID", obj.ReportID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@ReturnFlag", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retCode = ctx.GetOutputParameterValue(cmd, "@ReturnFlag");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retCode;
        }

        public TransResultApprovals ARDSManageRequest(ARDSManageRequest obj, TransResults tr)
        {
            var lst = new TransResultApprovals();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspARDSManageRequest");

            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@SpecID", obj.SpecID);
            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.EntityActID);
            if (obj.ArdsID > default(int))
                ctx.AddInParameter<int>(cmd, "@TrackID", obj.ArdsID);
            if (!string.IsNullOrEmpty(obj.DocPath))
                ctx.AddInParameter<string>(cmd, "@DocPath", obj.DocPath);
            ctx.AddInParameter<string>(cmd, "@AnalysisMode", obj.AnalysisMode);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (!string.IsNullOrEmpty(obj.ExtraneousAnalysis))
                ctx.AddInParameter<string>(cmd, "@ExtraneousMatterApplicable", obj.ExtraneousAnalysis);
            if (obj.List != null && obj.List.Count > 0)
                ctx.AddInParameter<string>(cmd, "@ExtraneousTestsXml", obj.ExtraneousTestsXml);
            if (!string.IsNullOrEmpty(obj.ContainerWiseAnalysisApp))
                ctx.AddInParameter<string>(cmd, "@ContaierWiseAnalysisApp", obj.ContainerWiseAnalysisApp);
            if (obj.DMSID > default(int))
                ctx.AddInParameter<int>(cmd, "@DMSID", obj.DMSID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public TransResultApprovals SaveContainerArdsDetails(SaveContainerArdsDetails obj, TransResults tr)
        {
            TransResultApprovals act = new TransResultApprovals();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspInsertContainerWiseAnalysis");
            ctx.AddInParameter<int>(cmd, "@SpecID", obj.SpecID);
            ctx.AddInParameter<string>(cmd, "@AnalysisMode", obj.ArdsMode);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            if (obj.TestID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecCatID", obj.TestID);
            ctx.AddInParameter<string>(cmd, "@PackXml", obj.PackXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@SioID", obj.SioID);
            if (obj.ArdsID > default(int))
                ctx.AddInParameter<int>(cmd, "@TRACKID", obj.ArdsID);
            if (!string.IsNullOrEmpty(obj.DocPath))
                ctx.AddInParameter<string>(cmd, "@DocPath", obj.DocPath);
            if (obj.DmsID > default(int))
                ctx.AddInParameter<int>(cmd, "@DMSID", obj.DmsID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }

        public SIngleBOList GetCumulativeInvalidationInfo(int entActID, string type)
        {
            SIngleBOList lst = new SIngleBOList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetInvalidationInfoForCumulativeReport");

            ctx.AddInParameter<int>(cmd, "@InvalidationID", entActID);
            ctx.AddInParameter<string>(cmd, "@Type", type);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleBO>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }
    }
}
