#if NET462
using System.Web.Mvc;
using SFA.DAS.Authorization.Mvc.Filters;
using SFA.DAS.Authorization.Services;

namespace SFA.DAS.Authorization.Mvc.Extensions
{
    public static class GlobalFilterCollectionExtensions
    {
        public static void AddAuthorizationFilter(this GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizationFilter(() => DependencyResolver.Current.GetService<IAuthorizationService>()));
        }

        public static void AddUnauthorizedAccessExceptionFilter(this GlobalFilterCollection filters)
        {
            filters.Add(new UnauthorizedAccessExceptionFilter());
        }
    }
}
#endif