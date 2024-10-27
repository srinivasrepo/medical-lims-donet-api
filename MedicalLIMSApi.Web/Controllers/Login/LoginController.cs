using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Login;
using MedicalLIMSApi.Core.Interface.Login;
using MedicalLIMSApi.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.Login
{
    public class LoginController : ApiController
    {
        ILogin db;
        StringBuilder sbError = null;

        public LoginController(ILogin db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("ValidateLogin")]
        public LoginDetails ValidateUserLogin(LoginUser credentials)
        {
            LoginDetails loginDetails;
            try
            {

                loginDetails = CommonStaticMethods.PostApiConnectionData<LoginDetails>("ValidateLogin", credentials);

                if (loginDetails.ReturnValue == "SUCCESS")
                {

                    TransResults trn = new TransResults();
                    loginDetails.Capabilities = db.GetUserCapabilities(loginDetails, trn.ApplicationType);

                }


            }
            catch (Exception e)
            {
                LogException(e);
                loginDetails = new LoginDetails();
                loginDetails.ReturnValue = sbError.ToString();
            }

            return loginDetails;

        }

        [HttpGet]
        [Route("GetSingleLoginUsers")]
        public Dictionary<string, string> GetSingleLoginUsers()
        {
            return SingleLoginApplicationCache.GetLoginData();
        }

        void LogException(Exception ex)
        {
            sbError = new StringBuilder();

            sbError.Append(Environment.NewLine);
            sbError.AppendLine("********************************************************************************");

            PrepareException(ex);

            sbError.Append(Environment.NewLine);
        }

        void PrepareException(Exception ex)
        {
            sbError.Append(Environment.NewLine);
            sbError.Append(ex.Message);

            if (ex.InnerException != null)
                PrepareException(ex.InnerException);
        }

        [HttpPost]
        [Route("SwitchPlant")]
        public string SwitchPlant(SwitchPlant switchPlant)
        {
            ChangeToken obj = new ChangeToken();
            obj.PlantID = switchPlant.PlantID;
            obj.DeptCode = switchPlant.DeptCode;
            obj.UserToken = Utilities.Common.GetUserDetails().UserToken;
            obj.UserID = Utilities.Common.GetUserDetails().UserID;
            var token = CommonStaticMethods.PostApiConnectionData<string>("ChangeTokenForAdmin", obj);
            return token;
        }

        [HttpPost]
        [Route("ValidateToken")]
        public LoginDetails ValidateToken(CommonLogin credentials)
        {
            LoginDetails loginDetails = CommonStaticMethods.PostApiConnectionData<LoginDetails>("ValidateToken", credentials);

            if (loginDetails.ReturnValue == "SUCCESS")
            {
                TransResults trn = new TransResults();

                loginDetails.Capabilities = db.GetUserCapabilities(loginDetails, trn.ApplicationType);

            }

            return loginDetails;
        }
    }
}
