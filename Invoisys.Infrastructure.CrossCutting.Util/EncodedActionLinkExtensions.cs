using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Invoisys.Infrastructure.CrossCutting.Util
{
    public static class EncodedActionLinkExtensions
    {
        public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string actionUrl, object routeValues,
            object htmlAttributes, string iconClass)
        {
            var htmlAttributesString = string.Empty;
            var queryString = string.Empty;
            if (routeValues != null)
            {
                var d = new RouteValueDictionary(routeValues);
                for (var i = 0; i < d.Keys.Count; i++)
                {
                    if (i > 0) queryString += "?";
                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }
            if (htmlAttributes != null)
            {
                var d = new RouteValueDictionary(htmlAttributes);
                for (var i = 0; i < d.Keys.Count; i++)
                {
                    htmlAttributesString += d.Keys.ElementAt(i) + "='" + d.Values.ElementAt(i) + "'";
                }
            }
            var ancor = new StringBuilder();
            ancor.Append("<a href='");
            if (actionUrl != string.Empty) ancor.Append(actionUrl);
            if (queryString != string.Empty) ancor.Append("?q=" + HttpUtility.UrlEncode(Encrypt(queryString)));
            ancor.Append("' ");
            if (htmlAttributesString != string.Empty) ancor.Append(htmlAttributesString);
            ancor.Append("><i class='" + iconClass + "'></i>");
            ancor.Append("");
            return new MvcHtmlString(ancor.ToString());
        }
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
                    using (var cStream = new CryptoStream(ms, des.CreateEncryptor(encryptKey, iv), CryptoStreamMode.Write))
                    {
                        cStream.Write(inputByte, 0, inputByte.Length);
                        cStream.FlushFinalBlock();
                    }
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
                    using (var cs = new CryptoStream(ms, des.CreateDecryptor(decryptKey, iv), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByte, 0, inputByte.Length);
                        cs.FlushFinalBlock();
                    }
                    var encoding = Encoding.UTF8;
                    return encoding.GetString(ms.ToArray());
                }
            }
        }
    }
}