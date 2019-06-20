#if NETCOREAPP2_0
using Microsoft.AspNetCore.Builder;
using SFA.DAS.Authorization.Mvc.Middleware;

namespace SFA.DAS.Authorization.Mvc.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUnauthorizedAccessExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UnauthorizedAccessExceptionMiddleware>();
        }
    }
}
#endif