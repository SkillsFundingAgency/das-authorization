#if NETCOREAPP2_0
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.Mvc.Filters;
using SFA.DAS.Authorization.Mvc.ModelBinding;

namespace SFA.DAS.Authorization.Mvc.Extensions
{
    public static class MvcOptionsExtensions
    {
        public static void AddAuthorization(this MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add<AuthorizationFilter>(int.MaxValue);
            mvcOptions.ModelBinderProviders.Insert(0, new AuthorizationModelBinderProvider());
        }
    }
}
#endif