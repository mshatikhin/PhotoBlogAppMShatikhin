using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoBlogApp.Helpers
{
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {

        public string LoginController { get; set; }
        public string LoginAction { get; set; }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(LoginController) && string.IsNullOrEmpty(LoginAction))
                base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary(new
                                    {
                                        controller = LoginController,
                                        action = LoginAction,
                                        returnUrl = HttpContext.Current.Request.Url
                                    }));
        }
    }
}