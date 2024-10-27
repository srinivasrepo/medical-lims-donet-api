using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MedicalLIMSApi.Infrastructure.Context
{
    public class DBInfo
    {
        private static DBInfo _instance = null;

        private static object _threadLock = new object();

        private string _connectionString = string.Empty;

        private static string hash = "!#$d51?27";
        private DBInfo()
        {
            string encrypted = string.Empty;

            encrypted = ConfigurationManager.AppSettings["CS_LIMS"];

            _connectionString = this.Decrypt(encrypted);
        }

        public static DBInfo GetInstance()
        {
            lock (_threadLock)
            {
                if (_instance == null)
                    _instance = new DBInfo();
            }

            return _instance;
        }

        public string ConnectionString { get { return _connectionString; } set { _connectionString = value; } }

        private string Decrypt(string stringToDecrypt)
        {
            string retValue = string.Empty;

            byte[] key = { };
            byte[] IV = { 10, 20, 30, 40 };

            stringToDecrypt = stringToDecrypt.Replace(" ", "+");

            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            try
            {
                key = Encoding.UTF8.GetBytes(hash);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                inputByteArray = Convert.FromBase64String(stringToDecrypt);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);

                        cs.FlushFinalBlock();

                        Encoding encoding = Encoding.UTF8;

                        retValue = encoding.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {

            }

            return retValue;
        }
    }
}
