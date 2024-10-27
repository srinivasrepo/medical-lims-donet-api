using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.OOS;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Interface.OOS
{
    public interface IOos
    {
        SearchResults<GetSearchOOSTestDetails> OOSSearchOOSTestDetails(SearchOOSTestDetails obj, TransResults tr);

        TestInfo GetTestInfo(OOSGetHypoTestingInfo obj, TransResults tr);

        RecordActionDetails UpdateOOSSummary(UpdateSummary obj, TransResults tr);

        GetOOSPhase1CheckList GetOOSPhase1CheckList(int OosDetailID);

        string OOSProcessItem(OOSProcessItem obj, TransResults tr);

        GetOOSHypoTestingResult GetOOSHypoTestingResult(OOSGetHypoTestingInfo obj);

        OOSManageHypoResults OOSManageHypoTestingPhases(OOSManageHypoTestingPhases obj, TransResults tr);

        TestingSameSample OOSTestingOfNewPortionOfSameSampleResult(OOSGetHypoTestingInfo obj);

        GetOOSSingleAndCatBDetails GetOOSSingleAndCatBDetails(int OOSTestDetailID);

        GetDeptReviewData GetDeptReviewDetails(int oosTestDetID);

        RecordActionDetails ManageOOSDeptReview(OOSDeptReview obj, TransResults tr);

        List<GetDepartmentWiseReviews> GetDepartmentWiseReview(int oosTestDetailID, int deptID);

        RecordActionDetails ManageDepartmentComments(ManageDeptCommets obj, TransResults tr);

        List<ManufactureChecklist> GetManufactureChecklist(string category);

        ManufactureCheckPoints GetManufactureCheckPoints(int oosTestDetID, int phaseID);

        MfgInvestigationDetails ManufactureInvestigationDetails(int oosTestDetID);

        GetActionAndRptData ManageQASummaryInfo(ManageQASummaryInfo obj, TransResults tr);

        GetQASummaryInfo GetQASummaryInfo(int oosTestID, int userRoleID);
        
        RecordActionDetails ManageOOSSummaryInfo(ManageOOSSummaryInfo obj, TransResults tr);

        ManageOOSSummaryInfo GetOOSSummaryInfo(int oosTestID);

        NewDeviationReg GenerateDeviationReq(int oosTestID, TransResults tr);

        List<NewSampleRequest> GetNewSampleRequests(int oosTestID);
    }
}
