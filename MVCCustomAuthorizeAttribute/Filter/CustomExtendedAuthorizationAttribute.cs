using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace MVCCustomAuthorizeAttribute.Filter
{
    public class CustomExtendedAuthorizationAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Good trick to add your request context verification here 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
        /// <summary>
        /// Best Place to implemente your special roles , activities and permissions 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected sealed override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            IPrincipal contextUser = httpContext.User;
            if (!contextUser.Identity.IsAuthenticated)
            {
                return false;
            }

            /*
               your custom Code to extend the default mvc authorization will be here 
               if you need to add some complexity to be the final role of your application and not let to be changed 
               set the method as sealed  
            */

            return base.AuthorizeCore(httpContext);

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }

        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            return base.OnCacheAuthorization(httpContext);
        }
    }
}