using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.RolePermissions;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Interface.RolePermissions
{
    public interface IRolePermissions
    {
        SearchResults<RolePermissionDetails> GetRolePermissionDetails(RolePermissionSearch search);

        ModulesAndStatus GetAllEntityModulesAndStatus(string entityType, string applicationType);

        string ManageTemplate(Conditions con, TransResults trn);

        AppLevelDetails ManageAppLevelDetails(GetApprovalTemplateDetails obj, TransResults trn);

        string InsertApprovalTemplates(ApprovalTemplateDetails app, string XMLstring, TransResults tr);

        GetApprovalRolesDetails GetManageRolesDetails(GetApprovalUsersFilters obj);

        string DeleteApprovalTemplateDetailsByDetailID(int detailsID, TransResults tr);

        TemplateNewVersion InsertNewVersion(RolePermissionDetails det, TransResults tr);

        string InsertApprovalPermission(ManageCapability cap, TransResults tr);

        RolesDetails GetManageCapabilityRolesDetails(short permissionID, byte deptID);

        string DeleteCapability(short permissionID, TransResults tr);

        string ManageCapabilityRoles(ManageApprovalTemplateRole role, TransResults tr, string XMLstring);

        List<Conditions> GetConditionSByEntityModulesByID(byte entityID);

        SingleUsrBasicList GetUsersByID(UsersByID obj);

        SingleUsrBasicList GetRolesByID(UsersByID obj);

        string ManageApprovalUsers(ManageApprovalUsers obj, TransResults trn);

        string ManageApprovalRoles(ManageApprovalUsers obj, TransResults trn);

        ApprovalPermissions GetSelectedDeptCapabilities(byte entityID);

        string DiscardTemplate(int templateID, TransResults trn);

        string ManageStatus(ManageStatus obj, TransResults tr);

        string ManageActionProvision(ManageActionProvision obj);

        ActionsList GetAllActiveActions();

    }
}
