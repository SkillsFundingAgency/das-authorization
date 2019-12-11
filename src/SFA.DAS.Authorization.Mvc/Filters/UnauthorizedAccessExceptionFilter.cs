#if NET462
using System;
using System.Net;
using System.Web.Mvc;

namespace SFA.DAS.Authorization.Mvc.Filters
{
    public class UnauthorizedAccessExceptionFilter : IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is UnauthorizedAccessException)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
#endif