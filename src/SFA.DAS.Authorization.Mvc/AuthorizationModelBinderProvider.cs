#if NETCOREAPP2_0
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace SFA.DAS.Authorization.Mvc
{
    public class AuthorizationModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return typeof(IAuthorizationContextModel).IsAssignableFrom(context.Metadata.ContainerType)
                ? new BinderTypeModelBinder(typeof(AuthorizationModelBinder))
                : null;
        }
    }
}
#endif