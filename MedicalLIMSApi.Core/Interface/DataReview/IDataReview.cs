using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.DataReview;
using MedicalLIMSApi.Core.Entities.MobilePhase;

namespace MedicalLIMSApi.Core.Interface.DataReview
{
    public interface IDataReview
    {
        DataReviewTestList GetTestForReview(GetReviewTests obj);

        GetActionAndRptData ManageDataReviewData(DataReviewData obj, TransResults tr);

        GetDataReviewDetails GetDataReviewData(int reviewID, TransResults tr);

        SearchResults<GetDataReviewDetailsBySearch> GetDataReviewDetailsBySearchId(SearchDataReview obj, short plantID);

    }
}
