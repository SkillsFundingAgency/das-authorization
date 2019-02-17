#if NETCOREAPP2_0
using Microsoft.AspNetCore.Builder;

namespace SFA.DAS.Authorization.Mvc
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