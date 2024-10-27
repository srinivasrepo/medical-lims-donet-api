using MedicalLIMSApi.Core.Entities.Login;
using MedicalLIMSApi.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MedicalLIMSApi.Web.App_Start
{
    public class MessageHandler : DelegatingHandler
    {
        StringBuilder sbError = null;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IEnumerable<string> keys;
            var token = request.Headers.TryGetValues("LIMSToken", out keys);

            if (token && !string.IsNullOrEmpty(keys.FirstOrDefault()))
            {
                EncryptingString encrypt = new EncryptingString();
                string decryptedToken = string.Empty;
                try
                {
                    var credentials = HandleTokenDecryption(keys.FirstOrDefault());
                    if (credentials != null)
                    {
                        var identity = new BasicAuthenticationIdentity(credentials.UserID.ToString(), credentials);
                        var principal = new GenericPrincipal(identity, null);

                        Thread.CurrentPrincipal = principal;
                        if (HttpContext.Current != null)
                            HttpContext.Current.User = principal;
                    }
                }
                catch { }
            }

            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                return response;
            }
            catch (Exception ex)
            {
                LogException(ex);

                throw ex;
            }
        }

        void LogException(Exception ex)
        {
            sbError = new StringBuilder();

            sbError.Append(Environment.NewLine);
            sbError.AppendLine("********************************************************************************");

            PrepareException(ex);

            sbError.Append(Environment.NewLine);

            string docName = DateTime.Now.ToString("MMddyyyy");
            string fullPath = HttpContext.Current.Server.MapPath(string.Format("~/UserFiles/{0}.txt", docName));

            File.AppendAllText(fullPath, sbError.ToString());
        }

        void PrepareException(Exception ex)
        {
            sbError.Append(Environment.NewLine);
            sbError.Append(ex.Message);

            if (ex.InnerException != null)
                PrepareException(ex.InnerException);
        }

        private UserDetails HandleTokenDecryption(string token)
        {

            try
            {

                string[] rawTokens = token.Split('|');
                string targetToken = rawTokens[0];

                targetToken = targetToken.Substring(1, (targetToken.Length - 2));
                UserDetails objUserDetails = new UserDetails();
                EncryptingString encrypt = new EncryptingString();

                string decryptedToken = string.Empty;
                decryptedToken = encrypt.decryptQueryString(targetToken);
                string[] path = decryptedToken.Split('|');
                string sourceToken = string.Empty;

                if (rawTokens.Length > 1)
                    sourceToken = rawTokens[1];

                int userID = default(int);
                byte plantID = default(byte);
                short roleID = default(short);
                int deptID = default(int);
                int userRoleID = default(int);
                int supplierID = default(int);
                string loginID = string.Empty;


                int.TryParse(path[0], out userID);
                byte.TryParse(path[1], out plantID);
                short.TryParse(path[2], out roleID);
                objUserDetails.DeptCode = Convert.ToString(path[3]);
                int.TryParse(path[4], out deptID);
                int.TryParse(path[5], out userRoleID);
                objUserDetails.PlantCode = Convert.ToString(path[6]);
                objUserDetails.TokenExpirationDate = Convert.ToDateTime(path[7]);
                objUserDetails.RoleType = path[8];
                objUserDetails.ExistingUserToken = path[9];

                int.TryParse(path[10], out supplierID);
                objUserDetails.LoginID = path[11];
                objUserDetails.RoleName = Convert.ToString(path[12]);

                //loginID = Convert.ToString(path[11]);

                objUserDetails.UserID = userID;
                objUserDetails.PlantID = plantID;
                objUserDetails.RoleID = roleID;
                objUserDetails.DeptID = deptID;
                objUserDetails.UserRoleID = userRoleID;
                objUserDetails.SupplierID = supplierID;

                //objUserDetails.LoginID = loginID;

                //objUserDetails.CurrentUserToken = userToken;
                return objUserDetails;
            }
            catch
            {
                return null;
            }
        }
    }

    public class LIMSAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;

            if (basicAuthenticationIdentity == null)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
            if (basicAuthenticationIdentity != null && DateTime.Now > basicAuthenticationIdentity.userDetails.TokenExpirationDate)
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent("USR_UN_AUTH") };  //new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }

    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public UserDetails userDetails { get; set; }

        public BasicAuthenticationIdentity(string userName, UserDetails objUserDetails)
            : base(userName, "Basic")
        {
            userDetails = objUserDetails;
        }
    }

    public static class APIContext
    {
        public static BasicAuthenticationIdentity Current
        {
            get
            {
                return (BasicAuthenticationIdentity)HttpContext.Current.User.Identity;
            }
        }
    }
}