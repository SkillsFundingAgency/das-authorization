#if NETCOREAPP2_0
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Authorization.Mvc
{
    public static class MvcOptionsExtensions
    {
        public static void AddDasAuthorization(this MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add<AuthorizationFilter>();
            mvcOptions.Filters.Add<UnauthorizedAccessExceptionFilter>();
            mvcOptions.ModelBinderProviders.Insert(0, new AuthorizationModelBinderProvider());
        }
    }
}
#endif