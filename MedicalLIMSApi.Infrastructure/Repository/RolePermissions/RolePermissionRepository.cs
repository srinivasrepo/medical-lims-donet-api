using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.RolePermissions;
using MedicalLIMSApi.Core.Entities.Utilities;
using MedicalLIMSApi.Core.Interface.RolePermissions;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.RolePermissions
{
    public class RolePermissionRepository : IRolePermissions
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public SearchResults<RolePermissionDetails> GetRolePermissionDetails(RolePermissionSearch search)
        {

            var lst = new SearchResults<RolePermissionDetails>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATGetApprovalTemplateDetails");
            ctx.AddInParameter<int>(cmd, "@PageIndex", search.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", search.PageSize);
            ctx.AddInParameter<string>(cmd, "@ApplicationType", search.ApplicationType);

            if (search.ConditionID > default(int))
                ctx.AddInParameter<int>(cmd, "@ConditionID", search.ConditionID);
            if (search.StatusID > default(int))
                ctx.AddInParameter<byte>(cmd, "@StatusID", search.StatusID);
            if (search.EntityID > default(int))
                ctx.AddInParameter<byte>(cmd, "@EntityID", search.EntityID);

            if (!string.IsNullOrEmpty(search.EntityType))
                ctx.AddInParameter<string>(cmd, "@EntityType", search.EntityType);

            ctx.AddOutParameter(cmd, "@TotalRecords", DbType.Int32, 4);

            using (var reader = cmd.ExecuteReader())
            {

                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<RolePermissionDetails>(reader);

                List<RolePermissionDetails> objList = new List<RolePermissionDetails>();

                foreach (var rr in rrResult)
                    objList.Add(rr);

                lst.SearchList = objList;
            }

            lst.TotalNumberOfRows = Convert.ToInt32(ctx.GetOutputParameterValue(cmd, "@TotalRecords"));
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return lst;
        }

        public ModulesAndStatus GetAllEntityModulesAndStatus(string entityType, string applicationType)
        {
            var lst = new ModulesAndStatus();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATGetModulesNStatus");
            ctx.AddInParameter<string>(cmd, "@EntityType", entityType);
            ctx.AddInParameter<string>(cmd, "@ApplicationType", applicationType);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<Entity>(reader);

                lst.EntityList = new List<Entity>();
                foreach (var rr in rrResult)
                    lst.EntityList.Add(rr);

                reader.NextResult();

                var rrSResult = ((IObjectContextAdapter)context).ObjectContext.Translate<ActiveStatus>(reader);
                lst.statusList = new List<ActiveStatus>();

                foreach (var rr in rrSResult)
                    lst.statusList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string ManageTemplate(Conditions con, TransResults trn)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATManageTemplate");

            ctx.AddInParameter<int>(cmd, "@UserID", trn.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", trn.DeptCode);

            if (con.ConditionID > default(int))
                ctx.AddInParameter<int>(cmd, "@ConditionID", con.ConditionID);

            ctx.AddInOutParameter<int>(cmd, "@TemplateID", con.TemplateID, 4, DbType.Int32);
            ctx.AddInOutParameter<string>(cmd, "@RetCode", retVal, 5, System.Data.DbType.String);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public AppLevelDetails ManageAppLevelDetails(GetApprovalTemplateDetails obj, TransResults trn)
        {
            var lst = new AppLevelDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspRPGetTemplateDetails");
            ctx.AddInParameter<int>(cmd, "@TemplateID", obj.TemplateID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrTemStatus = ((IObjectContextAdapter)context).ObjectContext.Translate<AppLevelDetails>(reader);

                foreach (var item in rrTemStatus)
                    lst = item;

                reader.NextResult();

                var rrActions = ((IObjectContextAdapter)context).ObjectContext.Translate<ActiveActions>(reader);
                lst.actionList = new List<ActiveActions>();

                foreach (var rr in rrActions)
                    lst.actionList.Add(rr);

                reader.NextResult();

                var rrAppLevel = ((IObjectContextAdapter)context).ObjectContext.Translate<ManageAppLevelDetails>(reader);
                lst.manageAppLevel = new List<ManageAppLevelDetails>();

                foreach (var rr in rrAppLevel)
                    lst.manageAppLevel.Add(rr);

                reader.NextResult();

                var rrAppNotes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetApprovalConditionNotes>(reader);

                lst.Notes = new GetApprovalConditionNotesList();
                foreach (var item in rrAppNotes)
                    lst.Notes.Add(item);

                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public string InsertApprovalTemplates(ApprovalTemplateDetails app, string XMLstring, TransResults tr)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspRPInsertApprovalTemplateDetails");
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", tr.DeptCode);
            ctx.AddInParameter<int>(cmd, "@TemplateID", app.DecTemplateID);
            ctx.AddInParameter<byte>(cmd, "@Applevel", app.AppLevel);
            ctx.AddInParameter<int>(cmd, "@StatusID", app.StatusID);
            ctx.AddInParameter<char>(cmd, "@AppType", app.AppType);
            ctx.AddInParameter<string>(cmd, "@Action", XMLstring);
            ctx.AddInParameter<bool>(cmd, "@RespectiveDept", app.RespectiveDept);
            ctx.AddInParameter<bool>(cmd, "@RespectivePlant", app.RespectivePlant);
            if (!string.IsNullOrEmpty(app.OperationType))
                ctx.AddInParameter<string>(cmd, "@OperationType", app.OperationType);
            if (app.ComponentID > default(int))
                ctx.AddInParameter<int>(cmd, "@ComponentID", app.ComponentID);

            ctx.AddInOutParameter<string>(cmd, "@RetCode", retVal, 25, System.Data.DbType.String);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public GetApprovalRolesDetails GetManageRolesDetails(GetApprovalUsersFilters obj)
        {
            var lst = new GetApprovalRolesDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspRPGetAssignedRolesAndUsers");
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResultAssinedRoles = ((IObjectContextAdapter)context).ObjectContext.Translate<RoleMasterSmall>(reader);
                lst.AssinedRoles = new RoleMasterSmallList();

                foreach (var rr in rrResultAssinedRoles)
                    lst.AssinedRoles.Add(rr);

                reader.NextResult();

                var rrResultAssignedRoleDept = ((IObjectContextAdapter)context).ObjectContext.Translate<DeptModules>(reader);
                lst.SelectedDeptList = new DeptModuleList();

                foreach (var rr in rrResultAssignedRoleDept)
                    lst.SelectedDeptList.Add(rr);

                reader.NextResult();

                var rrResultAssignedUsers = ((IObjectContextAdapter)context).ObjectContext.Translate<GetApprovalUsers>(reader);
                lst.AssignedUserList = new GetApprovalUsersList();

                foreach (var rr in rrResultAssignedUsers)
                    lst.AssignedUserList.Add(rr);

                reader.NextResult();

                var rrResultPlantDeptList = ((IObjectContextAdapter)context).ObjectContext.Translate<GetPlantDept>(reader);
                lst.PlantDeptList = new GetPlantDeptList();

                foreach (var rr in rrResultPlantDeptList)
                    lst.PlantDeptList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string DeleteApprovalTemplateDetailsByDetailID(int detailsID, TransResults tr)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATDeleteTemplateDetails");
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", tr.DeptCode);
            ctx.AddInParameter<int>(cmd, "@DetailID", detailsID);
            ctx.AddInOutParameter<string>(cmd, "@RetCode", retVal, 25, DbType.String);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public TemplateNewVersion InsertNewVersion(RolePermissionDetails det, TransResults tr)
        {
            var lst = new TemplateNewVersion();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATGenerateTemplateVersion");
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", tr.DeptCode);
            ctx.AddInParameter<int>(cmd, "@TemplateID", det.DecTemplateID);
            ctx.AddOutParameter(cmd, "@RetCode", DbType.String, 25);
            ctx.AddOutParameter(cmd, "@NewTemplateID", DbType.Int32, 4);
            cmd.ExecuteNonQuery();
            lst.ResultFlag = ctx.GetOutputParameterValue(cmd, "@RetCode");
            lst.NewTemplateID = Convert.ToInt32(ctx.GetOutputParameterValue(cmd, "@NewTemplateID"));
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return lst;
        }

        public string InsertApprovalPermission(ManageCapability cap, TransResults tr)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATInsertApprovalPermissions");
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", tr.DeptCode);
            ctx.AddInParameter<int>(cmd, "@EntityID", cap.EntityID);
            ctx.AddInParameter<string>(cmd, "@DeptCapXML", cap.XMLString);
            ctx.AddInOutParameter<string>(cmd, "@RetCode", retVal, 25, DbType.String);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public RolesDetails GetManageCapabilityRolesDetails(short permissionID, byte deptID)
        {
            var lst = new RolesDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATGetAppPermissionRoles");
            ctx.AddInParameter<short>(cmd, "@PermissionID", permissionID);
            ctx.AddInParameter<byte>(cmd, "@DepartmentID", deptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrMasterRoles = ((IObjectContextAdapter)context).ObjectContext.Translate<Role>(reader);
                lst.masterRoles = new RoleList();

                foreach (var rr in rrMasterRoles)
                    lst.masterRoles.Add(rr);

                reader.NextResult();

                var rrResultAssignedRoles = ((IObjectContextAdapter)context).ObjectContext.Translate<Role>(reader);
                lst.assinedRoles = new RoleList();

                foreach (var rr in rrResultAssignedRoles)
                    lst.assinedRoles.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string DeleteCapability(short permissionID, TransResults tr)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATDeleteApprovalPermissions");
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", tr.DeptCode);
            ctx.AddInParameter<short>(cmd, "@PermissionID", permissionID);
            ctx.AddInOutParameter<string>(cmd, "@RetCode", retVal, 25, DbType.String);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public string ManageCapabilityRoles(ManageApprovalTemplateRole role, TransResults tr, string XMLString)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATManageAppPermissionRoleAssignment");
            ctx.AddInParameter<int>(cmd, "@UserID", tr.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", tr.DeptCode);
            ctx.AddInParameter<short>(cmd, "@PermissionID", role.PermissionID);
            ctx.AddInParameter<string>(cmd, "@Roles", XMLString);
            ctx.AddInParameter<string>(cmd, "@RoleStatus", role.Type);
            ctx.AddInOutParameter<string>(cmd, "@RetCode", retVal, 20, DbType.String);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public List<Conditions> GetConditionSByEntityModulesByID(byte entityID)
        {
            var lst = new List<Conditions>();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATGetConditionsByModuleID");
            ctx.AddInParameter<byte>(cmd, "@EntityID", entityID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<Conditions>(reader);

                foreach (var rr in rrResult)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SingleUsrBasicList GetUsersByID(UsersByID obj)
        {
            var lst = new SingleUsrBasicList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspRPGetUsersByID");
            ctx.AddInParameter<short>(cmd, "@PlantID", obj.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", obj.DeptID);
            ctx.AddInParameter<int>(cmd, "@RoleID", obj.RoleID);
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleUsrBasic>(reader);

                foreach (var rr in rrResult)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }


        public SingleUsrBasicList GetRolesByID(UsersByID obj)
        {
            var lst = new SingleUsrBasicList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspRPGetRolesByID");
            ctx.AddInParameter<int>(cmd, "@DeptID", obj.DeptID);
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);

            if (obj.RoleID > default(int))
                ctx.AddInParameter<int>(cmd, "@RoleID", obj.RoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleUsrBasic>(reader);

                foreach (var rr in rrResult)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string ManageApprovalUsers(ManageApprovalUsers obj, TransResults trn)
        {

            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspRPManageApprovalUsers");
            ctx.AddInParameter<int>(cmd, "@UserID", trn.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", trn.DeptCode);
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);
            ctx.AddInParameter<string>(cmd, "@UsersXML", obj.XMLString);
            ctx.AddInParameter<string>(cmd, "@UserStatus", obj.Type);
            ctx.AddOutParameter(cmd, "@RetCode", DbType.String, 20);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public string ManageApprovalRoles(ManageApprovalUsers obj, TransResults trn)
        {
            string retVal = string.Empty;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATManageApprovalRoles");
            ctx.AddInParameter<int>(cmd, "@UserID", trn.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", trn.DeptCode);
            ctx.AddInParameter<int>(cmd, "@DetailID", obj.DetailID);
            ctx.AddInParameter<string>(cmd, "@Roles", obj.RoleXMLString);
            ctx.AddInParameter<string>(cmd, "@RoleStatus", obj.Type);
            ctx.AddOutParameter(cmd, "@RetCode", DbType.String, 20);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public ApprovalPermissions GetSelectedDeptCapabilities(byte entityID)
        {
            var lst = new ApprovalPermissions();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATGetSelectedDepartments");
            ctx.AddInParameter<byte>(cmd, "@EntityID", entityID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<ApprovalPermissions>(reader);

                foreach (var rr in rrResult)
                    lst = rr;

                reader.NextResult();

                var rrCapabilityList = ((IObjectContextAdapter)context).ObjectContext.Translate<ApprovalCapabilities>(reader);

                lst.AppCapaList = new ApprovalCapabilitiesList();

                foreach (var rr in rrCapabilityList)
                    lst.AppCapaList.Add(rr);

                reader.NextResult();

                var rrSelectedCapabilityList = ((IObjectContextAdapter)context).ObjectContext.Translate<ApprovalSelectedCapability>(reader);

                lst.SelectedAppCapaList = new ApprovalSelectedCapabilityList();

                foreach (var rr in rrSelectedCapabilityList)
                    lst.SelectedAppCapaList.Add(rr);

                reader.NextResult();

                var rrDeptList = ((IObjectContextAdapter)context).ObjectContext.Translate<DeptMasters>(reader);

                lst.DeptList = new DeptMasterLists();

                foreach (var rr in rrDeptList)
                    lst.DeptList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string DiscardTemplate(int templateID, TransResults trn)
        {
            string retVal = string.Empty;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspATDiscardTemplate");
            ctx.AddInParameter<int>(cmd, "@UserID", trn.UserID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@ModuleCode", trn.DeptCode);
            ctx.AddInParameter<int>(cmd, "@TemplateID", templateID);
            ctx.AddOutParameter(cmd, "@RetCode", DbType.String, 20);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;

        }

        public string ManageStatus(ManageStatus obj, TransResults trn)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspManageStatus");

            ctx.AddInParameter<string>(cmd, "@StatusCode", obj.StatusCode);
            ctx.AddInParameter<string>(cmd, "@Status", obj.Status);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddOutParameter(cmd, "@ResultFlag", DbType.String, 20);

            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@ResultFlag");

            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retVal;

        }

        public string ManageActionProvision(ManageActionProvision obj)
        {
            string retVal = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspManageActionDetails");
            ctx.AddInParameter<string>(cmd, "@ActionCode", obj.ActionCode);
            ctx.AddInParameter<string>(cmd, "@Action", obj.Action);

            ctx.AddInOutParameter<string>(cmd, "@RetCode", retVal, 25, DbType.String);
            cmd.ExecuteNonQuery();
            retVal = ctx.GetOutputParameterValue(cmd, "@RetCode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retVal;
        }

        public ActionsList GetAllActiveActions()
        {
            var lst = new ActionsList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetActionDetails");

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<ActionsDetails>(reader);

                foreach (var rr in rrResult)
                    lst.Add(rr);

                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

    }
}
