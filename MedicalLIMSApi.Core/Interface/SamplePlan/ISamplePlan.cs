using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SamplePlan;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Interface.SamplePlan
{
    public interface ISamplePlan
    {
        RecordActionDetails CreatePlan(int shiftID, TransResults tr);

        RecordActionDetails SavePalnAnalyst(PlanAnalyst obj, TransResults tr);

        AnalystDetList AnalystDetails(int PlanID, TransResults tr);

        List<AnalystOccupancyDet> AnalystOccupancyDetails(int UserRoleID);

        SampleDetailsList GetSampleDetails(int PlanID, TransResults tr);

        RecordActionDetails SavePlanSamples(PlanSamples obj, TransResults tr);

        SamSpecDet GetSampleSpecificationsDet(int PlanID);

        RecordActionDetails ManageSampleSpecification(SampleSpecificationBO obj, TransResults tr);

        PalnDetails GetPalnDetailsByID(int PlanID, TransResults tr);

        GetTestActivityDet GetTestsAndActivitySamples(int PlanID);

        RecordActionDetails ManageTestActivities(ManageSamplingTestModel obj, TransResults tr);

        string AutoPlan(int PlanID, string type, TransResults tr);

        AutoPalnDetails PlanDetails(int PlanID);

        string ChangeAnalyst(ChangeAnalyst obj, TransResults tr);

        RecordActionDetails DeleteTestSample(TestInfo obj, TransResults tr);

        PlanActivitiesList PlanActivities(PlanActivityBo obj);

        RecordActionDetails UnAssignUserTests(UserTestBo obj, TransResults tr);

        AssignActivityList AssignActivityToUser(AssignActivityBo obj, TransResults tr);

        PlanPendingActivitiesList GetPendingActivities(TransResults tr);

        string AddCommentForCompletedTask(TestCompletedComment obj);

        ShiftActivities GetShiftCloseActivities(int shiftID, TransResults tr);

        RecordActionDetails ManageShiftCloseActivities(ShiftCloseActivities obj, TransResults tr);

        ViewSamplePlanDetails ViewSamplePlanDetailsByID(int planID, short plantID);

        SearchResults<SearchPlanresult> SearchSamplePlan(SearchPlanBO obj);

        RecordActionDetails ChangeUserPlanTest(ChangeUserTestPlan obj, TransResults tr);

        TestOccupanyDetails ManageTestOccupany(TestOccBO obj, TransResults tr);

        SearchResults<GetSearchCloseShiftActivitiesDetails> SearchCloseShiftActivities(SearchCloseShiftActivities obj, TransResults trn);

        GetCloseShiftActivitiesByID GetCloseShiftActivitiesByID(int shiftID);

        RecordActionDetails ManageUserTestActivityStatus(ManageActivityStatusBO obj, TransResults tr);
    }
}
