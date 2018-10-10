#if NET462
using System.Web.Mvc;

namespace SFA.DAS.Authorization.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static AuthorizationResult GetAuthorizationResult(this HtmlHelper htmlHelper, params string[] options)
        {
            var authorizationService = DependencyResolver.Current.GetService<IAuthorizationService>();
            var authorizationResult = authorizationService.GetAuthorizationResult(options);

            return authorizationResult;
        }

        public static bool IsAuthorized(this HtmlHelper htmlHelper, params string[] options)
        {
            var authorizationService = DependencyResolver.Current.GetService<IAuthorizationService>();
            var isAuthorized = authorizationService.IsAuthorized(options);

            return isAuthorized;
        }
    }
}
#endif