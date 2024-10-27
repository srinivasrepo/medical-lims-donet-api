using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MedicalLIMSApi.Web.Utilities
{
    public class EncryptingString
    {
        public string encryptQueryString(string strQueryString)
        {
            return Encrypt(strQueryString, "!#$a54?3");
        }

        public string decryptQueryString(string strQueryString)
        {
            return Decrypt(strQueryString, "!#$a54?3");
        }

        public static string Encrypt(string strQueryString)
        {
            return Encrypt(strQueryString, "!#$a54?3");
        }

        public static T Decrypt<T>(string strQueryString)
        {
            T target = default(T);
            try
            {
                target = (T)Convert.ChangeType(Decrypt(strQueryString, "!#$a54?3"), typeof(T));
            }
            catch
            {
                if (strQueryString != "undefined" && strQueryString != null)
                    target = (T)Convert.ChangeType(strQueryString, typeof(T));
            }

            return target;

            //return Decrypt(strQueryString, "!#$a54?3");
        }
        protected static string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] key = { };
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };

            stringToDecrypt = stringToDecrypt.Replace(" ", "+");

            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            try
            {
                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();

                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                Encoding encoding = Encoding.UTF8; return encoding.GetString(ms.ToArray());
            }

            catch (System.Exception ex)
            {
                if (ex.Message == "Invalid length for a Base-64 char array.")
                {
                    stringToDecrypt = stringToDecrypt + "+";
                    return Decrypt(stringToDecrypt, sEncryptionKey);
                }
                else
                    throw ex;
            }
        }

        protected static string Encrypt(string stringToEncrypt, string sEncryptionKey)
        {
            byte[] key = { };
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };

            byte[] inputByteArray; //Convert.ToByte(stringToEncrypt.Length)

            try
            {
                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();

                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray()).Replace("+", " ");
            }

            catch (System.Exception ex)
            {

                throw ex;
            }

        }//end of Encrypt
    }
}