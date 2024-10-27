using MedicalLIMSApi.Core.Entities.Login;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Interface.Login
{
    public interface ILogin
    {
        List<EnityCapabilities> GetUserCapabilities(LoginDetails loginDetails, string applicationType);

        string GetLoginPassword(string loginID, int userID);

        LoginEncryptedDetails ValidateUserLogin(LoginUser login);
    }
}
