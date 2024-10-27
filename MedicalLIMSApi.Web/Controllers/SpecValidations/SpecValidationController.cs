using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SpecValidation;
using MedicalLIMSApi.Core.Interface.SpecValidation;
using MedicalLIMSApi.Web.App_Start;
using System.Collections.Generic;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.Spec_files
{
    [LIMSAuthorization]
    public class SpecValidationController : ApiController
    {
        ISpecValidation db;

        public SpecValidationController(ISpecValidation db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("ManageSpecValidationsDetails")]
        public SpecValidationCycle ManageSpecValidationsDetails(ManageSpecValidation obj)
        {
            return db.ManageSpecValidationsDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageCycleDetails")]
        public SpecValidationCycle ManageCycleDetails(ManageCycleDetailsBO obj)
        {
            return db.ManageCycleDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetSpecValidationDetails")]
        public GetSpecValidationDetails GetSpecValidationDetails(string encSpecValidationID, string entityCode)
        {
            EditSpecValidation obj = new EditSpecValidation();
            obj.EncSpecValidationID = encSpecValidationID;
            obj.EntityCode = entityCode;
            obj.UserRoleID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID;
            return db.GetSpecValidationDetails(obj);
        }

        [HttpPost]
        [Route("SearchResultSpecValidations")]
        public SearchResults<SearchResultSpecValidations> SearchResultSpecValidations(SearchSpecValidations obj)
        {
            obj.PlantID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID;
            return db.SearchResultSpecValidations(obj);
        }

        [HttpPost]
        [Route("ValidateTest")]
        public string ValidateTest(Validate obj)
        {
            return db.ValidateTest(obj);
        }

        [HttpGet]
        [Route("GetSpecificationTestToAssignSTP")]
        public List<GetSpecificationTestToAssignSTP> GetSpecificationTestToAssignSTP(int specID, int calibID)
        {
            return db.GetSpecificationTestToAssignSTP(specID, calibID);
        }

        [HttpPost]
        [Route("AssignSTPToTest")]
        public string AssignSTPToTest(AssignSTPToTest obj)
        {
            return db.AssignSTPToTest(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("TestSTPHistory")]
        public List<TestSTPHistory> TestSTPHistory(int specCatID)
        {
            return db.TestSTPHistory(specCatID);
        }
    }
}
