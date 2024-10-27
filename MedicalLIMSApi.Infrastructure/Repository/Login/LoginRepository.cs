using MedicalLIMSApi.Core.Entities.Login;
using MedicalLIMSApi.Core.Interface.Login;
using MedicalLIMSApi.Infrastructure.Context;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.Login
{
    public class LoginRepository : ILogin
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public List<EnityCapabilities> GetUserCapabilities(LoginDetails loginDetails, string applicationType)
        {
            List<EnityCapabilities> lst = new List<EnityCapabilities>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "usr.uspUSRGetCapabilities");
            ctx.AddInParameter<int>(cmd, "@UserID", loginDetails.UserID);
            ctx.AddInParameter<int>(cmd, "@RoleID", loginDetails.RoleID);
            ctx.AddInParameter<byte>(cmd, "@DeptID", loginDetails.DeptID);
            ctx.AddInParameter<string>(cmd, "@ApplicationType", applicationType);
            ctx.AddInParameter<string>(cmd, "@RoleType", loginDetails.RoleType);

            using (var reader = cmd.ExecuteReader())
            {
                var capabilities = ((IObjectContextAdapter)context).ObjectContext.Translate<EnityCapabilities>(reader);
                foreach (var item in capabilities)
                    lst.Add(item);
            }

            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return lst;
        }

        public string GetLoginPassword(string loginID, int userID)
        {
            string dbPassword = string.Empty;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "ams.uspUSRGetPassword");

            if (!string.IsNullOrEmpty(loginID))
                ctx.AddInParameter<string>(cmd, "@LoginID", loginID);

            if (userID > default(int))
                ctx.AddInParameter<int>(cmd, "@UserID", userID);

            ctx.AddOutParameter(cmd, "@Password", System.Data.DbType.String, 255);
            cmd.ExecuteNonQuery();
            dbPassword = ctx.GetOutputParameterValue(cmd, "@Password");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return dbPassword;
        }

        public LoginEncryptedDetails ValidateUserLogin(LoginUser login)
        {
            LoginEncryptedDetails obj = new LoginEncryptedDetails();
            obj.Details = new LoginDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "ams.uspUSRValidateSupplierDetails");
            ctx.AddInParameter<string>(cmd, "@LoginID", login.LoginID);
            ctx.AddOutParameter(cmd, "@Result", System.Data.DbType.String, 10);

            using (var reader = cmd.ExecuteReader())
            {
                var encDetails = ((IObjectContextAdapter)context).ObjectContext.Translate<LoginEncryptedDetails>(reader);
                foreach (var item in encDetails)
                {
                    obj.UserID = item.UserID;
                    obj.RoleType = item.RoleType;
                    obj.UserRoleID = item.UserRoleID;
                }

                reader.NextResult();

                var details = ((IObjectContextAdapter)context).ObjectContext.Translate<LoginDetails>(reader);
                foreach (var item in details)
                    obj.Details.LastLoginDate = item.LastLoginDate;
            }

            obj.Details.ReturnValue = ctx.GetOutputParameterValue(cmd, "@Result");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return obj;
        }

    }
}
