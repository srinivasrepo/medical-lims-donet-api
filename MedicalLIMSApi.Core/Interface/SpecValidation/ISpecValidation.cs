using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SpecValidation;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Interface.SpecValidation
{
    public interface ISpecValidation
    {
        SpecValidationCycle ManageSpecValidationsDetails(ManageSpecValidation obj, TransResults tr);

        SpecValidationCycle ManageCycleDetails(ManageCycleDetailsBO obj, TransResults tr);

        GetSpecValidationDetails GetSpecValidationDetails(EditSpecValidation obj);

        SearchResults<SearchResultSpecValidations> SearchResultSpecValidations(SearchSpecValidations obj);

        string ValidateTest(Validate obj);

        List<GetSpecificationTestToAssignSTP> GetSpecificationTestToAssignSTP(int specID, int calibID);

        string AssignSTPToTest(AssignSTPToTest obj, TransResults trn);

        List<TestSTPHistory> TestSTPHistory(int specCatID);
    }
}
