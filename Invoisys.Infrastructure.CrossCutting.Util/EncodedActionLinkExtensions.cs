using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Invoisys.Infrastructure.CrossCutting.Util
{
    public static class EncodedActionLinkExtensions
    {
        public static string Encrypt(string plainText)
        {
            const string key = "KeyInvoisys#";
            byte[] iv = { 55, 34, 87, 64, 87, 195, 54, 21 };
            var encryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            using (var des = new DESCryptoServiceProvider())
            {
                var inputByte = Encoding.UTF8.GetBytes(plainText);
                using (var ms = new MemoryStream())
                {
                    var cStream = new CryptoStream(ms, des.CreateEncryptor(encryptKey, iv), CryptoStreamMode.Write);
                    cStream.Write(inputByte, 0, inputByte.Length);
                    cStream.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        public static string Decrypt(string encryptedText)
        {
            const string key = "KeyInvoisys#";
            byte[] iv = { 55, 34, 87, 64, 87, 195, 54, 21 };
            var decryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            using (var des = new DESCryptoServiceProvider())
            {
                var inputByte = Convert.FromBase64String(encryptedText);
                using (var ms = new MemoryStream())
                {
                    var cs = new CryptoStream(ms, des.CreateDecryptor(decryptKey, iv), CryptoStreamMode.Write);
                    cs.Write(inputByte, 0, inputByte.Length);
                    cs.FlushFinalBlock();
                    var encoding = Encoding.UTF8;
                    return encoding.GetString(ms.ToArray());
                }
            }
        }
    }
}