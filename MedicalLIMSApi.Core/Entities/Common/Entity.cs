using MedicalLIMSApi.Core.CommonMethods;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Entities.Common
{
    public class Entity
    {
        public int EntityID { get; set; }

        public string EntityName { get; set; }

        public string EntityType { get; set; }

        public string BasePath { get; set; }
    }


    public class UserToDoListCount
    {
        public short ConditionID { get; set; }

        public string Condition { get; set; }

        public int TotalCount { get; set; }

        public string EntityType { get; set; }

        public int? ExecSubStatus { get; set; }
    }
    public class UserToDoListCountList : List<UserToDoListCount> { }

    public class GetTodoListCountDetails
    {
        public UserToDoListCountList TodoList { get; set; }

        public GetSamplePlanAnalystsDetailsList SamplePlan { get; set; }

    }

    public class GetSamplePlanAnalystsDetails
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public int AssignedCount { get; set; }

        public int UserRoleID { get; set; }

        public string  EncUserRoleID { get { return CommonStaticMethods.Encrypt(UserRoleID.ToString()); } }

    }
    public class GetSamplePlanAnalystsDetailsList : List<GetSamplePlanAnalystsDetails> { }

    public class NavItem
    {
        public string DisplayName { get; set; }

        public string IconName { get; set; }

        public string Route { get; set; }

        public string EntityType { get; set; }

        public string EntityCode { get; set; }

        public List<NavItem> Children { get; set; }
    }

    public class MenuList
    {
        public List<NavItem> MainList { get; set; }

        public List<NavItem> Childs { get; set; }

        public List<NavItem> NewReqList { get; set; }
    }

    public class AuditUnderRecords
    {
        public string EntityName { get; set; }

        public string EntityType { get; set; }

        public int AuditsCount { get; set; }

        public string HasCapable { get; set; }

        public int SchedulePrograms { get; set; }
    }

    public class AuditUnderRecordsList : List<AuditUnderRecords> { }
}
