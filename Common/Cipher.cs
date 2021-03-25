using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class Cipher
    {
        public static string password = "yeliz_ay_" + DateTime.Now.ToString("MMyyyy");

        public static string Encrypt(string encryptString)
        {
            try
            {
                return encryptString;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                return cipherText;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}