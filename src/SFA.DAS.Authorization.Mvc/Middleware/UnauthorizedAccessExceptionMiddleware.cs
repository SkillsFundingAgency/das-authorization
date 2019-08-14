#if NETCOREAPP2_0
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Authorization.Mvc.Middleware
{
    public class UnauthorizedAccessExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnauthorizedAccessExceptionMiddleware> _logger;

        public UnauthorizedAccessExceptionMiddleware(RequestDelegate next, ILogger<UnauthorizedAccessExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized Access");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
#endif