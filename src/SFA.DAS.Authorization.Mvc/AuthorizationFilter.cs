#if NET462
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SFA.DAS.Authorization.Mvc
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        private readonly Func<IAuthorizationService> _authorizationService;

        public AuthorizationFilter(Func<IAuthorizationService> authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dasAuthorizeAttributes = filterContext.ActionDescriptor.GetDasAuthorizeAttributes().ToList();

            if (dasAuthorizeAttributes.Any())
            {
                var options = dasAuthorizeAttributes.SelectMany(a => a.Options).ToArray();
                var isAuthorized = _authorizationService().IsAuthorized(options);

                if (!isAuthorized)
                {
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }
        }
    }
}
#endif