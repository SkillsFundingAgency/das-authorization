#if NET462
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi.StructureMap;

namespace SFA.DAS.Authorization.WebApi
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var dasAuthorizeAttributes = actionContext.ActionDescriptor.GetDasAuthorizeAttributes();

            if (dasAuthorizeAttributes.Any())
            {
                var options = dasAuthorizeAttributes.SelectMany(a => a.Options).ToArray();
                var isAuthorized = actionContext.Request.GetService<IAuthorizationService>().IsAuthorized(options);

                if (!isAuthorized)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                }
            }
        }
    }
}
#endif