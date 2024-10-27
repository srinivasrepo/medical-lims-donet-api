using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Invalidations;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Interface.Invalidations;
using MedicalLIMSApi.Web.App_Start;
using MedicalLIMSApi.Web.Utilities;
using System.Collections.Generic;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.Invalidations
{
    [LIMSAuthorization]
    public class InvalidationsController : ApiController
    {
        IInvalidations db;
        public InvalidationsController(IInvalidations db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("SearchInvalidations")]
        public SearchResults<SearchInvalidations> SearchInvalidations(InvSearchBO obj)
        {
            obj.PlantID = MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID;
            return db.SearchInvalidations(obj);
        }

        [HttpGet]
        [Route("GetInvalidationData")]
        public InvalidationInfo GetInvalidationData(string encInvalidationID)
        {
            int invalidationID = default(int);
            invalidationID = CommonStaticMethods.Decrypt<int>(encInvalidationID);

            return db.GetInvalidationData(invalidationID, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageInvalidationInfo")]
        public RecordActionDetails ManageInvalidationInfo(ManageInvalidationBO obj)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();
            retObj = db.ManageInvalidationInfo(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
            if (retObj.Act.ReturnFlag == "SUCCESS" && retObj.RptList != null && retObj.RptList.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = retObj.Act.TransKey;
                dmsObj.EntityCode = "INVALIDATIONS";
                dmsObj.ReferenceNumber = retObj.Act.ReferenceNumber;
                dmsObj.List = retObj.RptList;
                string retCode = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
                if (retCode != "OK" && retCode != "SUCCESS")
                    retObj.Act.ReturnFlag = retCode;
            }
            return retObj.Act;
        }


        [HttpGet]
        [Route("GetInvalidationTypesInstrumentTypes")]
        public SearchInvalidationList GetInvalidationTypesInstrumentTypes()
        {
            return db.GetInvalidationTypesInstrumentTypes();
        }

        [HttpGet]
        [Route("GetPreviousInvalidations")]
        public List<GetPreviousInvalidations> GetPreviousInvalidations(string encInvalidationID)
        {
            return db.GetPreviousInvalidations(CommonStaticMethods.Decrypt<int>(encInvalidationID));
        }

        [HttpPost]
        [Route("ManageInvalidationManualInfo")]
        public RecordActionDetails ManageInvalidationManualInfo(ManageInvalidationManualInfo obj)
        {
            return db.ManageInvalidationManualInfo(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

    }
}
