using MedicalLIMSApi.Core.Entities.AnalystQualification;
using MedicalLIMSApi.Core.Entities.Common;

namespace MedicalLIMSApi.Core.Interface.AnalystQualification
{
    public  interface IAnalystQualification
    {
        RecordActionDetails ManageAnalystQualification(AnalystQualificationBo obj, TransResults tr);

        SearchResults<SearchAnalyst> SearchAnalystDetails(SearchAnalystBo obj);

        SearchAnalystDet GetAnalystDetailsByID(int AnalystID, TransResults tr);

        CatItemsMasterList GetAnalystQualifications();

        RecordActionDetails ManageQualificationRequest(ManageQualificationRequest obj , TransResults tr);

        GetQualificationTypeList GetQualificationType(int techniqueID,int analystID, int requestTypeID);

        GetAnalysisTypeList GetAnalysisTypeByCategoryID(int CategoryID);

        GetTestsByTechniqueAndArIDList GetTestsByTechniqueAndArID(SearchTestsByTechniqueAndArID obj);

        GetQualificationDetails GetQualificationDetails(EditQualificationDetails obj);

        GetQualificationDetailsForView GetQualificationDetailsForView(int qualificationID, TransResults tr);

        SearchResults<SearchResultsQualificationDetails> SearchResultsQualificationDetails(SearchQualificationDetails obj ,short plantID);

        RecordActionDetails ManageQualificationEvaluation(ManageQualificationEvaluation obj, TransResults tr);

        string ManageAnalystDisqualify(DisqualifyBO obj, TransResults tr);

    }
}
