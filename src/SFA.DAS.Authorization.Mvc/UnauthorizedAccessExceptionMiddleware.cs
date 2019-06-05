#if NETCOREAPP2_0
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.Authorization.Mvc
{
    public class UnauthorizedAccessExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedAccessExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
#endif