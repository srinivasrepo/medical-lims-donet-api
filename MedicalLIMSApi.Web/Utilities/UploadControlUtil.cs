using System;

namespace MedicalLIMSApi.Web.Utilities
{
    public class MedicalLIMSInvalidSourceException : Exception
    {
        public MedicalLIMSInvalidSourceException(string message)
            : base(message)
        {

        }
    }
    public class UploadControlUtil : Exception
    {
    }
}