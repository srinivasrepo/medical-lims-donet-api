using System;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Entities.Login
{
    public class LoginUser
    {
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string Purpose { get; set; }
    }

    public class CommonLogin
    {
        public string Key { get; set; }
        
        public string Purpose { get; set; }
    }

    public class LoginDetails
    {
        public string Name { get; set; }

        public string ShotName { get; set; }

        public string Token { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string ReturnValue { get; set; }

        public string RoleName { get; set; }

        public byte DeptID { get; set; }

        public int RoleID { get; set; }

        public string RoleType { get; set; }

        public int UserID { get; set; }

        public string PlantName { get; set; }

        public string DeptName { get; set; }

        public int ? SupplierID { get; set; }

        public string LoginID { get; set; }

        public List<EnityCapabilities> Capabilities { get; set; }
    }

    public class UserDetails
    {
        public int UserID { get; set; }

        public byte PlantID { get; set; }

        public string DeptCode { get; set; }

        public string PlantCode { get; set; }

        public int DeptID { get; set; }

        public short RoleID { get; set; }

        public DateTime TokenExpirationDate { get; set; }

        public int UserRoleID { get; set; }

        public string RoleType { get; set; }

        public int ? SupplierID { get; set; }

        public string LoginID { get; set; }

        public string RoleName { get; set; }

        public string ExistingUserToken { get; set; }
    }

    public class EnityCapabilities
    {
        public string CapabilityCode { get; set; }

        public string EntityCode { get; set; }

    }

    public class LoginEncryptedDetails
    {
        public int UserID { get; set; }

        public short PlantID { get; set; }

        public string DeptCode { get; set; }

        public int RoleID { get; set; }

        public int DeptID { get; set; }

        public int UserRoleID { get; set; }

        public string PlantCode { get; set; }

        public string RoleType { get; set; }

        public LoginDetails Details { get; set; }

        public List<EnityCapabilities> Capabilities { get; set; }

        public string PlantName { get; set; }

        public string DeptName { get; set; }
    }

    public class ChangeToken
    {
        public byte PlantID { get; set; }

        public string DeptCode { get; set; }

        public string UserToken { get; set; }

        public int UserID { get; set; }
    }

  
}
