using MedicalLIMSApi.Core.Entities.ApprovalProcess;
using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using MedicalLIMSApi.Core.Interface.ApprovalProcess;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.ApprovalProcess
{
    public class ApprovalProcessRepository: IApprovalProcess
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public ApprovalActions GetUserEntityApplicableActions(ApprovalDetails app)
        {
            ApprovalActions result = new ApprovalActions();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspUMGetUserApprovalActions");
            ctx.AddInParameter<int>(cmd, "@RoleID", app.RoleID);
            ctx.AddInParameter<int>(cmd, "@UserID", app.UserID);
            ctx.AddInParameter<int>(cmd, "@DetailID", app.DetailID);

            ctx.AddOutParameter(cmd, "@Retcode", System.Data.DbType.String, 25);

            using (var reader = cmd.ExecuteReader()) 
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<ApprovalAction>(reader);
                result.Actions = new List<ApprovalAction>();
                foreach (var rr in rrResult)
                    result.Actions.Add(rr);
            }

            result.Status = ctx.GetOutputParameterValue(cmd, "@Retcode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return result;
        }

        public string ConfirmAction(ApprovalConfirmDetails app, TransResults trn)
        {
            string retVal = string.Empty;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "lims.uspUMConfirmAction");
            ctx.AddInParameter<int>(cmd, "@EntActID", app.EntActID);
            ctx.AddInParameter<int>(cmd, "@DetailID", app.DetailID);
            ctx.AddInParameter<string>(cmd, "@InitTime", app.InitTime);
            if (!string.IsNullOrEmpty(app.Comment))
                ctx.AddInParameter<string>(cmd, "@Comment", app.Comment);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<int>(cmd, "@ActionID", app.ActionID);
            ctx.AddInParameter<int>(cmd, "@PlantID", trn.PlantID);
            ctx.AddOutParameter(cmd, "@Retcode", System.Data.DbType.String, 500);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@Retcode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public ArdsReportInvalidBO GetArdsInvalidDocument(int qualificationID)
        {
            ArdsReportInvalidBO obj = new ArdsReportInvalidBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "aqual.uspGetARDSDocsByQualificationID");
            ctx.AddInParameter<int>(cmd, "@QualificationID", qualificationID);
            ctx.AddOutParameter(cmd, "@WaterMarkText", System.Data.DbType.String, 50);
            ctx.AddOutParameter(cmd, "@EntityCode", System.Data.DbType.String, 25);
            ctx.AddOutParameter(cmd, "@Section", System.Data.DbType.String, 25);
            using(var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleBO>(reader);
                obj.List = new SIngleBOList();
                foreach (var rr in rrRes)
                    obj.List.Add(rr);
            }
            obj.ReturnFlag = ctx.GetOutputParameterValue(cmd, "@WaterMarkText");
            obj.EntityCode = ctx.GetOutputParameterValue(cmd, "@EntityCode");
            obj.SectionCode = ctx.GetOutputParameterValue(cmd, "@Section");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return obj;
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
            ctx.AddInParameter<int?>(cmd, "@InvalidationID", obj.InvalidationID);
            ctx.AddOutParameter(cmd, "@ReturnFlag", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retCode = ctx.GetOutputParameterValue(cmd, "@ReturnFlag");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retCode;
        }

        public ARDSInvalidation GetInvalidationInfoForArds(int invalidationID)
        {
            ARDSInvalidation obj = new ARDSInvalidation();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetInvalidationInfoForARDSReport");

            ctx.AddInParameter<int>(cmd, "@InvalidationID", invalidationID);
            using(var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ARDSInvalidation>(reader);
                foreach (var rr in rrRes)
                    obj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public CumulativeInvalidationBO GetCumulativeInvalidationInfo(int invalidationID)
        {
            CumulativeInvalidationBO obj = new CumulativeInvalidationBO();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetInvalidationInfoForCumulativeReport");

            ctx.AddInParameter<int>(cmd, "@InvalidationID", invalidationID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<CumulativeInvalidationBO>(reader);
                foreach (var rr in rrRes)
                    obj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public ARDSMergedReport GetArdsMergedReportInfo(int entActID, string entityCode)
        {
            ARDSMergedReport obj = new ARDSMergedReport();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetARDSReportDMSIDs");

            ctx.AddInParameter<int>(cmd, "@EntActID", entActID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", entityCode);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ARDSMergedReport>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();
                obj.List = new SIngleBOList();
                var rrList = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleBO>(reader);
                foreach (var rr in rrList)
                    obj.List.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }
    }
}
