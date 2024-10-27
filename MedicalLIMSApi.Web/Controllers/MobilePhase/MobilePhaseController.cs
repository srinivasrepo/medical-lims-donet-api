using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using MedicalLIMSApi.Core.Interface.MobilePhase;
using MedicalLIMSApi.Web.App_Start;
using MedicalLIMSApi.Web.Utilities;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.MobilePhase
{
    [LIMSAuthorization]
    public class MobilePhaseController : ApiController
    {
        IMobilePhase db;

        public MobilePhaseController(IMobilePhase db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("ManageMobilePhase")]
        public RecordActionDetails ManageMobilePhase(MobilePhaseBO obj)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            retObj = db.ManageMobilePhase(obj, trn);
            if (retObj.Act.ResultFlag == "SUCCESS" && obj.FileUploadedIDs != null && obj.FileUploadedIDs.Count > 0)
            {
                getUplodedFiles uploadBo = new getUplodedFiles();
                uploadBo.AppCode = trn.ApplicationType;
                uploadBo.PlantCode = trn.PlantCode;
                uploadBo.EntityCode = obj.EntityCode;
                uploadBo.DeptCode = trn.DeptCode;
                uploadBo.LoginID = trn.LoginID;
                uploadBo.EntActID = retObj.Act.TransKey;
                uploadBo.RefNumber = retObj.Act.ReferenceNumber;
                uploadBo.Role = obj.Role;
                uploadBo.DMSTempIDLst = obj.FileUploadedIDs;

                string retCode = CommonStaticMethods.PostApiConnectionData<string>("UploadTempFiles", uploadBo, "DMS_URL");

                if (retCode != "Success")
                    retObj.Act.ReturnFlag = "DOC_UPLOAD_FAILED";
            }
            if (retObj.Act.ResultFlag == "SUCCESS" && retObj.RptList != null && retObj.RptList.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = retObj.Act.TransKey;
                dmsObj.EntityCode = "MOBILE_PHASE";
                dmsObj.ReferenceNumber = retObj.Act.ReferenceNumber;
                dmsObj.List = retObj.RptList;
                retObj.Act.ReturnFlag = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
            }
            return retObj.Act;
        }

        [HttpGet]
        [Route("GetMobilePhaseData")]
        public MobilePhaseData GetMobilePhaseData(string encMobilePhaseID)
        {
            int mobilePhaseID = default(int);
            mobilePhaseID = CommonStaticMethods.Decrypt<int>(encMobilePhaseID);
            return db.GetMobilePhaseData(mobilePhaseID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetConvertableUOMByMatID")]
        public UOMList GetConvertableUOMByMatID(int? materialID)
        {
            return db.GetConvertableUOMByMatID(materialID, 0);
        }

        [HttpGet]
        [Route("GetConvertableUOMBySioMatID")]
        public UOMList GetConvertableUOMByMatID(int? materialID, string encSioID)
        {
            int sioID = CommonStaticMethods.Decrypt<int>(encSioID);
            return db.GetConvertableUOMByMatID(materialID, sioID);
        }

        [HttpPost]
        [Route("ManageMobilePhaseSolventsPreparation")]
        public Preparations ManageMobilePhaseSolventsPreparation(SolventPreparation obj)
        {
            return db.ManageMobilePhaseSolventsPreparation(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageMobilePhasePrepComments")]
        public TransResultApprovals ManageMobilePhasePrepComments(MobilePhasePrepComments obj)
        {
            return db.ManageMobilePhasePrepComments(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetValidityPeriods")]
        public SingleUsrBasicList GetValidityPeriods(string entityCode)
        {
            return db.GetValidityPeriods(entityCode);
        }

        [HttpPost]
        [Route("ManagePhaseOutput")]
        public ManageMobilePreparationOutput ManagePhaseOutput(MobilePhaseOutput obj)
        {
            return db.ManagePhaseOutput(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetSearchMobilePhaseData")]
        public SearchMobilePhaseData GetSearchMobilePhaseData(SearchMPBO obj)
        {
            obj.PlantID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID;
            return db.GetSearchMobilePhaseData(obj);
        }

        [HttpGet]
        [Route("ViewMobilePhaseData")]
        public viewMobilePhase ViewMobilePhaseData(string encPhaseID)
        {
            int phaseID = default(int);
            phaseID = CommonStaticMethods.Decrypt<int>(encPhaseID);
            return db.ViewMobilePhaseData(phaseID);
        }

        [HttpGet]
        [Route("GetPreparationDetails")]
        public GetPreparationDetailsList GetPreparationDetails(string encMobilePhaseID)
        {
            return db.GetPreparationDetails(CommonStaticMethods.Decrypt<int>(encMobilePhaseID));
        }

        [HttpGet]
        [Route("GetCalibrationParameters")]
        public ParameterTypeList GetCalibrationParameters()
        {
            return db.GetCalibrationParameters();
        }

        [HttpPost]
        [Route("ManagePreparationMasterData")]
        public GetMasterData ManagePreparationMasterData(MasterData obj)
        {
            return db.ManagePreparationMasterData(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("DiscardPreparationBatch")]
        public string DiscardPreparationBatch(DiscardPreparationBatch obj)
        {
            return db.DiscardPreparationBatch(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetMaterialDetailsBySpecID")]
        public GetProductStageDetails GetMaterialDetailsBySpecID(int specID)
        {
            return db.GetMaterialDetailsBySpecID(specID);
        }

    }
}
