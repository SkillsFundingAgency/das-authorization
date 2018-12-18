using System.Web.Mvc;

namespace SFA.DAS.Authorization.Mvc
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