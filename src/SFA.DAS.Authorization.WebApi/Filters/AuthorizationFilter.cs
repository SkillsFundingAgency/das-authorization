using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using SFA.DAS.Authorization.Services;
using SFA.DAS.Authorization.WebApi.Extensions;
using WebApi.StructureMap;

namespace SFA.DAS.Authorization.WebApi.Filters
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var dasAuthorizeAttributes = actionContext.ActionDescriptor.GetDasAuthorizeAttributes();

            if (dasAuthorizeAttributes.Count > 0)
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