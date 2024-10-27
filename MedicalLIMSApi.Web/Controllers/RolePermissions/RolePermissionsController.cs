using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.RolePermissions;
using MedicalLIMSApi.Core.Interface.RolePermissions;
using MedicalLIMSApi.Web.App_Start;
using System.Collections.Generic;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.RolePermissions
{
    [LIMSAuthorization]
    public class RolePermissionsController : ApiController
    {
        IRolePermissions db;

        public RolePermissionsController(IRolePermissions db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("GetRolePermissionDetails")]
        public SearchResults<RolePermissionDetails> GetRolePermissionDetails(RolePermissionSearch obj)
        {
            TransResults trn = new TransResults();
            obj.ApplicationType = trn.ApplicationType;

            return db.GetRolePermissionDetails(obj);
        }

        [HttpGet]
        [Route("GetAllEntityModules")]
        public ModulesAndStatus GetAllEntityModulesAndStatus(string entityType)
        {
            return db.GetAllEntityModulesAndStatus(entityType, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().ApplicationType);
        }


        [HttpPost]
        [Route("ManageTemplate")]
        public string ManageTemplate(Conditions con)
        {
            return db.ManageTemplate(con, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }


        [HttpPost]
        [Route("GetManageAppLevel")]
        public AppLevelDetails GetManageAppLevelDetails(GetApprovalTemplateDetails obj)
        {
            return db.ManageAppLevelDetails(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }


        [HttpPost]
        [Route("InsertApprovalTemplates")]

        public string InsertApprovalTemplates(ApprovalTemplateDetails app)
        {

            string XMLstring = null;
            XMLstring = Utilities.Common.Serialize<ActiveActionsList>(app.actions);
            TransResults tr = Utilities.Common.GetUserDetails();
            return db.InsertApprovalTemplates(app, XMLstring, tr);
        }


        [HttpPost]
        [Route("GetManageRolesDetails")]
        public GetApprovalRolesDetails GetManageRolesDetails(GetApprovalUsersFilters obj)
        {
            return db.GetManageRolesDetails(obj);
        }

        [HttpPost]
        [Route("DeleteApprovalTemplateDetails")]

        public string DeleteApprovalTemplateDetailsByDetailID(ManageApprovalTemplateRole detailID)
        {
            TransResults tr = Utilities.Common.GetUserDetails();
            return db.DeleteApprovalTemplateDetailsByDetailID(detailID.DetailID, tr);
        }


        [HttpPost]
        [Route("CreateNewVersion")]
        public TemplateNewVersion InsertNewVersion(RolePermissionDetails det)
        {
            TransResults tr = Utilities.Common.GetUserDetails();
            return db.InsertNewVersion(det, tr);
        }

        [HttpPost]
        [Route("InsertApprovalPermission")]

        public string InsertApprovalPermission(ManageCapability cap)
        {
            TransResults tr = Utilities.Common.GetUserDetails();
            return db.InsertApprovalPermission(cap, tr);
        }


        [HttpGet]
        [Route("GetManageCapabilityRolesDetails")]
        public RolesDetails GetManageCapabilityRolesDetails(string encPermissionID, byte deptID)
        {
            short permissionID = default(int);
            permissionID = CommonStaticMethods.Decrypt<short>(encPermissionID);
            return db.GetManageCapabilityRolesDetails(permissionID, deptID);
        }


        [HttpDelete]
        [Route("DeleteCapability")]
        public string DeleteCapability(string encPermissionID)
        {
            short permissionID = default(short);
            permissionID = CommonStaticMethods.Decrypt<short>(encPermissionID);
            TransResults tr = Utilities.Common.GetUserDetails();

            return db.DeleteCapability(permissionID, tr);
        }

        [HttpPost]
        [Route("ManageCapabilityRoles")]

        public string ManageCapabilityRoles(ManageApprovalTemplateRole role)
        {
            string XMLstring = null;
            XMLstring = Utilities.Common.Serialize<SingleRoleList>(role.roleList);
            TransResults tr = Utilities.Common.GetUserDetails();

            return db.ManageCapabilityRoles(role, tr, XMLstring);
        }


        [HttpGet]
        [Route("GetConditionSByEntityModulesByID")]
        public List<Conditions> GetConditionSByEntityModulesByID(byte entityID)
        {
            return db.GetConditionSByEntityModulesByID(entityID);
        }

        [HttpPost]
        [Route("GetUsersByID")]
        public SingleUsrBasicList GetUsersByID(UsersByID obj)
        {
            return db.GetUsersByID(obj);
        }

        [HttpPost]
        [Route("GetRolesByID")]
        public SingleUsrBasicList GetRolesByID(UsersByID obj)
        {
            return db.GetRolesByID(obj);
        }


        [HttpPost]
        [Route("ManageApprovalUsers")]
        public string ManageApprovalUsers(ManageApprovalUsers obj)
        {
            return db.ManageApprovalUsers(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }


        [HttpPost]
        [Route("ManageApprovalRoles")]
        public string ManageApprovalRoles(ManageApprovalUsers obj)
        {
            return db.ManageApprovalRoles(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }


        [HttpGet]
        [Route("GetSelectedDeptCapabilities")]
        public ApprovalPermissions GetSelectedDeptCapabilities(byte entityID)
        {
            return db.GetSelectedDeptCapabilities(entityID);
        }

        [HttpGet]
        [Route("DiscardTemplate")]
        public string DiscardTemplate(string encTemplateID)
        {
            return db.DiscardTemplate(CommonStaticMethods.Decrypt<int>(encTemplateID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageStatus")]
        public string ManageStatus(ManageStatus obj)
        {
            return db.ManageStatus(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageActionProvision")]
        public string ManageActionProvision(ManageActionProvision obj)
        {
            return db.ManageActionProvision(obj);
        }

        [HttpGet]
        [Route("GetAllActiveActions")]
        public ActionsList GetAllActiveActions()
        {
            return db.GetAllActiveActions();
        }
    }
}
