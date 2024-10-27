using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SamplePlan;
using MedicalLIMSApi.Core.Interface.SamplePlan;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.SamplePlan
{
    public class SamplePlanRepository : ISamplePlan
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public RecordActionDetails CreatePlan(int shiftID, TransResults tr)
        {
            RecordActionDetails lt = new RecordActionDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspCreatePlan");
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<int>(cmd, "@ShiftID", shiftID);
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

        public RecordActionDetails SavePalnAnalyst(PlanAnalyst obj, TransResults tr)
        {
            var lt = new RecordActionDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspSaveAnalysts");
            ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<string>(cmd, "@XmlString", obj.XmlString);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lt = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lt;
        }

        public AnalystDetList AnalystDetails(int PlanID, TransResults tr)
        {
            var lst = new AnalystDetList();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetAnalysts");
            ctx.AddInParameter<int>(cmd, "@PlanID", PlanID);
            ctx.AddInParameter<int>(cmd, "@PlantID", tr.PlantID);
            using (var reader = cmd.ExecuteReader())
            {
                var AnalystDet = ((IObjectContextAdapter)context).ObjectContext.Translate<AnalystDet>(reader);
                foreach (var rr in AnalystDet)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public List<AnalystOccupancyDet> AnalystOccupancyDetails(int UserRoleID)
        {
            var lst = new List<AnalystOccupancyDet>();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetAnalystOccupancy");
            ctx.AddInParameter<int>(cmd, "@UserRoleID", UserRoleID);
            using (var reader = cmd.ExecuteReader())
            {
                var AnalystDet = ((IObjectContextAdapter)context).ObjectContext.Translate<AnalystOccupancyDet>(reader);
                foreach (var rr in AnalystDet)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SampleDetailsList GetSampleDetails(int PlanID, TransResults tr)
        {
            var lst = new SampleDetailsList();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetSamples");
            ctx.AddInParameter<int>(cmd, "@PlanID", PlanID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            using (var reader = cmd.ExecuteReader())
            {
                var SampleDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<SampleDetails>(reader);
                foreach (var rr in SampleDetails)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public RecordActionDetails SavePlanSamples(PlanSamples obj, TransResults tr)
        {
            var lt = new RecordActionDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspSaveSamples");
            ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<string>(cmd, "@XmlString", obj.XmlString);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lt = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lt;
        }

        public SamSpecDet GetSampleSpecificationsDet(int PlanID)
        {
            var lst = new SamSpecDet();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetSamplesWithMultipleSpecs");
            ctx.AddInParameter<int>(cmd, "@PlanID", PlanID);
            using (var reader = cmd.ExecuteReader())
            {
                lst.Sam = new List<SampleDet>();
                var SampleDet = ((IObjectContextAdapter)context).ObjectContext.Translate<SampleDet>(reader);
                foreach (var rr in SampleDet)
                    lst.Sam.Add(rr);

                reader.NextResult();

                lst.SamSpe = new List<SampleSpecifications>();
                var SampleSpecifications = ((IObjectContextAdapter)context).ObjectContext.Translate<SampleSpecifications>(reader);
                foreach (var rr in SampleSpecifications)
                    lst.SamSpe.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public RecordActionDetails ManageSampleSpecification(SampleSpecificationBO obj, TransResults Tr)
        {
            var lt = new RecordActionDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspSaveSampleSpecs");
            ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", Tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", Tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", Tr.DeptID);
            ctx.AddInParameter<string>(cmd, "@XmlString", obj.XmlString);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lt = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lt;
        }

        public PalnDetails GetPalnDetailsByID(int PlanID, TransResults Tr)
        {
            var lst = new PalnDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetPlanDetailsByID");
            ctx.AddInParameter<int>(cmd, "@PlanID", PlanID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", Tr.UserRoleID);
            using (var reader = cmd.ExecuteReader())
            {
                lst.plan = new PalnDet();
                var PalnDet = ((IObjectContextAdapter)context).ObjectContext.Translate<PalnDet>(reader);
                foreach (var rr in PalnDet)
                    lst.plan = rr;

                reader.NextResult();

                lst.rec = new RecordActionDetails();
                var RecordActionDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in RecordActionDetails)
                    lst.rec = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetTestActivityDet GetTestsAndActivitySamples(int PlanID)
        {
            var lst = new GetTestActivityDet();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetTestsAndActivitySamples");
            ctx.AddInParameter<int>(cmd, "@PlanID", PlanID);
            using (var reader = cmd.ExecuteReader())
            {
                lst.Sam = new SamplingActivityList();
                var SamplingActivity = ((IObjectContextAdapter)context).ObjectContext.Translate<SamplingActivity>(reader);
                foreach (var rr in SamplingActivity)
                    lst.Sam.Add(rr);

                reader.NextResult();

                lst.WetIns = new WetInstrumentaionList();
                var WetInstrumentaion = ((IObjectContextAdapter)context).ObjectContext.Translate<WetInstrumentaion>(reader);
                foreach (var rr in WetInstrumentaion)
                    lst.WetIns.Add(rr);

                reader.NextResult();

                lst.inv = new InvalidationsList();
                var Invalidations = ((IObjectContextAdapter)context).ObjectContext.Translate<Invalidation>(reader);
                foreach (var rr in Invalidations)
                    lst.inv.Add(rr);

                reader.NextResult();

                lst.MasterList = new MasterTestList();
                var MasterList = ((IObjectContextAdapter)context).ObjectContext.Translate<MasterTestBO>(reader);
                foreach (var rr in MasterList)
                    lst.MasterList.Add(rr);

                reader.NextResult();

                lst.OosTestList = new OOSTestList();
                var oosLst = ((IObjectContextAdapter)context).ObjectContext.Translate<OOSTestBO>(reader);
                foreach (var rr in oosLst)
                    lst.OosTestList.Add(rr);

                reader.NextResult();

                lst.DrList = new DataReviewList();
                var drlst = ((IObjectContextAdapter)context).ObjectContext.Translate<DataReviewBO>(reader);
                foreach (var rr in drlst)
                    lst.DrList.Add(rr);

                reader.NextResult();

                lst.CalibList = new CalibrationList();
                var callst = ((IObjectContextAdapter)context).ObjectContext.Translate<CalibrationBO>(reader);
                foreach (var rr in callst)
                    lst.CalibList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;

        }

        public RecordActionDetails ManageTestActivities(ManageSamplingTestModel obj, TransResults Tr)
        {
            var lt = new RecordActionDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspSaveTestsAndActivitySamples");
            ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", Tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", Tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", Tr.DeptID);
            if (!string.IsNullOrEmpty(obj.SamplesXml))
                ctx.AddInParameter<string>(cmd, "@SamplesXml", obj.SamplesXml);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lt = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lt;
        }

        public string AutoPlan(int PlanID, string type, TransResults tr)
        {
            string retMsg = string.Empty;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samPlan.uspAutoPlan");
            ctx.AddInParameter<int>(cmd, "@PlanID", PlanID);
            ctx.AddInParameter<string>(cmd, "@Type", type);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetMsg", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public AutoPalnDetails PlanDetails(int PlanID)
        {
            var lst = new AutoPalnDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetPlan");
            ctx.AddInParameter<int>(cmd, "@PlanID", PlanID);
            using (var reader = cmd.ExecuteReader())
            {
                lst.Plandet = new List<Core.Entities.SamplePlan.PlanDetails>();
                var PlanDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<PlanDetails>(reader);
                foreach (var rr in PlanDetails)
                    lst.Plandet.Add(rr);

                reader.NextResult();

                lst.UserPlanDet = new List<UserPalnDetails>();
                var UserPalnDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<UserPalnDetails>(reader);
                foreach (var rr in UserPalnDetails)
                    lst.UserPlanDet.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string ChangeAnalyst(ChangeAnalyst obj, TransResults tr)
        {
            string retMsg = string.Empty;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samPlan.uspChangeAnalyst");
            ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            ctx.AddInParameter<int>(cmd, "@TestID", obj.TestID);
            ctx.AddInParameter<int>(cmd, "@ExtUserRoleID", obj.UserRoleID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddOutParameter(cmd, "@RetMsg", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public RecordActionDetails DeleteTestSample(TestInfo obj, TransResults tr)
        {
            var lt = new RecordActionDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspDeleteTestSample");
            ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            if (obj.UserTestID > default(int))
                ctx.AddInParameter<int>(cmd, "@UserTestID", obj.UserTestID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            if (obj.TestID > default(int))
                ctx.AddInParameter<int>(cmd, "@TestID", obj.TestID);
            if (!string.IsNullOrEmpty(obj.SamplesXml))
                ctx.AddInParameter<string>(cmd, "@SampleXml", obj.SamplesXml);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lt = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lt;
        }

        public PlanActivitiesList PlanActivities(PlanActivityBo obj)
        {
            var lst = new PlanActivitiesList();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetPlanActivities");
            if (obj.PlanID > default(int))
                ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", obj.UserRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var PlanActivities = ((IObjectContextAdapter)context).ObjectContext.Translate<PlanActivities>(reader);
                foreach (var rr in PlanActivities)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public RecordActionDetails UnAssignUserTests(UserTestBo obj, TransResults tr)
        {
            var lt = new RecordActionDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspUnAssignUserTests");
            if (obj.PlanID > default(int))
                ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            ctx.AddInParameter<int>(cmd, "@UserTestID", obj.UserTestID);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lt = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lt;
        }

        public AssignActivityList AssignActivityToUser(AssignActivityBo obj, TransResults tr)
        {
            var lst = new AssignActivityList();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspAssignActivityToUser");
            if (obj.PlanID > default(int))
                ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@ActivityCode", obj.ActivityCode);
            ctx.AddInParameter<int>(cmd, "@ActivityActualID", obj.ActivityActualID);
            ctx.AddInParameter<string>(cmd, "@ActivityDesc", obj.ActivityDesc);
            ctx.AddInParameter<int>(cmd, "@AssignUserRoleID", obj.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            if (!string.IsNullOrEmpty(obj.RefNumber))
                ctx.AddInParameter<string>(cmd, "@ReferenceNumber", obj.RefNumber);
            if (!string.IsNullOrEmpty(obj.MaterialName))
                ctx.AddInParameter<string>(cmd, "@MaterialName", obj.MaterialName);

            ctx.AddInParameter<short>(cmd, "@OccInMinutes", obj.OccupancyMin);

            using (var reader = cmd.ExecuteReader())
            {
                lst.Rec = new RecordActionDetails();
                var RecordActionDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in RecordActionDetails)
                    lst.Rec = rr;

                reader.NextResult();

                lst.Act = new PlanActivitiesList();
                var PlanActivities = ((IObjectContextAdapter)context).ObjectContext.Translate<PlanActivities>(reader);
                foreach (var rr in PlanActivities)
                    lst.Act.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public PlanPendingActivitiesList GetPendingActivities(TransResults tr)
        {
            PlanPendingActivitiesList lst = new PlanPendingActivitiesList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetPendingPlanActivities");
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            using (var reader = cmd.ExecuteReader())
            {
                var PlanActivities = ((IObjectContextAdapter)context).ObjectContext.Translate<PlanPendingActivities>(reader);
                foreach (var rr in PlanActivities)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public string AddCommentForCompletedTask(TestCompletedComment obj)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspAddCommentForCompletedTask");

            ctx.AddInParameter<int>(cmd, "@UserTestID", obj.UserTestID);
            ctx.AddInParameter<string>(cmd, "@Commnet", obj.Comment);
            ctx.AddOutParameter(cmd, "@RetCode", System.Data.DbType.String, 15);

            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retMsg;
        }

        public ShiftActivities GetShiftCloseActivities(int shiftID, TransResults tr)
        {
            ShiftActivities lst = new ShiftActivities();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetShiftCloseActivities");
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            if (shiftID > default(int))
                ctx.AddInParameter<int>(cmd, "@ShiftID", shiftID);
            using (var reader = cmd.ExecuteReader())
            {
                lst.Act = new RecordActionDetails();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lst.Act = rr;

                reader.NextResult();

                var PlanActivities = ((IObjectContextAdapter)context).ObjectContext.Translate<PlanPendingActivities>(reader);
                lst.List = new PlanPendingActivitiesList();
                foreach (var rr in PlanActivities)
                    lst.List.Add(rr);

                reader.NextResult();

                var rrReq = ((IObjectContextAdapter)context).ObjectContext.Translate<ShiftActivities>(reader);
                foreach (var rr in rrReq)
                {
                    lst.RequestCode = rr.RequestCode;
                    lst.Status = rr.Status;
                    lst.UserName = rr.UserName;
                    lst.RequestDate = rr.RequestDate;
                    lst.Assessment = rr.Assessment;
                    lst.Observations = rr.Observations;
                }

                reader.NextResult();

                lst.PlanLst = new List<PlanBO>();
                var rrPlan = ((IObjectContextAdapter)context).ObjectContext.Translate<PlanBO>(reader);
                foreach (var rr in rrPlan)
                    lst.PlanLst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return lst;
        }

        public RecordActionDetails ManageShiftCloseActivities(ShiftCloseActivities obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspManageShiftCloseActivities");
            if (obj.ShiftID > default(int))
                ctx.AddInParameter<int>(cmd, "@ShiftID", obj.ShiftID);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@ShiftCloseXml", obj.ShiftCloseXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            if (!string.IsNullOrEmpty(obj.InchargeAssessment))
                ctx.AddInParameter<string>(cmd, "@Assessment", obj.InchargeAssessment);
            if (!string.IsNullOrEmpty(obj.Observation))
                ctx.AddInParameter<string>(cmd, "@Observations", obj.Observation);

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

        public ViewSamplePlanDetails ViewSamplePlanDetailsByID(int planID, short plantID)
        {

            var lst = new ViewSamplePlanDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspViewSamplePlanDetails");
            ctx.AddInParameter<int>(cmd, "@PlanID", planID);
            ctx.AddInParameter<short>(cmd, "@PlantID", plantID);
            using (var reader = cmd.ExecuteReader())
            {

                lst.PlanDetails = new PalnDet();
                var rrPlanDetailsRes = ((IObjectContextAdapter)context).ObjectContext.Translate<PalnDet>(reader);
                foreach (var rr in rrPlanDetailsRes)
                    lst.PlanDetails = rr;

                reader.NextResult();

                lst.Analysts = new AnalystDetList();
                var rrAnalystsRes = ((IObjectContextAdapter)context).ObjectContext.Translate<AnalystDet>(reader);
                foreach (var rr in rrAnalystsRes)
                    lst.Analysts.Add(rr);

                reader.NextResult();

                lst.Sampling = new SampleDetailsList();
                var rrSampleRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SampleDetails>(reader);
                foreach (var rr in rrSampleRes)
                    lst.Sampling.Add(rr);

                reader.NextResult();

                lst.SampleSpec = new SamSpecDet();
                lst.SampleSpec.Sam = new List<SampleDet>();
                var rrSampleInfoRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SampleDet>(reader);
                foreach (var rr in rrSampleInfoRes)
                    lst.SampleSpec.Sam.Add(rr);

                reader.NextResult();

                lst.SampleSpec.SamSpe = new List<SampleSpecifications>();
                var rrSampleSpecInfoRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SampleSpecifications>(reader);
                foreach (var rr in rrSampleSpecInfoRes)
                    lst.SampleSpec.SamSpe.Add(rr);

                reader.NextResult();

                lst.SampleTestAct = new GetTestActivityDet();
                lst.SampleTestAct.Sam = new SamplingActivityList();
                var rrSampleTestSamRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SamplingActivity>(reader);
                foreach (var rr in rrSampleTestSamRes)
                    lst.SampleTestAct.Sam.Add(rr);

                reader.NextResult();

                lst.SampleTestAct.WetIns = new WetInstrumentaionList();
                var rrSampleTestWetRes = ((IObjectContextAdapter)context).ObjectContext.Translate<WetInstrumentaion>(reader);
                foreach (var rr in rrSampleTestWetRes)
                    lst.SampleTestAct.WetIns.Add(rr);

                reader.NextResult();

                lst.SampleTestAct.inv = new InvalidationsList();
                var rrSampleTestInvRes = ((IObjectContextAdapter)context).ObjectContext.Translate<Invalidation>(reader);
                foreach (var rr in rrSampleTestInvRes)
                    lst.SampleTestAct.inv.Add(rr);

                reader.NextResult();

                lst.Planning = new AutoPalnDetails();
                lst.Planning.Plandet = new List<PlanDetails>();
                var rrPlanningRes = ((IObjectContextAdapter)context).ObjectContext.Translate<PlanDetails>(reader);
                foreach (var rr in rrPlanningRes)
                    lst.Planning.Plandet.Add(rr);

                reader.NextResult();

                lst.Planning.UserPlanDet = new List<UserPalnDetails>();
                var rrPlanningUserRes = ((IObjectContextAdapter)context).ObjectContext.Translate<UserPalnDetails>(reader);
                foreach (var rr in rrPlanningUserRes)
                    lst.Planning.UserPlanDet.Add(rr);

                reader.NextResult();

                lst.MasterList = new MasterTestList();
                var MasterList = ((IObjectContextAdapter)context).ObjectContext.Translate<MasterTestBO>(reader);
                foreach (var rr in MasterList)
                    lst.MasterList.Add(rr);

                reader.NextResult();

                lst.OosTestList = new OOSTestList();
                var oosLst = ((IObjectContextAdapter)context).ObjectContext.Translate<OOSTestBO>(reader);
                foreach (var rr in oosLst)
                    lst.OosTestList.Add(rr);

                reader.NextResult();

                lst.DrList = new DataReviewList();
                var drlst = ((IObjectContextAdapter)context).ObjectContext.Translate<DataReviewBO>(reader);
                foreach (var rr in drlst)
                    lst.DrList.Add(rr);

                reader.NextResult();

                lst.CalibList = new CalibrationList();
                var callst = ((IObjectContextAdapter)context).ObjectContext.Translate<CalibrationBO>(reader);
                foreach (var rr in callst)
                    lst.CalibList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SearchResults<SearchPlanresult> SearchSamplePlan(SearchPlanBO obj)
        {
            var lst = new SearchResults<SearchPlanresult>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspSearchSamplePlan");
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            if (obj.SamplePlanIDFrom > default(int))
                ctx.AddInParameter<int>(cmd, "@SamplePlanID", obj.SamplePlanIDFrom);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateFrom", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateTo", obj.DateTo);
            ctx.AddInParameter<short>(cmd, "@PlantID", obj.PlantID);
            if (obj.ArID > default(int))
                ctx.AddInParameter<int>(cmd, "@ArID", obj.ArID);
            if (obj.SampleID > default(int))
                ctx.AddInParameter<int>(cmd, "@SampleID", obj.SampleID);
            if (obj.TestID > default(int))
                ctx.AddInParameter<int>(cmd, "@TestID", obj.TestID);
            if (obj.MatID > default(int))
                ctx.AddInParameter<int>(cmd, "@MatID", obj.MatID);
            if (obj.AnalystID > default(int))
                ctx.AddInParameter<int>(cmd, "@AnalystID", obj.AnalystID);
            if (obj.SamplePlanIDTo > default(int))
                ctx.AddInParameter<int>(cmd, "@SamplePlanIDTo", obj.SamplePlanIDTo);
            if (obj.SpecificationID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecificationID", obj.SpecificationID);
            if (obj.PlanCreatedUserRoleID > default(int))
                ctx.AddInParameter<int>(cmd, "@PlanCreatedUserRoleID", obj.PlanCreatedUserRoleID);
            if (obj.PlanCreatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@PlanCreatedOn", obj.PlanCreatedOn);
            if (obj.ShiftID > default(int))
                ctx.AddInParameter<int>(cmd,"@ShiftID",obj.ShiftID);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);

            using (var reader = cmd.ExecuteReader())
            {
                var rrTo = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var rr in rrTo)
                    lst.TotalNumberOfRows = rr;

                reader.NextResult();

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<SearchPlanresult>(reader);
                List<SearchPlanresult> item = new List<SearchPlanresult>();
                foreach (var rr in rrRes)
                    item.Add(rr);
                lst.SearchList = item;

                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public RecordActionDetails ChangeUserPlanTest(ChangeUserTestPlan obj, TransResults tr)
        {
            var lst = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspSPChangeUserPlanTest");
            if (obj.PlanID > default(int))
                ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            if (obj.UserTestID > default(int))
                ctx.AddInParameter<int>(cmd, "@UserTestID", obj.UserTestID);
            if (obj.TestID > default(int))
                ctx.AddInParameter<int>(cmd, "@TestID", obj.TestID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@AssignedUserRoleID", obj.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);

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

        public TestOccupanyDetails ManageTestOccupany(TestOccBO obj, TransResults tr)
        {
            var lst = new TestOccupanyDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspManageMasterTestOccupancy");
            if (obj.OccMinutes > default(short))
                ctx.AddInParameter<short?>(cmd, "@OccMinutes", obj.OccMinutes);
            if (obj.TestID > default(int))
                ctx.AddInParameter<int>(cmd, "@TestID", obj.TestID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            using (var reader = cmd.ExecuteReader())
            {
                lst.TestList = new TestOccXmlList();

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<TestOccBO>(reader);
                foreach (var rr in rrRes)

                    lst.TestList.Add(rr);
                reader.NextResult();

                lst.ReturnFlag = string.Empty;
                if (reader.HasRows)
                {

                    var rRes = ((IObjectContextAdapter)context).ObjectContext.Translate<string>(reader);
                    foreach (var rr in rRes)

                        lst.ReturnFlag = rr;
                }


                cmd.Connection.Close();

            }
            ctx.CloseConnection(context);
            return lst;
        }
        public SearchResults<GetSearchCloseShiftActivitiesDetails> SearchCloseShiftActivities(SearchCloseShiftActivities obj, TransResults trn)
        {
            var lst = new SearchResults<GetSearchCloseShiftActivitiesDetails>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspSearchCloseShiftActivities");

            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateFrom", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateTo", obj.DateTo);
            if (obj.CreatedUserRoleID > default(int))
                ctx.AddInParameter<int>(cmd, "@CreatedUserRoleID", obj.CreatedUserRoleID);
            if (obj.ShiftIDFrom > default(int))
                ctx.AddInParameter<int>(cmd, "@ShiftIDFrom", obj.ShiftIDFrom);
            if (obj.ShiftIDTo > default(int))
                ctx.AddInParameter<int>(cmd, "@ShiftIDTo", obj.ShiftIDTo);
            ctx.AddInParameter<int>(cmd, "@LoginUserRoleID", trn.UserRoleID);
            ctx.AddInParameter<bool>(cmd, "@IsHODApp", obj.IsHODApp);
            using (var reader = cmd.ExecuteReader())
            {
                var rrToNo = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var rr in rrToNo)
                    lst.TotalNumberOfRows = rr;

                reader.NextResult();

                List<GetSearchCloseShiftActivitiesDetails> item = new List<GetSearchCloseShiftActivitiesDetails>();

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSearchCloseShiftActivitiesDetails>(reader);
                foreach (var rr in rrRes)
                    item.Add(rr);

                lst.SearchList = item;

                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);

            return lst;
        }

        public GetCloseShiftActivitiesByID GetCloseShiftActivitiesByID(int shiftID)
        {
            var obj = new GetCloseShiftActivitiesByID();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspGetCloseShiftActivitiesByID");

            ctx.AddInParameter<int>(cmd, "@ShiftID", shiftID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrCloS = ((IObjectContextAdapter)context).ObjectContext.Translate<GetCloseShiftActivitiesByID>(reader);
                foreach (var rr in rrCloS)
                    obj = rr;

                reader.NextResult();

                obj.GetShiftActivitiesList = new List<GetShiftActivities>();
                var rrShi = ((IObjectContextAdapter)context).ObjectContext.Translate<GetShiftActivities>(reader);
                foreach (var rr in rrShi)
                    obj.GetShiftActivitiesList.Add(rr);

                reader.NextResult();

                obj.PlanLst = new List<PlanBO>();
                var rrPlan = ((IObjectContextAdapter)context).ObjectContext.Translate<PlanBO>(reader);
                foreach (var rr in rrPlan)
                    obj.PlanLst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return obj;
        }

        public RecordActionDetails ManageUserTestActivityStatus(ManageActivityStatusBO obj, TransResults tr)
        {
            RecordActionDetails act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "samplan.uspManageUserTestActivityStatus");

            ctx.AddInParameter<int>(cmd, "@PlanID", obj.PlanID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<string>(cmd, "@ActvityXML", obj.ActivityXml);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            using(var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    act = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return act;
        }
    }
}
