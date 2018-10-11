#if NET462
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace SFA.DAS.Authorization.WebApi
{
    public class UnauthorizedAccessExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is UnauthorizedAccessException)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }
    }
}
#endif