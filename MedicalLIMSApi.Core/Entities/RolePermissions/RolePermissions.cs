using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MedicalLIMSApi.Core.Entities.RolePermissions
{
    public class RolePermissionDetails
    {
        public string EntityName { get; set; }

        public string Condition { get; set; }

        public string Version { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Status { get; set; }

        public int TemplateID { get; set; }

        public string EncTemplateID        // Convert Dec To Enc From Database To UI
        {
            get { return CommonStaticMethods.Encrypt(TemplateID.ToString()); }

        }

        public string EncTemplateIDU { get; set; } // from UI to Database

        public int DecTemplateID { get { return CommonStaticMethods.Decrypt<int>(EncTemplateIDU); } }

        public string StatusCode { get; set; }

    }

    public class RolePermissionSearch : PagerBO
    {
        public int ConditionID { get; set; }

        public byte StatusID { get; set; }

        public byte EntityID { get; set; }

        public string EntityType { get; set; }

        public string ApplicationType { get; set; }
    }

    public class ModulesAndStatus
    {
        public List<Entity> EntityList { get; set; }


        public List<ActiveStatus> statusList { get; set; }
    }

    public class Conditions
    {
        public short ConditionID { get; set; }

        public string Condition { get; set; }

        public int TemplateID { get { return CommonStaticMethods.Decrypt<int>(EncTemplateIDU); } }

        public string EncTemplateIDU { get; set; }


    }


    public class AppLevelDetails
    {
        public List<ActiveStatuses> statusList { get; set; }

        public List<ActiveActions> actionList { get; set; }

        public List<ManageAppLevelDetails> manageAppLevel { get; set; }

        public string TemplateStatus { get; set; }

        public string Version { get; set; }

        public string Condition { get; set; }

        public string Status { get; set; }

        public string ConditionCode { get; set; }

        public GetApprovalConditionNotesList Notes { get; set; }
    }

    public class GetApprovalConditionNotes
    {
        public string Note { get; set; }
    }

    public class GetApprovalConditionNotesList : List<GetApprovalConditionNotes> { }

    public class ManageAppLevelDetails
    {
        public byte Applevel { get; set; }

        public string Status { get; set; }

        public string AppType { get; set; }

        public int DetailID { get; set; }

        public string ACTION { get; set; }

        public string EncDetailID        // Convert Dec To Enc From Database To UI
        {
            get { return CommonStaticMethods.Encrypt(DetailID.ToString()); }

        }

        public bool RespectiveDept { get; set; }

        public bool? RespectivePlant { get; set; }

        public string Component { get; set; }

        public string OperationType { get; set; }
    }


    [XmlType("ITEM")]
    public class ActiveActions
    {
        public int ActionID { get; set; }

        public string ActionCode { get; set; }

        public bool isSelect { get; set; }
    }

    [XmlType("RT")]
    public class ActiveActionsList : List<ActiveActions> { }


    public class ActiveStatuses
    {
        public byte StatusID { get; set; }

        public string Status { get; set; }
    }

    public class ApprovalTemplateDetails
    {
        public byte AppLevel { get; set; }

        public int StatusID { get; set; }

        public char AppType { get; set; }

        public ActiveActionsList actions { get; set; }

        public int TemplateID { get; set; }

        public string EncTemplateIDU { get; set; }

        public int DecTemplateID { get { return CommonStaticMethods.Decrypt<int>(EncTemplateIDU); } }

        public bool RespectiveDept { get; set; }

        public bool RespectivePlant { get; set; }

        public string OperationType { get; set; }

        public int ComponentID { get; set; }

    }

    public class ManageApprovalTemplateRole
    {
        public SingleRoleList roleList { get; set; }

        public string encDetailID { get; set; }

        public int DetailID { get { return CommonStaticMethods.Decrypt<int>(encDetailID); } }

        public short PermissionID { get { return CommonStaticMethods.Decrypt<short>(encDetailID); } }
        public string Type { get; set; }
    }


    public class ManageCapability
    {
        public int EntityID { get; set; }

        public SelectedCapabilitiesList SelectedCapaList { get; set; }

        public string XMLString { get { if (SelectedCapaList.Count > 0) return CommonStaticMethods.Serialize<SelectedCapabilitiesList>(SelectedCapaList); else return string.Empty; } }
    }

    [XmlType("ITEM")]
    public class SelectedCapabilities
    {
        public int DeptID { get; set; }

        public int CapID { get; set; }
    }

    [XmlType("RT")]
    public class SelectedCapabilitiesList : List<SelectedCapabilities> { }

    public class RolesDetails
    {
        public RoleList assinedRoles { get; set; }

        public RoleList masterRoles { get; set; }

    }

    public class UserRoleDetails
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public bool IsSelect { get; set; }
    }

    public class UserRoleDetailsList : List<UserRoleDetails> { }

    public class UsersByID
    {
        public short PlantID { get; set; }

        public int DeptID { get; set; }

        public int RoleID { get; set; }

        public string EncDetailID { get; set; }

        public int DetailID { get { return CommonStaticMethods.Decrypt<int>(EncDetailID); } }

        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

    }

    public class GetApprovalUsers : UsersByID
    {
        public string UserName { get; set; }

        public int UserID { get; set; }

        public bool IsSelect { get; set; }
    }

    public class GetApprovalUsersList : List<GetApprovalUsers> { }

    public class GetApprovalRolesDetails
    {
        public RoleMasterSmallList AssinedRoles { get; set; }

        public DeptModuleList SelectedDeptList { get; set; }

        public GetApprovalUsersList AssignedUserList { get; set; }

        public GetPlantDeptList PlantDeptList { get; set; }
    }

    public class ManageApprovalUsers : UsersByID
    {
        public SIngleBOList UsrList { get; set; }

        public string XMLString { get { return CommonStaticMethods.Serialize<SIngleBOList>(UsrList); } }

        public SingleRoleList RoleList { get; set; }

        public string RoleXMLString { get { return CommonStaticMethods.Serialize<SingleRoleList>(RoleList); } }

        public string Type { get; set; }

    }

    public class GetApprovalUsersFilters
    {
        public string EncDeptID { get; set; }

        public string EncDetailID { get; set; }

        public int DetailID { get { return CommonStaticMethods.Decrypt<int>(EncDetailID); } }

        public int DeptID { get { return CommonStaticMethods.Decrypt<int>(EncDeptID); } }

    }

    public class GetApprovalTemplateDetails
    {
        public string EncEntityActID { get; set; }

        public int EntityActID { get { return CommonStaticMethods.Decrypt<int>(EncEntityActID); } }

        public string EncTemplateID { get; set; }

        public int TemplateID { get { return CommonStaticMethods.Decrypt<int>(EncTemplateID); } }
    }

    public class TemplateNewVersion
    {
        public string ResultFlag { get; set; }

        public int NewTemplateID { get; set; }

        public string EncNewTemplateID { get { return CommonStaticMethods.Encrypt(NewTemplateID.ToString()); } }
    }

    public class ApprovalPermissions
    {
        public string EntityName { get; set; }

        public DeptMasterLists DeptList { get; set; }

        public ApprovalCapabilitiesList AppCapaList { get; set; }

        public ApprovalSelectedCapabilityList SelectedAppCapaList { get; set; }

    }

    public class DeptMasters
    {
        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }
    }

    public class DeptMasterLists : List<DeptMasters> { }

    public class ApprovalCapabilities
    {
        public int CapabilityID { get; set; }

        public string Capability { get; set; }
    }

    public class ApprovalCapabilitiesList : List<ApprovalCapabilities> { }

    public class ApprovalSelectedCapability
    {
        public int DepartmentID { get; set; }

        public int CapabilityID { get; set; }

        public short PermissionID { get; set; }

        public string EncPermissionID { get { return CommonStaticMethods.Encrypt(PermissionID.ToString()); } }
    }

    public class ApprovalSelectedCapabilityList : List<ApprovalSelectedCapability> { }

    [XmlType("ITEM")]
    public class SingleRole
    {
        public int RoleID { get; set; }
    }

    [XmlType("RT")]
    public class SingleRoleList : List<SingleRole> { }

    public class GetPlantDept
    {
        public short PlantID { get; set; }

        public int DeptID { get; set; }

        public string DeptName { get; set; }

        public string PlantName { get; set; }
    }

    public class GetPlantDeptList : List<GetPlantDept> { }

    public class ManageStatus
    {
        public string StatusCode { get; set; }

        public string Status { get; set; }
    }

    public class ManageActionProvision
    {
        public string ActionCode { get; set; }

        public string Action { get; set; }

    }

    public class ActionsList : List<ActionsDetails> { }

    public class ActionsDetails
    {
        public int ActionID { get; set; }

        public string ActionCode { get; set; }

        public bool isSelect { get; set; }
    }

}

