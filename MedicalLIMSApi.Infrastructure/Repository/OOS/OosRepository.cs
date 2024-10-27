using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.OOS;
using MedicalLIMSApi.Core.Interface.OOS;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalLIMSApi.Infrastructure.Repository.OOS
{
    public class OosRepository : IOos
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public SearchResults<GetSearchOOSTestDetails> OOSSearchOOSTestDetails(SearchOOSTestDetails obj, TransResults tr)
        {
            var list = new SearchResults<GetSearchOOSTestDetails>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSSearchOOSTestDetails");

            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (obj.CategoryID > default(int))
                ctx.AddInParameter<int>(cmd, "@CategoryID", obj.CategoryID);
            if (obj.SubCatID > default(int))
                ctx.AddInParameter<int>(cmd, "@SubCatID", obj.SubCatID);
            if (obj.MaterialID > default(int))
                ctx.AddInParameter<int>(cmd, "@MaterialID", obj.MaterialID);
            if (obj.OosNumberFrom > default(int))
                ctx.AddInParameter<int>(cmd, "@OosNumberFrom", obj.OosNumberFrom);
            if (obj.OosNumberTo > default(int))
                ctx.AddInParameter<int>(cmd, "@OosNumberTo", obj.OosNumberTo);
            if (obj.BatchNumber > default(int))
                ctx.AddInParameter<int>(cmd, "@BatchNumber", obj.BatchNumber);
            if (obj.TestID > default(int))
                ctx.AddInParameter<int>(cmd, "@TestID", obj.TestID);
            if (obj.SpecificationID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecificationID", obj.SpecificationID);
            if (obj.ProjectID > default(int))
                ctx.AddInParameter<int>(cmd, "@ProjectID", obj.ProjectID);
            if (obj.MoleculeType > default(short))
                ctx.AddInParameter<short>(cmd, "@MoleculeType", obj.MoleculeType);
            if (obj.BuildID > default(int))
                ctx.AddInParameter<int>(cmd, "@BuildID", obj.BuildID);
            if (obj.StageID > default(int))
                ctx.AddInParameter<int>(cmd, "@StageID", obj.StageID);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateFrom", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateTo", obj.DateTo);
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var totalRecords = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var tnr in totalRecords)
                    list.TotalNumberOfRows = tnr;

                reader.NextResult();

                List<GetSearchOOSTestDetails> gsts = new List<GetSearchOOSTestDetails>();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSearchOOSTestDetails>(reader);
                foreach (var rr in rrRes)
                    gsts.Add(rr);

                list.SearchList = gsts;
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return list;
        }

        public TestInfo GetTestInfo(OOSGetHypoTestingInfo data, TransResults tr)
        {
            TestInfo obj = new TestInfo();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSGetTestInfo");
            if (data.OOSTestID > default(int))
                ctx.AddInParameter<int>(cmd, "@OosTestID", data.OOSTestID);
            if (data.OOSTestDetailID > default(int))
                ctx.AddInParameter<int>(cmd, "@OosTestDetailsID", data.OOSTestDetailID);
            ctx.AddInParameter<string>(cmd, "@ConditionCode", data.ConditionCode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TestInfo>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();

                var rrResPhaseList = ((IObjectContextAdapter)context).ObjectContext.Translate<GetPhaseDetailsBO>(reader);

                obj.PhaseList = new GetPhaseDetailsBOList();

                foreach (var rr in rrResPhaseList)
                    obj.PhaseList.Add(rr);

                reader.NextResult();
                obj.Act = new RecordActionDetails();

                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrAct)
                    obj.Act = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public RecordActionDetails UpdateOOSSummary(UpdateSummary obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSUpdateSummary");
            ctx.AddInParameter<int>(cmd, "@OosTestID", obj.OosTestID);
            ctx.AddInParameter<string>(cmd, "@Summary", obj.Summary);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return act;
        }

        public GetOOSPhase1CheckList GetOOSPhase1CheckList(int OosDetailID)
        {
            var list = new GetOOSPhase1CheckList();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSGetPhase1CheckList");
            ctx.AddInParameter<int>(cmd, "@OOS_DETID", OosDetailID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetOOSPhase1CheckList>(reader);
                foreach (var rr in rrRes)
                    list = rr;

                reader.NextResult();

                list.GetQCOOSCheckList = new List<GetQCOOSCheckList>();
                var getchList = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQCOOSCheckList>(reader);
                foreach (var rr in getchList)
                    list.GetQCOOSCheckList.Add(rr);

                reader.NextResult();

                list.GetCheckListQuestions = new List<GetCheckListQuestions>();
                var getChQs = ((IObjectContextAdapter)context).ObjectContext.Translate<GetCheckListQuestions>(reader);
                foreach (var rr in getChQs)
                    list.GetCheckListQuestions.Add(rr);

                reader.NextResult();

                list.QCOosTestDetails = new QCOosTestDetails();
                var getTestDet = ((IObjectContextAdapter)context).ObjectContext.Translate<QCOosTestDetails>(reader);
                foreach (var rr in getTestDet)
                    list.QCOosTestDetails = rr;

                reader.NextResult();

                list.OosProcessCondition = new OosProcessCondition();
                var getOosPrCon = ((IObjectContextAdapter)context).ObjectContext.Translate<OosProcessCondition>(reader);
                foreach (var rr in getOosPrCon)
                    list.OosProcessCondition = rr;

                reader.NextResult();

                list.OosProcessList = new List<OosProcessList>();
                var getOosProList = ((IObjectContextAdapter)context).ObjectContext.Translate<OosProcessList>(reader);
                foreach (var rr in getOosProList)
                    list.OosProcessList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return list;
        }

        public string OOSProcessItem(OOSProcessItem obj, TransResults tr)
        {
            var resultFlag = string.Empty;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSProcessItem");

            ctx.AddInParameter<int>(cmd, "@OOSTestID", obj.OOSTestID);
            ctx.AddInParameter<int>(cmd, "@OOSTestDetailID", obj.OOSTestDetailID);
            ctx.AddInParameter<int>(cmd, "@CreatedBY", tr.UserID);
            ctx.AddInParameter<byte>(cmd, "@Count", obj.Count);
            ctx.AddInParameter<string>(cmd, "@Status", obj.Status);
            if (!string.IsNullOrEmpty(obj.Validity))
                ctx.AddInParameter<string>(cmd, "@Validity", obj.Validity);
            if (!string.IsNullOrEmpty(obj.Remarks))
                ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            if (!string.IsNullOrEmpty(obj.CheckList))
                ctx.AddInParameter<string>(cmd, "@CheckList", obj.CheckList);
            ctx.AddInParameter<bool>(cmd, "@IsMisc", obj.IsMisc);
            if (!string.IsNullOrEmpty(obj.DocXml))
                ctx.AddInParameter<string>(cmd, "@DocXml", obj.DocXml);
            if (!string.IsNullOrEmpty(obj.JustificationToSkip))
                ctx.AddInParameter<string>(cmd, "@JustificationToSkip", obj.JustificationToSkip);
            if (!string.IsNullOrEmpty(obj.CorrectError))
                ctx.AddInParameter<string>(cmd, "@CorrectError", obj.CorrectError);
            if (!string.IsNullOrEmpty(obj.CorrectiveAction))
                ctx.AddInParameter<string>(cmd, "@CorrectiveAction", obj.CorrectiveAction);
            if (!string.IsNullOrEmpty(obj.AnalysisProposal))
                ctx.AddInParameter<string>(cmd, "@AnalysisProposal", obj.AnalysisProposal);
            if (!string.IsNullOrEmpty(obj.AnalysisJustification))
                ctx.AddInParameter<string>(cmd, "@AnalysisJustification", obj.AnalysisJustification);
            if (!string.IsNullOrEmpty(obj.RootCauseRelatedTo))
                ctx.AddInParameter<string>(cmd, "@RootCauseRelatedTo", obj.RootCauseRelatedTo);
            if (!string.IsNullOrEmpty(obj.ObviousRootcause))
                ctx.AddInParameter<string>(cmd, "@ObviousRootcause", obj.ObviousRootcause);
            if (obj.PhaseID > default(int))
                ctx.AddInParameter<int>(cmd, "@PhaseID", obj.PhaseID);
            if (!string.IsNullOrEmpty(obj.ProposeCapa))
                ctx.AddInParameter<string>(cmd, "@ProposeCapa", obj.ProposeCapa);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);

            ctx.AddOutParameter(cmd, "@ResultFlag", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            resultFlag = ctx.GetOutputParameterValue(cmd, "@ResultFlag");

            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return resultFlag;
        }

        public GetOOSHypoTestingResult GetOOSHypoTestingResult(OOSGetHypoTestingInfo obj)
        {
            var list = new GetOOSHypoTestingResult();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSGetHypoTestingInfo");
            ctx.AddInParameter<int>(cmd, "@OOSTestID", obj.OOSTestID);
            ctx.AddInParameter<int>(cmd, "@OOSTestDetailID", obj.OOSTestDetailID);

            using (var reader = cmd.ExecuteReader())
            {
                var getHyList = ((IObjectContextAdapter)context).ObjectContext.Translate<GetOOSHypoTestingResult>(reader);
                foreach (var rr in getHyList)
                    list = rr;

                reader.NextResult();

                list.OOSHypoTestingPhasesDetails = new List<OOSHypoTestingPhasesDetails>();
                var getHyPhaseList = ((IObjectContextAdapter)context).ObjectContext.Translate<OOSHypoTestingPhasesDetails>(reader);
                foreach (var rr in getHyPhaseList)
                    list.OOSHypoTestingPhasesDetails.Add(rr);

                reader.NextResult();

                list.OOSPhasesMaster = new List<OOSPhasesMaster>();
                var getPhaseMas = ((IObjectContextAdapter)context).ObjectContext.Translate<OOSPhasesMaster>(reader);
                foreach (var rr in getPhaseMas)
                    list.OOSPhasesMaster.Add(rr);

                reader.NextResult();

                list.OOSPhasesCondition = new List<OOSPhasesCondition>();
                var getPhaseCon = ((IObjectContextAdapter)context).ObjectContext.Translate<OOSPhasesCondition>(reader);
                foreach (var rr in getPhaseCon)
                    list.OOSPhasesCondition.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return list;
        }

        public OOSManageHypoResults OOSManageHypoTestingPhases(OOSManageHypoTestingPhases obj, TransResults tr)
        {
            var list = new OOSManageHypoResults();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSManageHypoTestingPhases");

            ctx.AddInParameter<int>(cmd, "@OOSTestID", obj.OOSTestID);
            ctx.AddInParameter<int>(cmd, "@OOSTestDetailID", obj.OOSTestDetailID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<string>(cmd, "@Action", obj.Action);
            if (obj.HypoTestPhaseID > default(int))
                ctx.AddInParameter<int>(cmd, "@HypoTestPhaseID", obj.HypoTestPhaseID);
            if (obj.PhaseID > default(int))
                ctx.AddInParameter<int>(cmd, "@PhaseID", obj.PhaseID);
            using (var reader = cmd.ExecuteReader())
            {
                list.ResultFlag = string.Empty;
                var resultFlag = ((IObjectContextAdapter)context).ObjectContext.Translate<string>(reader);
                foreach (var rr in resultFlag)
                    list.ResultFlag = rr;

                reader.NextResult();

                list.OOSHypoTestingPhasesDetails = new List<OOSHypoTestingPhasesDetails>();
                var hypList = ((IObjectContextAdapter)context).ObjectContext.Translate<OOSHypoTestingPhasesDetails>(reader);
                foreach (var rr in hypList)
                    list.OOSHypoTestingPhasesDetails.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return list;
        }

        public TestingSameSample OOSTestingOfNewPortionOfSameSampleResult(OOSGetHypoTestingInfo obj)
        {
            TestingSameSample data = new TestingSameSample();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSGetNewPortionofSameSampleResult");

            ctx.AddInParameter<int>(cmd, "@OOSTestID", obj.OOSTestID);
            ctx.AddInParameter<int>(cmd, "@OOSTestDetailID", obj.OOSTestDetailID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TestingSameSample>(reader);
                foreach (var rr in rrRes)
                    data = rr;

                reader.NextResult();

                var rrCon = ((IObjectContextAdapter)context).ObjectContext.Translate<OosProcessCondition>(reader);
                data.ConditionList = new List<OosProcessCondition>();
                foreach (var rr in rrCon)
                    data.ConditionList.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public GetOOSSingleAndCatBDetails GetOOSSingleAndCatBDetails(int OOSTestDetailID)
        {
            var list = new GetOOSSingleAndCatBDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSGetSingleAndCatBDetails");
            ctx.AddInParameter<int>(cmd, "@OOSTestDetailsID", OOSTestDetailID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetOOSSingleAndCatBDetails>(reader);
                foreach (var rr in rrRes)
                    list = rr;
                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return list;
        }

        public GetDeptReviewData GetDeptReviewDetails(int oosTestDetID)
        {
            GetDeptReviewData obj = new GetDeptReviewData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspGetDeptReviewDetails");
            ctx.AddInParameter<int>(cmd, "@OOSTestDetailID", oosTestDetID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetDeptReviewData>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();

                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<OOSDeptList>(reader);
                obj.DeptList = new List<OOSDeptList>();
                foreach (var rr in rrLst)
                    obj.DeptList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public RecordActionDetails ManageOOSDeptReview(OOSDeptReview obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSProcessDeptReview");
            ctx.AddInParameter<int>(cmd, "@OOSTestDetailID", obj.OOSTestDetailID);
            ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            ctx.AddInParameter<string>(cmd, "@DeptXML", obj.DeptListXml);
            if (!string.IsNullOrEmpty(obj.Validity))
                ctx.AddInParameter<string>(cmd, "@Validity", obj.Validity);
            if (!string.IsNullOrEmpty(obj.Status))
                ctx.AddInParameter<string>(cmd, "@Status", obj.Status);
            if (!string.IsNullOrEmpty(obj.OthDeptName))
                ctx.AddInParameter<string>(cmd, "@OthModName", obj.OthDeptName);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }

        public List<GetDepartmentWiseReviews> GetDepartmentWiseReview(int oosTestDetailID, int deptID)
        {
            var obj = new List<GetDepartmentWiseReviews>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSGetDeptRevComments");
            ctx.AddInParameter<int>(cmd, "@OOSTestDetailID", oosTestDetailID);
            ctx.AddInParameter<int>(cmd, "@DeptID", deptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetDepartmentWiseReviews>(reader);
                foreach (var rr in rrRes)
                    obj.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public RecordActionDetails ManageDepartmentComments(ManageDeptCommets obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.manageOOSDepartmentComments");
            ctx.AddInParameter<int>(cmd, "@OOSTestDetailID", obj.OOSTestDetailID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@DeptCommentXml", obj.DeptReviewXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }

        public List<ManufactureChecklist> GetManufactureChecklist(string category)
        {
            var lst = new List<ManufactureChecklist>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspGetChecklists");
            ctx.AddInParameter<string>(cmd, "@Category", category);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ManufactureChecklist>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ManufactureCheckPoints GetManufactureCheckPoints(int oosTestDetID, int phaseID)
        {
            ManufactureCheckPoints obj = new ManufactureCheckPoints();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSGetMfgCheckList");
            ctx.AddInParameter<int>(cmd, "@OOSTestDetID", oosTestDetID);
            ctx.AddInParameter<int>(cmd, "@PhaseID", phaseID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrCat = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQCOOSCheckList>(reader);
                obj.GetQCOOSCheckList = new List<GetQCOOSCheckList>();
                foreach (var rr in rrCat)
                    obj.GetQCOOSCheckList.Add(rr);

                reader.NextResult();
                var rrQue = ((IObjectContextAdapter)context).ObjectContext.Translate<GetCheckListQuestions>(reader);
                obj.GetCheckListQuestions = new List<GetCheckListQuestions>();
                foreach (var rr in rrQue)
                    obj.GetCheckListQuestions.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public MfgInvestigationDetails ManufactureInvestigationDetails(int oosTestDetID)
        {
            MfgInvestigationDetails obj = new MfgInvestigationDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspOOSGetMfginvestigationDetails");
            ctx.AddInParameter<int>(cmd, "@OOSTestDetailID", oosTestDetID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<MfgInvestigationDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public GetActionAndRptData ManageQASummaryInfo(ManageQASummaryInfo obj, TransResults tr)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspManageQASummaryInfo");

            ctx.AddInParameter<int>(cmd, "@OOSTestID", obj.OOSTestID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@CustomerID", obj.CustomerID);
            ctx.AddInParameter<string>(cmd, "@IsQualityAgreement", obj.IsQualityAgreement);
            ctx.AddInParameter<string>(cmd, "@IsOOSNotify", obj.IsOOSNotify);
            if (!string.IsNullOrEmpty(obj.QARemarks))
                ctx.AddInParameter<string>(cmd, "@QARemarks", obj.QARemarks);
            if (!string.IsNullOrEmpty(obj.DescNotification))
                ctx.AddInParameter<string>(cmd, "@DescNotification", obj.DescNotification);
            if (!string.IsNullOrEmpty(obj.CustomerName))
                ctx.AddInParameter<string>(cmd, "@CustomerName", obj.CustomerName);
            if (!string.IsNullOrEmpty(obj.ReasonForDelay))
                ctx.AddInParameter<string>(cmd, "@ReasonForDelay", obj.ReasonForDelay);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    retObj.Act = rr;

                reader.NextResult();

                retObj.RptList = new UploadReportList();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadReports>(reader);
                foreach (var rr in rrLst)
                    retObj.RptList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return retObj;
        }

        public GetQASummaryInfo GetQASummaryInfo(int oosTestID, int userRoleID)
        {
            var rrRes = new GetQASummaryInfo();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspGetQASummaryInfo");
            ctx.AddInParameter<int>(cmd, "@OOSTestID",oosTestID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", userRoleID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRead = ((IObjectContextAdapter)context).ObjectContext.Translate<GetQASummaryInfo>(reader);
                foreach (var rr in rrRead)
                    rrRes = rr;

                reader.NextResult();

                rrRes.recordActions = new RecordActionDetails();

                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrAct)
                    rrRes.recordActions = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return rrRes;
        }

        public RecordActionDetails ManageOOSSummaryInfo(ManageOOSSummaryInfo obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspManageQCSummaryInfo");
            ctx.AddInParameter<int>(cmd, "@OOSTestID", obj.OOSTestID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@IsLabInvestReviewed", obj.IsLabInvestReviewed);
            if (obj.RootCauseOOS > default(int))
                ctx.AddInParameter<int?>(cmd, "@RootCauseOOS", obj.RootCauseOOS);
            if (!string.IsNullOrEmpty(obj.OtherCause))
                ctx.AddInParameter<string>(cmd, "@OtherCause", obj.OtherCause);
            ctx.AddInParameter<string>(cmd, "@MfgInvstDone", obj.MfgInvstDone);
            ctx.AddInParameter<string>(cmd, "@RootCauseMfgInvestigation", obj.RootCauseMfgInvestigation);
            ctx.AddInParameter<string>(cmd, "@MfgChkAttached", obj.MfgChkAttached);
            ctx.AddInParameter<string>(cmd, "@RefDevInvestigation", obj.RefDevInvestigation);
            if (!string.IsNullOrEmpty(obj.SummaryOOS))
                ctx.AddInParameter<string>(cmd, "@SummaryOOS", obj.SummaryOOS);
            if (!string.IsNullOrEmpty(obj.CommentsIfAny))
                ctx.AddInParameter<string>(cmd, "@CommentsIfAny", obj.CommentsIfAny);
            if (!string.IsNullOrEmpty(obj.PraposePrevAction))
                ctx.AddInParameter<string>(cmd, "@PraposePrevAction", obj.PraposePrevAction);
            if (!string.IsNullOrEmpty(obj.ProcViolationJustification))
                ctx.AddInParameter<string>(cmd, "@ProcViolationJustification", obj.ProcViolationJustification);
            ctx.AddInParameter<string>(cmd, "@CheckListObservation", obj.CheckListObservation);
            if (!string.IsNullOrEmpty(obj.CheckListJustification))
                ctx.AddInParameter<string>(cmd, "@CheckListJustification", obj.CheckListJustification);
            ctx.AddInParameter<string>(cmd, "@ReAnaObservation", obj.ReAnaObservation);
            if (!string.IsNullOrEmpty(obj.ReAnaJustification))
                ctx.AddInParameter<string>(cmd, "@ReAnaJustification", obj.ReAnaJustification);
            ctx.AddInParameter<string>(cmd, "@ConfirmAnaObservation", obj.ConfirmAnaObservation);
            if (!string.IsNullOrEmpty(obj.ConfirmAnaJustification))
                ctx.AddInParameter<string>(cmd, "@ConfirmAnaJustification", obj.ConfirmAnaJustification);
            ctx.AddInParameter<string>(cmd, "@OOSInvestObservation", obj.OOSInvestObservation);
            if (!string.IsNullOrEmpty(obj.OOSInvestJustification))
                ctx.AddInParameter<string>(cmd, "@OOSInvestJustification", obj.OOSInvestJustification);
            if (!string.IsNullOrEmpty(obj.JustificationForDelay))
                ctx.AddInParameter<string>(cmd, "@JustificationForDelay", obj.JustificationForDelay);
            if (obj.DevID > default(int))
                ctx.AddInParameter<int?>(cmd, "@DevID", obj.DevID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }

        public ManageOOSSummaryInfo GetOOSSummaryInfo(int oosTestID)
        {
            ManageOOSSummaryInfo obj = new ManageOOSSummaryInfo();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspGetQCSummaryInfo");
            ctx.AddInParameter<int>(cmd, "@OOSTestID", oosTestID);
            using(var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ManageOOSSummaryInfo>(reader);
                foreach (var rr in rrRes)
                    obj = rr;

                reader.NextResult();
                var rrCat = ((IObjectContextAdapter)context).ObjectContext.Translate<string>(reader);
                foreach (var rr in rrCat)
                    obj.CatCode = rr;

                    reader.NextResult();
                var rrDev = ((IObjectContextAdapter)context).ObjectContext.Translate<OOSDevReq>(reader);
                obj.DevReqList = new List<OOSDevReq>();
                foreach (var rr in rrDev)
                    obj.DevReqList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public NewDeviationReg GenerateDeviationReq(int oosTestID, TransResults tr)
        {
            NewDeviationReg obj = new NewDeviationReg();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "dbo.uspOOSCreateAutoDeviationReq");
            ctx.AddInParameter<int>(cmd, "@OOS_TESTID", oosTestID);
            ctx.AddInParameter<int>(cmd, "@USER_ID", tr.UserID);
            ctx.AddInParameter<string>(cmd, "@MODULE_CODE", tr.DeptCode);

            using (var reader = cmd.ExecuteReader())
            {
                var rrDev = ((IObjectContextAdapter)context).ObjectContext.Translate<OOSDevReq>(reader);
                obj.DevReqList = new List<OOSDevReq>();
                foreach (var rr in rrDev)
                    obj.DevReqList.Add(rr);

                reader.NextResult();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<NewDeviationReg>(reader);
                foreach(var rr in rrRes)
                {
                    obj.DEV_ID = rr.DEV_ID;
                    obj.RETVAL = rr.RETVAL;
                }
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public List<NewSampleRequest> GetNewSampleRequests(int oosTestID)
        {
            List<NewSampleRequest> lst = new List<NewSampleRequest>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "oos.uspGetOOSNewSamples");
            ctx.AddInParameter<int>(cmd, "@OOSTestID", oosTestID);

            using(var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<NewSampleRequest>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }
    }
}
