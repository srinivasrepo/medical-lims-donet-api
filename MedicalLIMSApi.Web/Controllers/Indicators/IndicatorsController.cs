using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Indicators;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.UtilUploads;
using MedicalLIMSApi.Core.Entities.VolumetricSolution;
using MedicalLIMSApi.Core.Interface.Indicators;
using MedicalLIMSApi.Web.App_Start;
using MedicalLIMSApi.Web.Utilities;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.Indicators
{

    [LIMSAuthorization]

    public class IndicatorsController : ApiController
    {
        IIndicators db;

        public IndicatorsController(IIndicators db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("SearchIndicators")]
        public SearchResults<SearchIndicatorData> SearchIndicators(SearchIndicatorsBO obj)
        {
            obj.PlantID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID;
            return db.SearchIndicators(obj);
        }

        [HttpGet]
        [Route("GetIndicatorsInfo")]
        public GetIndicatorsInfo GetIndicatorsInfo(string encIndicatorID)
        {
            return db.GetIndicatorsInfo(CommonStaticMethods.Decrypt<int>(encIndicatorID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID);
        }

        [HttpPost]
        [Route("ManageIndicatorsInfo")]
        public RecordActionDetails ManageIndicatorsInfo(ManageIndicatorsInfo obj)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();
            TransResults trn = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails();
            retObj = db.ManageIndicatorsInfo(obj, trn);
            if (retObj.Act.ReturnFlag == "OK" && obj.UploadedID != null && obj.UploadedID.Count > 0)
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
                uploadBo.DMSTempIDLst = obj.UploadedID;

                string retCode = CommonStaticMethods.PostApiConnectionData<string>("UploadTempFiles", uploadBo, "DMS_URL");

                if (retCode != "Success")
                    retObj.Act.ReturnFlag = "DOC_UPLOAD_FAILED";
            }
            if (retObj.Act.ReturnFlag == "SUCCESS" && retObj.RptList != null && retObj.RptList.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = retObj.Act.TransKey;
                dmsObj.EntityCode = "INDICATOR";
                dmsObj.ReferenceNumber = retObj.Act.ReferenceNumber;
                dmsObj.List = retObj.RptList;
                string retCode = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
                if (retCode != "OK" && retCode != "SUCCESS")
                    retObj.Act.ReturnFlag = retCode;
            }
            return retObj.Act;
        }

        [HttpGet]
        [Route("ViewIndicatorInfo")]
        public ViewIndicatorInfo ViewIndicatorInfo(string encIndicatorID)
        {
            return db.ViewIndicatorInfo(CommonStaticMethods.Decrypt<int>(encIndicatorID));
        }

        [HttpPost]
        [Route("GetSolPreparationDetails")]
        public SolventsList GetPreparationDetails(SolventPreparation obj)
        {
            return db.GetPreparationDetails(obj);
        }
        
        [HttpPost]
        [Route("ManageIndicatorMasterData")]
        public GetINDMasterData ManageIndicatorMasterData(ManageMasterData obj)
        {
            return db.ManageIndicatorMasterData(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageTestSolutionIndex")]
        public VolumetricSolIndexData ManageTestSolutionIndex(VolumetricSolIndexFilter obj)
        {
            return db.ManageTestSolutionIndex(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetTestSolutionIndexByID")]
        public GetVolumetricSolIndex GetTestSolutionIndexByID(string encIndexID)
        {
            return db.GetTestSolutionIndexByID(CommonStaticMethods.Decrypt<short>(encIndexID));
        }


    }
}
