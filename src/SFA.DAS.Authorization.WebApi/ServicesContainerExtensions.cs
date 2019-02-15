using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace SFA.DAS.Authorization.WebApi
{
    public static class ServicesContainerExtensions
    {
        public static void UseAuthorizationModelBinder(this ServicesContainer services)
        {
            services.Insert(typeof(ModelBinderProvider), 0, new AuthorizationModelBinderProvider());
        }
    }
}