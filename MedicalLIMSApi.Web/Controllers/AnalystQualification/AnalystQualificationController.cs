using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.AnalystQualification;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using MedicalLIMSApi.Core.Interface.AnalystQualification;
using MedicalLIMSApi.Web.App_Start;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.AnalystQualification
{
    [LIMSAuthorization]
    public class AnalystQualificationController : ApiController
    { 
        IAnalystQualification db;

        public AnalystQualificationController(IAnalystQualification db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("ManageAnalystQualification")]
        public RecordActionDetails ManageAnalystQualification(AnalystQualificationBo obj)
        {
            return db.ManageAnalystQualification(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SearchAnalystDetails")]
        public SearchResults<SearchAnalyst> SearchAnalystDetails(SearchAnalystBo obj)
        {
            return db.SearchAnalystDetails(obj);
        }

        [HttpGet]
        [Route("GetAnalystDetailsByID")]
        public SearchAnalystDet GetAnalystDetailsByID(string encAnalystID)
        {
            return db.GetAnalystDetailsByID(CommonStaticMethods.Decrypt<int>(encAnalystID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetAnalystQualifications")]
        public CatItemsMasterList GetAnalystQualifications()
        {
            return db.GetAnalystQualifications();
        }

        [HttpPost]
        [Route("ManageQualificationRequest")]
        public RecordActionDetails ManageQualificationRequest(ManageQualificationRequest obj)
        {
            RecordActionDetails act = new RecordActionDetails();
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            act = db.ManageQualificationRequest(obj, trn);

            if (act.ReturnFlag == "SUCCESS" && obj.FileUploadedIDs != null && obj.FileUploadedIDs.Count > 0)
            {
                getUplodedFiles uploadBo = new getUplodedFiles();
                uploadBo.AppCode = trn.ApplicationType;
                uploadBo.PlantCode = trn.PlantCode;
                uploadBo.EntityCode = obj.EntityCode;
                uploadBo.DeptCode = trn.DeptCode;
                uploadBo.LoginID = trn.LoginID;
                uploadBo.EntActID = act.TransKey;
                uploadBo.RefNumber = act.ReferenceNumber;
                uploadBo.Role = obj.Role;
                uploadBo.DMSTempIDLst = obj.FileUploadedIDs;

                string retCode = CommonStaticMethods.PostApiConnectionData<string>("UploadTempFiles", uploadBo, "DMS_URL");

                if (retCode != "Success")
                    act.ReturnFlag = "DOC_UPLOAD_FAILED";
            }

            return act;
        }

        [HttpGet]
        [Route("GetQualificationType")]
        public GetQualificationTypeList GetQualificationType(int techniqueID, int analystID, int requestTypeID)
        {
            if(MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID == analystID)
            {
                GetQualificationTypeList lst = new GetQualificationTypeList();
                GetQualificationType obj = new GetQualificationType();
                obj.QualificationTypeID = 1;
                obj.QualificationTypeCode = "SAME_USER";
                lst.Add(obj);
                return lst;      
                
                          
            }
            else
                return db.GetQualificationType(techniqueID, analystID, requestTypeID);
        }

        [HttpGet]
        [Route("GetAnalysisTypeByCategoryID")]
        public GetAnalysisTypeList GetAnalysisTypeByCategoryID(int CategoryID)
        {
            return db.GetAnalysisTypeByCategoryID(CategoryID);
        }

        [HttpPost]
        [Route("GetTestsByTechniqueAndArID")]
        public GetTestsByTechniqueAndArIDList GetTestsByTechniqueAndArID(SearchTestsByTechniqueAndArID obj)
        {
            return db.GetTestsByTechniqueAndArID(obj);
        }

        [HttpGet]
        [Route("GetQualificationDetails")]
        public GetQualificationDetails GetQualificationDetails(string encQualificationID)
        {
            EditQualificationDetails obj = new EditQualificationDetails();
            obj.EncQualificationID = encQualificationID;
            obj.UserRoleID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID;
            return db.GetQualificationDetails(obj);
        }

        [HttpGet]
        [Route("GetQualificationDetailsForView")]
        public GetQualificationDetailsForView GetQualificationDetailsForView(string encQualificationID)
        {
            return db.GetQualificationDetailsForView(CommonStaticMethods.Decrypt<int>(encQualificationID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

      

        [HttpPost]
        [Route("SearchQualificationDetails")]
        public SearchResults<SearchResultsQualificationDetails> SearchResultsQualificationDetails(SearchQualificationDetails obj)
        {
            return db.SearchResultsQualificationDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID);
        }

        [HttpPost]
        [Route("ManageQualificationEvaluation")]
        public RecordActionDetails ManageQualificationEvaluation(ManageQualificationEvaluation obj)
        {
            return db.ManageQualificationEvaluation(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageAnalystDisqualify")]
        public string ManageAnalystDisqualify(DisqualifyBO obj)
        {
            return db.ManageAnalystDisqualify(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }
    }
}
