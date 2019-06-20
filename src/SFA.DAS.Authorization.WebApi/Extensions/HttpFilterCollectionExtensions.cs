using System.Web.Http.Filters;
using SFA.DAS.Authorization.WebApi.Filters;

namespace SFA.DAS.Authorization.WebApi.Extensions
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