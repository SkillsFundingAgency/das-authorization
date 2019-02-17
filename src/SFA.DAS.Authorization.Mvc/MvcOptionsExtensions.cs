#if NETCOREAPP2_0
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Authorization.Mvc
{
    public static class MvcOptionsExtensions
    {
        public static void AddAuthorization(this MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add<AuthorizationFilter>();
            mvcOptions.ModelBinderProviders.Insert(0, new AuthorizationModelBinderProvider());
        }
    }
}
#endif