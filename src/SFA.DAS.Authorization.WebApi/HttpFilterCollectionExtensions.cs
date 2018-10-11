#if NET462
using System.Web.Http.Filters;

namespace SFA.DAS.Authorization.WebApi
{
    public static class HttpFilterCollectionExtensions
    {
        public static void AddAuthorizationFilter(this HttpFilterCollection filters)
        {
            filters.Add(new AuthorizationFilter());
        }

        public static void AddUnauthorizedAccessExceptionFilter(this HttpFilterCollection filters)
        {
            filters.Add(new UnauthorizedAccessExceptionFilter());
        }
    }
}
#endif