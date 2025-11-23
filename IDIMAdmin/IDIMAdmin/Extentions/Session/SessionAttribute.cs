using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Extentions.Session
{
    public class SessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            var userInformation = UserExtention.Get<UserInformation>(nameof(UserInformation));

            if (userInformation == null || userInformation.Applications.All(app => app.ApplicationId != userInformation.ApplicationId))
            {
                filterContext.Result = RedirectToLogin(filterContext);
                return;
            }

            var menus = userInformation.Menus.SelectMany(e => e.Menus).ToList();

            if (menus.Any(e => e.ControllerName.ToLower() == controller)) return;
            filterContext.Result = RedirectToLogin(filterContext);
        }

        private static ActionResult RedirectToLogin(ControllerContext filterContext)
        {
            return new RedirectToRouteResult
            (
                new RouteValueDictionary
                (
                    new
                    {
                        controller = "Login",
                        action = "Login",
                        returnUrl = filterContext.HttpContext.Request.Url?.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                    }
                )
            );
        }
    }
}