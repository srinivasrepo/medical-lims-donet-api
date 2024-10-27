using MedicalLIMSApi.Core.Entities.AnalystQualification;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Interface.AnalystQualification;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.AnalystQualification
{
    public class AnalystQualificationRepository : IAnalystQualification
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public RecordActionDetails ManageAnalystQualification(AnalystQualificationBo obj, TransResults tr)
        {
            RecordActionDetails lt = new RecordActionDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspInsertAnalystQualification");
            if (obj.AnalystID > default(int))
                ctx.AddInParameter<int>(cmd, "@AnalystID", obj.AnalystID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", obj.UserRoleID);
            ctx.AddInParameter<string>(cmd, "@Reason", obj.Reason);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@LoginUserRoleID", tr.UserRoleID);
            ctx.AddInParameter<string>(cmd, "@XmlString", obj.XmlString);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lt = rr;
            }
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return lt;
        }

        public SearchResults<SearchAnalyst> SearchAnalystDetails(SearchAnalystBo obj)
        {
            var lst = new SearchResults<SearchAnalyst>();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspSearchAnalystDetails");
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateFrom", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateTo", obj.DateTo);
            if (obj.UserID > default(int))
                ctx.AddInParameter<int>(cmd, "@UserID", obj.UserID);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrResultCount = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var rr in rrResultCount)
                    lst.TotalNumberOfRows = rr;
                reader.NextResult();

                List<SearchAnalyst> list = new List<SearchAnalyst>();
                var rrResultList = ((IObjectContextAdapter)context).ObjectContext.Translate<SearchAnalyst>(reader);
                foreach (var rr in rrResultList)
                    list.Add(rr);

                lst.SearchList = list;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SearchAnalystDet GetAnalystDetailsByID(int AnalystID, TransResults tr)
        {
            var lst = new SearchAnalystDet();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspViewAnalystDetailsByID");
            ctx.AddInParameter<int>(cmd, "@AnalystID", AnalystID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            using (var reader = cmd.ExecuteReader())
            {
                lst.Analyst = new AnalystDet();
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<AnalystDet>(reader);
                foreach (var rr in rrResult)
                    lst.Analyst = rr;

                reader.NextResult();
                lst.AnalystList = new List<AnalystQualificationDet>();
                var AnalystList = ((IObjectContextAdapter)context).ObjectContext.Translate<AnalystQualificationDet>(reader);
                foreach (var rr in AnalystList)
                    lst.AnalystList.Add(rr);

                reader.NextResult();
                lst.Act = new RecordActionDetails();
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrAct)
                    lst.Act = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public CatItemsMasterList GetAnalystQualifications()
        {
            CatItemsMasterList lst = new CatItemsMasterList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspGetAnalystQualifications");

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<CatItemsMaster>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public RecordActionDetails ManageQualificationRequest(ManageQualificationRequest obj, TransResults tr)
        {
            RecordActionDetails actResult = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "aqual.uspManageQualificationRequest");

            if (obj.QualificationID > default(int))
                ctx.AddInParameter<int>(cmd, "@QualificationID", obj.QualificationID);
            if(obj.TechniqueID > default(int))
                ctx.AddInParameter<int>(cmd, "@TechniqueID", obj.TechniqueID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", obj.AnalystID);
            ctx.AddInParameter<int>(cmd, "@QualificationTypeID", obj.QualificationTypeID);
            if(obj.MatID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatID", obj.MatID);
            if(obj.ArID > default(int))
                ctx.AddInParameter<int>(cmd, "@ArID", obj.ArID);
            if(obj.List != null && obj.List.Count > 0)
                ctx.AddInParameter<string>(cmd, "@TestXml", obj.TestXml);
            ctx.AddInParameter<int>(cmd, "@CreUserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            if (obj.ReQualificationPurposeID > default(int))
                ctx.AddInParameter<int>(cmd, "@ReQualificationPurposeID", obj.ReQualificationPurposeID);
            if (!string.IsNullOrEmpty(obj.ReferenceNo))
                ctx.AddInParameter<string>(cmd, "@ReferenceNo", obj.ReferenceNo);
            ctx.AddInParameter<string>(cmd, "@TypeCode", obj.TypeCode);
            ctx.AddInParameter<int>(cmd, "@RequestTypeID", obj.RequestTypeID);
            if (obj.VolumetricIndexID > default(short))
                ctx.AddInParameter<short>(cmd, "@VolIndexID", obj.VolumetricIndexID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    actResult = rr;
            }

            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return actResult;
        }

        public GetQualificationTypeList GetQualificationType(int techniqueID, int analystID, int requestTypeID)
        {
            var lst = new GetQualificationTypeList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "aqual.uspGetQualificationType");

            if(techniqueID > default(int))
                ctx.AddInParameter<int>(cmd, "@TechniqueID", techniqueID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", analystID);
            ctx.AddInParameter<int>(cmd, "@RequestTypeID", requestTypeID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQualificationType>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();

            }
            ctx.CloseConnection(context);

            return lst;
        }

        public GetAnalysisTypeList GetAnalysisTypeByCategoryID(int CategoryID)
        {
            var lst = new GetAnalysisTypeList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "aqual.uspGetAnalysisTypeByCategoryID");

            if (CategoryID > default(int))
                ctx.AddInParameter<int>(cmd, "@CategoryID", CategoryID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetAnalysisType>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public GetTestsByTechniqueAndArIDList GetTestsByTechniqueAndArID(SearchTestsByTechniqueAndArID obj)
        {
            var lst = new GetTestsByTechniqueAndArIDList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "aqual.uspGetTestsByTechniqueAndArID");

            if (obj.TechniqueID > default(int))
                ctx.AddInParameter<int>(cmd, "@TechniqueID", obj.TechniqueID);
            if (obj.ArID > default(int))
                ctx.AddInParameter<int>(cmd, "@ArID", obj.ArID);
            if (obj.QualificationID > default(int))
                ctx.AddInParameter<int>(cmd, "@QualificationID", obj.QualificationID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetTestsByTechniqueAndArID>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public GetQualificationDetails GetQualificationDetails(EditQualificationDetails obj)
        {
            var lst = new GetQualificationDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "aqual.uspGetQualificationDetails");

            ctx.AddInParameter<int>(cmd, "@QualificationID", obj.QualificationID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", obj.UserRoleID);

            using (var reader = cmd.ExecuteReader())
            {

                lst.GetQualificationRequestDetails = new GetQualificationRequestDetails();
                var getQuaReqDet = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQualificationRequestDetails>(reader);
                foreach (var item in getQuaReqDet)
                    lst.GetQualificationRequestDetails = item;

                reader.NextResult();
                lst.GetTestsByTechniqueAndArIDList = new List<GetTestsByTechniqueAndArID>();
                var getTeByTecAr = ((IObjectContextAdapter)context).ObjectContext.Translate<GetTestsByTechniqueAndArID>(reader);
                foreach (var item in getTeByTecAr)
                    lst.GetTestsByTechniqueAndArIDList.Add(item);

                reader.NextResult();
                lst.GetQualificationTestsDetailsList = new List<GetQualificationTestsDetails>();
                var getQuaTeDet = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQualificationTestsDetails>(reader);
                foreach (var item in getQuaTeDet)
                    lst.GetQualificationTestsDetailsList.Add(item);

                reader.NextResult();
                lst.recordActionResults = new RecordActionDetails();
                var getRecAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var item in getRecAct)
                    lst.recordActionResults = item;
                reader.NextResult();

                lst.GetAcceptenceCriteriaDetailsList = new List<AcceptenceCriteria>();
                var getAccpt = ((IObjectContextAdapter)context).ObjectContext.Translate<AcceptenceCriteria>(reader);
                foreach (var item in getAccpt)
                    lst.GetAcceptenceCriteriaDetailsList.Add(item);

                reader.NextResult();

                lst.AnalysisResultList = new AnalysisResultList();
                var rrAnaLst = ((IObjectContextAdapter)context).ObjectContext.Translate<AnalysisResult>(reader);
                foreach (var rr in rrAnaLst)
                    lst.AnalysisResultList.Add(rr);

                reader.NextResult();

                lst.TestCriteriaLst = new SelectedCriteiaList();
                var rrTestCri = ((IObjectContextAdapter)context).ObjectContext.Translate<SelectedCriteiaBO>(reader);
                foreach (var rr in rrTestCri)
                    lst.TestCriteriaLst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public GetQualificationDetailsForView GetQualificationDetailsForView(int qualificationID, TransResults tr)
        {
            var lst = new GetQualificationDetailsForView();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "aqual.uspGetQualificationDetailsForView");

            ctx.AddInParameter<int>(cmd, "@QualificationID", qualificationID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<int>(cmd, "@RoleID", tr.RoleID);

            using (var reader = cmd.ExecuteReader())
            {
                lst.QualificationRequest = new QualificationRequest();

                var quaReq = ((IObjectContextAdapter)context).ObjectContext.Translate<QualificationRequest>(reader);
                foreach (var item in quaReq)
                    lst.QualificationRequest = item;

                reader.NextResult();

                lst.GetQualificationTestsDetails = new List<GetQualificationTestsDetails>();
                var getQuaTesDet = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQualificationTestsDetails>(reader);
                foreach (var item in getQuaTesDet)
                    lst.GetQualificationTestsDetails.Add(item);

                reader.NextResult();

                lst.GetAcceptenceCriteriaDetailsList = new List<AcceptenceCriteria>();
                var getAccpt = ((IObjectContextAdapter)context).ObjectContext.Translate<AcceptenceCriteria>(reader);
                foreach (var item in getAccpt)
                    lst.GetAcceptenceCriteriaDetailsList.Add(item);

                reader.NextResult();

                lst.AnalysisResultList = new AnalysisResultList();
                var rrAnaLst = ((IObjectContextAdapter)context).ObjectContext.Translate<AnalysisResult>(reader);
                foreach (var rr in rrAnaLst)
                    lst.AnalysisResultList.Add(rr);


                cmd.Connection.Close();

            }

            ctx.CloseConnection(context);

            return lst;
        }


        public SearchResults<SearchResultsQualificationDetails> SearchResultsQualificationDetails(SearchQualificationDetails obj, short plantID)
        {
            var lst = new SearchResults<SearchResultsQualificationDetails>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "aqual.uspSearchQualification");

            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            ctx.AddInParameter<short>(cmd, "@PlantID", plantID);

            if (obj.TechniqueID > default(int))
                ctx.AddInParameter<int>(cmd, "@TechniqueID", obj.TechniqueID);
            if (obj.AnalystID > default(int))
                ctx.AddInParameter<int>(cmd, "@AnalystID", obj.AnalystID);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (obj.MatID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatID", obj.MatID);
            if (obj.AnalysisType > default(int))
                ctx.AddInParameter<int>(cmd, "@AnalysisType", obj.AnalysisType);
            if (obj.ActivityType > default(int))
                ctx.AddInParameter<int>(cmd, "@ActivityType", obj.ActivityType);
            if (obj.ArNumberID > default(int))
                ctx.AddInParameter<int>(cmd, "@ArNumberID", obj.ArNumberID);
            if (obj.SpecTestID > default(int))
                ctx.AddInParameter<int>(cmd, "@TestID", obj.SpecTestID);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateFrom", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateTo", obj.DateTo);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (!string.IsNullOrEmpty(obj.FormulaType))
                ctx.AddInParameter<string>(cmd, "@FormulaType", obj.FormulaType);
            if (obj.ConclusionID > default(int))
                ctx.AddInParameter<int>(cmd,"@ConclusionID",obj.ConclusionID);
            if (obj.InitiatedDate > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@IntiatedOn", obj.InitiatedDate);
            if (obj.InitiatedID > default(int))
                ctx.AddInParameter<int>(cmd, "@IntiatedUserRoleID", obj.InitiatedID);
            if (obj.SioID > default(int))
                ctx.AddInParameter<int>(cmd, "@SioID", obj.SioID);
            if (obj.QualificationID > default(int))
                ctx.AddInParameter<int>(cmd, "@QualificationID", obj.QualificationID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResCount = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var rr in rrResCount)
                    lst.TotalNumberOfRows = rr;

                reader.NextResult();

                List<SearchResultsQualificationDetails> list = new List<SearchResultsQualificationDetails>();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SearchResultsQualificationDetails>(reader);
                foreach (var rr in rrRes)
                    list.Add(rr);

                lst.SearchList = list;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public RecordActionDetails ManageQualificationEvaluation(ManageQualificationEvaluation obj, TransResults tr)
        {
            var lst = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "aqual.uspManageQualificationEvaluation");

            ctx.AddInParameter<int>(cmd, "@QualificationID", obj.QualificationID);
            ctx.AddInParameter<int>(cmd, "@ConclusionID", obj.ConclusionID);
            ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            if (!string.IsNullOrEmpty(obj.Justification))
                ctx.AddInParameter<string>(cmd, "@Justification", obj.Justification);
            if (obj.List != null && obj.List.Count > 0)
                ctx.AddInParameter<string>(cmd, "@TestResultXml", obj.TestXml);
            if (obj.SelectedAcceptanceCriteiaLst != null && obj.SelectedAcceptanceCriteiaLst.Count > 0)
                ctx.AddInParameter<string>(cmd, "@CriteriaXml", obj.CriteriaXml);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public string ManageAnalystDisqualify(DisqualifyBO obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "aqual.uspManageAnalystDisqualify");

            ctx.AddInParameter<int>(cmd, "@QualificationID", obj.QualificationID);
            ctx.AddInParameter<string>(cmd, "@Comments", obj.Comments);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddOutParameter(cmd, "@RetMsg", System.Data.DbType.String, 25);

            cmd.ExecuteNonQuery();
            retMsg = Convert.ToString(ctx.GetOutputParameterValue(cmd, "@RetMsg"));
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retMsg;
        }
    }
}
