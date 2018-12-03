using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Invoisys.Infrastructure.CrossCutting.Util
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var decryptedParameters = new Dictionary<string, object>();
            if (HttpContext.Current.Request.QueryString.Get("q") != null)
            {
                var encryptedQueryString = HttpContext.Current.Request.QueryString.Get("q");
                var decrptedString = EncodedActionLinkExtensions.Decrypt(encryptedQueryString);
                var paramsArrs = decrptedString.Split('?');
                foreach (var t in paramsArrs)
                {
                    var paramArr = t.Split('=');
                    decryptedParameters.Add(paramArr[0], Convert.ToInt32(paramArr[1]));
                }
            }
            else filterContext.ActionParameters["id"] = null;
            for (var i = 0; i < decryptedParameters.Count; i++)
            {
                filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = decryptedParameters.Values.ElementAt(i);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}