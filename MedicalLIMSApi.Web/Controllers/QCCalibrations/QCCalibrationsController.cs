using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.QCCalibrations;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using MedicalLIMSApi.Core.Interface.QCCalibrations;
using MedicalLIMSApi.Web.App_Start;
using System;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.QCCalibrations
{
    [LIMSAuthorization]

    public class QCCalibrationsController : ApiController
    {
        IQCCalibrations db;
        public QCCalibrationsController(IQCCalibrations _db)
        {
            this.db = _db;
        }


        [HttpPost]
        [Route("ManageCalibrationHeadersInfo")]
        public TransResultApprovals ManageCalibrationHeadersInfo(QCCalibrationsHeadersInfoBO obj)
        {
            TransResultApprovals act = new TransResultApprovals();
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            act = db.ManageCalibrationHeadersInfo(obj, trn);
            if (act.ReturnFlag == "SUCCESS" && obj.UploadFiles != null && obj.UploadFiles.Count > 0)
            {
                getUplodedFiles uploadBo = new getUplodedFiles();
                uploadBo.AppCode = trn.ApplicationType;
                uploadBo.PlantCode = trn.PlantCode;
                uploadBo.EntityCode = obj.EntityCode;
                uploadBo.DeptCode = trn.DeptCode;
                uploadBo.LoginID = trn.LoginID;
                uploadBo.EntActID = Convert.ToInt32(act.TransKey);
                uploadBo.RefNumber = act.ReferenceNumber;
                uploadBo.Role = obj.Role;
                uploadBo.DMSTempIDLst = obj.UploadFiles;

                string retCode = CommonStaticMethods.PostApiConnectionData<string>("UploadTempFiles", uploadBo, "DMS_URL");

                if (retCode != "Success")
                    act.ReturnFlag = "DOC_UPLOAD_FAILED";
            }

            return act; 
        }

        [HttpGet]
        [Route("GetCalibrationHeaderDetails")]
        public GetQCCalibrationsHeadersInfoBO GetCalibrationHeaderDetails(string encCalibParamID)
        {
            return db.GetCalibrationHeaderDetails(CommonStaticMethods.Decrypt<int>(encCalibParamID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("AddNewSpecCategory")]
        public string AddNewSpecCategory(AddNewSpecCategoryBO obj)
        {
            return db.AddNewSpecCategory(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("AddNewSpecSubCategory")]
        public string AddNewSpecSubCategory(AddNewSpecCategoryBO obj)
        {
            return db.AddNewSpecSubCategory(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("QCUpdateSingleTestMethodInstrumentData")]
        public QCGetSingleTestMethod QCUpdateSingleTestMethodInstrumentData(ManageQCSingleTestMethod obj)
        {
            return db.QCUpdateSingleTestMethodInstrumentData(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetCalibrationTests")]
        public GetCalibrationTestsBOList GetCalibrationTests(string encCalibParamID, int specID)
        {
            return db.GetCalibrationTests(CommonStaticMethods.Decrypt<int>(encCalibParamID), specID);
        }

        [HttpGet]
        [Route("GetCalibrationTestDetailsByID")]
        public GetCalibrationsTestDetailsBO GetCalibrationTestDetailsByID(string encSpecCatID)
        {
            return db.GetCalibrationTestDetailsByID(CommonStaticMethods.Decrypt<int>(encSpecCatID));
        }

        [HttpGet]
        [Route("GetTestResultByID")]
        public CalibrationResultBOList GetTestResultByID(string encSpecCatID)
        {
            return db.GetTestResultByID(CommonStaticMethods.Decrypt<int>(encSpecCatID));
        }

        [HttpPost]
        [Route("QCSPECDeleteTestMethods")]
        public TransResults QCSPECDeleteTestMethods(QCSPECDeleteTestMethodsBO obj)
        {
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();

            db.QCSPECDeleteTestMethods(obj, trn);

            return trn;
        }

        [HttpGet]
        [Route("ViewCalibrationDetailsByID")]
        public ViewQCCalibrationDetailsBO ViewCalibrationDetailsByID(string encCalibParamID)
        {
            return db.ViewCalibrationDetailsByID(CommonStaticMethods.Decrypt<int>(encCalibParamID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SearchQCCalibrations")]
        public SearchResults<SearchQCCalibrationResultBO> SearchQCCalibrations(SearchQCCalibrationsBO obj)
        {
            return db.SearchQCCalibrations(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageAssignSTPGroupTest")]
        public TransResultApprovals ManageAssignSTPGroupTest(ManageAssignSTPGroupTestDetails obj)
        {
            return db.ManageAssignSTPGroupTest(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetInstrumentsByType")]
        public GetIntrumentsBOList GetInstrumentsByType(string equipCode)
        {
            return db.GetInstrumentsByType(equipCode, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID);
        }

        [HttpGet]
        [Route("NewVersionCalibParamSet")]
        public TransResultApprovals NewVersionCalibParamSet(string encCalibParamID)
        {
            return db.NewVersionCalibParamSet(CommonStaticMethods.Decrypt<int>(encCalibParamID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("CalibrationChangeStatusComments")]
        public string CalibrationChangeStatus(CommentsBO obj)
        {
            return db.CalibrationChangeStatus(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetTestByID")]
        public SpecTestList GetTestByID(TestBo obj)
        {
            return db.GetTestByID(obj);
        }

        [HttpPost]
        [Route("AssignInstruments")]
        public AssignInstrumentList AssignInstruments(AssignInstrumentDetailsBO obj)
        {
            return db.AssignInstruments(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("AssignPlant")]
        public ManagePlantForCalibrationParametersResult GetPlantForCalibrationParameters(ManagePlantForCalibrationParameters obj)
        {
            return db.GetPlantForCalibrationParameters(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageArdsDocuments")]
        public CALIBManageARDS CALIBManageARDS(ManageArdsDocuments obj)
        {
            return db.CALIBManageARDS(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("CloneCalibrationParamSet")]
        public RecordActionDetails CloneCalibrationParamSet(string encCalibParamID)
        {
            return db.CloneCalibrationParamSet(CommonStaticMethods.Decrypt<int>(encCalibParamID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetManualReferenceNumber")]
        public string GetManualReferenceNumber(GetReferenceNumber manulRefObj)
        {
            string result = string.Empty;
            return result = CommonStaticMethods.PostApiConnectionData<string>("GetMasterDocumentNumbers", manulRefObj, "DMS_URL");
        }
    }
}
