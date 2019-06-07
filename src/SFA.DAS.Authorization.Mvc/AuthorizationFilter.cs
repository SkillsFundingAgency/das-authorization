#if NETCOREAPP2_0
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SFA.DAS.Authorization.Mvc
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationFilter(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var dasAuthorizeAttributes = controllerActionDescriptor.GetDasAuthorizeAttributes();

            if (dasAuthorizeAttributes.Count > 0)
            {
                var options = dasAuthorizeAttributes.SelectMany(a => a.Options).ToArray();
                var isAuthorized = await _authorizationService.IsAuthorizedAsync(options).ConfigureAwait(false);

                if (!isAuthorized)
                {
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                }
            }
        }
    }
}
#elif NET462
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
            var dasAuthorizeAttributes = filterContext.ActionDescriptor.GetDasAuthorizeAttributes();

            if (dasAuthorizeAttributes.Count > 0)
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