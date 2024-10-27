using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SamplePlan;
using MedicalLIMSApi.Core.Interface.SamplePlan;
using System.Collections.Generic;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.SamplePlan
{
    public class SamplePlanController : ApiController
    {
        ISamplePlan db;

        public SamplePlanController(ISamplePlan db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("CreatePlan")]
        public RecordActionDetails CreatePlan(int shiftID)
        {
            return db.CreatePlan(shiftID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SaveAnalyst")]
        public RecordActionDetails SavePalnAnalyst(PlanAnalyst obj)
        {
            return db.SavePalnAnalyst(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetAnalystDetails")]
        public AnalystDetList AnalystDetails(string encPlanID)
        {
            return db.AnalystDetails(CommonStaticMethods.Decrypt<int>(encPlanID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetAnalystOccupancyDetails")]
        public List<AnalystOccupancyDet> AnalystOccupancyDetails(string encUserRoleID)
        {
            return db.AnalystOccupancyDetails(CommonStaticMethods.Decrypt<int>(encUserRoleID));
        }

        [HttpGet]
        [Route("GetSampleDetails")]
        public SampleDetailsList GetSampleDetails(string encPlanID)
        {
            return db.GetSampleDetails(CommonStaticMethods.Decrypt<int>(encPlanID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SavePlanSamples")]
        public RecordActionDetails SavePlanSamples(PlanSamples obj)
        {
            return db.SavePlanSamples(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetSampleSpecificationsDet")]
        public SamSpecDet GetSampleSpecificationsDet(string encPlanID)
        {
            return db.GetSampleSpecificationsDet(CommonStaticMethods.Decrypt<int>(encPlanID));
        }

        [HttpPost]
        [Route("ManageSampleSpecification")]
        public RecordActionDetails ManageSampleSpecification(SampleSpecificationBO obj)
        {
            return db.ManageSampleSpecification(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetPlanningDetails")]
        public PalnDetails GetPalnDetailsByID(string encPlanID)
        {
            return db.GetPalnDetailsByID(CommonStaticMethods.Decrypt<int>(encPlanID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetTestsAndActivitySamples")]
        public GetTestActivityDet GetTestsAndActivitySamples(string encPlanID)
        {
            return db.GetTestsAndActivitySamples(CommonStaticMethods.Decrypt<int>(encPlanID));
        }

        [HttpPost]
        [Route("ManageTestActivities")]
        public RecordActionDetails ManageTestActivities(ManageSamplingTestModel obj)
        {
            return db.ManageTestActivities(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("AutoPlan")]
        public string AutoPlan(string encPlanID, string type)
        {
            return db.AutoPlan(CommonStaticMethods.Decrypt<int>(encPlanID), type, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("PlanDetails")]
        public AutoPalnDetails PlanDetails(string encPlanID)
        {
            return db.PlanDetails(CommonStaticMethods.Decrypt<int>(encPlanID));
        }

        [HttpPost]
        [Route("ChangeAnalyst")]
        public string ChangeAnalyst(ChangeAnalyst obj)
        {
            return db.ChangeAnalyst(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }


        [HttpPost]
        [Route("DeleteTestSample")]
        public RecordActionDetails DeleteTestSample(TestInfo obj)
        {
            return db.DeleteTestSample(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("PlanActivities")]
        public PlanActivitiesList PlanActivities(PlanActivityBo obj)
        {
            return db.PlanActivities(obj);
        }

        [HttpPost]
        [Route("UnAssignUserTests")]
        public RecordActionDetails UnAssignUserTests(UserTestBo obj)
        {
            return db.UnAssignUserTests(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("AssignActivityToUser")]
        public AssignActivityList AssignActivityToUser(AssignActivityBo obj)
        {
            return db.AssignActivityToUser(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetPendingActivities")]
        public PlanPendingActivitiesList GetPendingActivities()
        {
            return db.GetPendingActivities(MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("AddCommentForCompletedTask")]
        public string AddCommentForCompletedTask(TestCompletedComment obj)
        {
            return db.AddCommentForCompletedTask(obj);
        }

        [HttpGet]
        [Route("GetShiftCloseActivities")]
        public ShiftActivities GetShiftCloseActivities(string encShiftID)
        {
            int shiftID = default(int);
            shiftID = CommonStaticMethods.Decrypt<int>(encShiftID);
            return db.GetShiftCloseActivities(shiftID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageShiftCloseActivities")]
        public RecordActionDetails ManageShiftCloseActivities(ShiftCloseActivities obj)
        {
            return db.ManageShiftCloseActivities(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("ViewSamplePlanDetailsByID")]
        public ViewSamplePlanDetails ViewSamplePlanDetailsByID(string encPlanID)
        {
            return db.ViewSamplePlanDetailsByID(CommonStaticMethods.Decrypt<int>(encPlanID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID);
        }

        [HttpPost]
        [Route("SearchSamplePlan")]
        public SearchResults<SearchPlanresult> SearchSamplePlan(SearchPlanBO obj)
        {
            obj.PlantID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID;
            return db.SearchSamplePlan(obj);
        }

        [HttpPost]
        [Route("ChangeUserPlanTest")]
        public RecordActionDetails ChangeUserPlanTest(ChangeUserTestPlan obj)
        {
            return db.ChangeUserPlanTest(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageTestOccupancy")]
        public TestOccupanyDetails ManageTestOccupany(TestOccBO obj)
        {
            return db.ManageTestOccupany(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SearchCloseShiftActivities")]
        public SearchResults<GetSearchCloseShiftActivitiesDetails> SearchCloseShiftActivities(SearchCloseShiftActivities obj)
        {
            return db.SearchCloseShiftActivities(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetCloseShiftActivitiesByID")]
        public GetCloseShiftActivitiesByID GetCloseShiftActivitiesByID(string encShiftID)
        {
            return db.GetCloseShiftActivitiesByID(CommonStaticMethods.Decrypt<int>(encShiftID));
        }

        [HttpPost]
        [Route("ManageUserTestActivityStatus")]
        public RecordActionDetails ManageUserTestActivityStatus(ManageActivityStatusBO obj)
        {
            return db.ManageUserTestActivityStatus(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

    }
}
