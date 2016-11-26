using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace MVCCustomAuthorizeAttribute.Filter
{
    /// <summary>
    /// Implement the IAuthorizationFilter to extend the authorization behavior 
    /// This happens in the real projects as we need some extra verification
    /// we implement the just method as OnAuthorization
    /// nice idea to also implement the FilterAttribute 
    /// </summary>
    public class CustomFullAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string[] _permissionList = new string[0];
        /// <summary>
        /// This method accepts just one parameter as AuthorizationContext which is derived from ControllerContext 
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {

            if (filterContext == null)
            {
                throw new ArgumentNullException("Context");
            }

            // This part of code is from microsoft
            bool isAuthorizationDisable = filterContext.ActionDescriptor.IsDefined
                                               (typeof(AllowAnonymousAttribute), inherit: true)
                                                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(
                                               typeof(AllowAnonymousAttribute), inherit: true);
            if (isAuthorizationDisable) { return; }

            if (AuthorizeCore(filterContext.HttpContext))
            {
                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, null);
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult("Not authorized");

            }
        }
        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            if (_permissionList.Length > 0 && !_permissionList.Contains(
                                     user.Identity.Name, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private void CacheValidateHandler(HttpContext context,
                      object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            bool isAuthorized = AuthorizeCore(httpContext);
            return (isAuthorized) ? HttpValidationStatus.Valid :
                                   HttpValidationStatus.IgnoreThisRequest;
        }
    }


}
