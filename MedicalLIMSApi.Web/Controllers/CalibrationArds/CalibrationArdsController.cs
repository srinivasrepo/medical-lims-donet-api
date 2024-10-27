using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Interface.CalibrationArds;
using MedicalLIMSApi.Web.App_Start;
using System.Collections.Generic;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.CalibrationArds
{
    [LIMSAuthorization]
    public class CalibrationArdsController : ApiController
    {
        ICalibrationArds db;
        public CalibrationArdsController(ICalibrationArds db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("GetCalibrationArdsHeaderInfo")]
        public CalibrationArdsHeader GetCalibrationArdsHeaderInfo(string encID)
        {
            return db.GetCalibrationArdsHeaderInfo(CommonStaticMethods.Decrypt<int>(encID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID);
        }

        [HttpPost]
        [Route("SearchEquipmentMaintenance")]
        public SearchResults<GetEquipmentMaintenance> SearchEquipmentMaintenance(SearchEquipmentMaintenance obj)
        {
            return db.SearchEquipmentMaintenance(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID);
        }

        [HttpGet]
        [Route("GetEquipmentCategories")]
        public List<GetEquipmentCategories> GetEquipmentCategories()
        {
            return db.GetEquipmentCategories(MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().DeptCode);
        }

        [HttpPost]
        [Route("GetEquipmentTypesByCategory")]
        public List<GetEquipmentTypesByCategory> GetEquipmentTypesByCategory (EquipmentTypesByCategory obj)
        {
            return db.GetEquipmentTypesByCategory(obj);
        }

        [HttpGet]
        [Route("GetScheduleTypesByDeptCode")]
        public List<GetScheduleTypesByDeptCode> GetScheduleTypesByDeptCode()
        {
            return db.GetScheduleTypesByDeptCode(MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().DeptCode);
        }

        [HttpPost]
        [Route("RunCalibration")]
        public RecordActionDetails RunCalibration(RunCalibration obj)
        {
            return db.RunCalibration(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GenerateNewRequest")]
        public RecordActionDetails GenerateNewRequest(EQUPMAINInsertNewRequest obj)
        {
            return db.GenerateNewRequest(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }
    }
}
