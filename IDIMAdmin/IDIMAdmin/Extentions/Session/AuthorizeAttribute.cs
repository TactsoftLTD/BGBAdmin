using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace IDIMAdmin.Extentions.Session
{
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!(filterContext.Result is HttpUnauthorizedResult)) return;

            string returnUrl = null;
            if (filterContext.HttpContext.Request.HttpMethod.Equals("GET", System.StringComparison.CurrentCultureIgnoreCase))
                returnUrl = filterContext.HttpContext.Request.Url?.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);

            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
            {
                //{ "client", filterContext.RouteData.Values[ "client" ] },
                { "controller", "Account" },
                { "action", "Login" },
                { "returnUrl", returnUrl }
            });
        }
    }
}