#if NETCOREAPP2_0
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.Authorization.Mvc.ModelBinding
{
    public class AuthorizationModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (typeof(IAuthorizationContextModel).IsAssignableFrom(context.Metadata.ContainerType) && !context.Metadata.IsComplexType)
            {
                var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
                var simpleTypeModelBinder = new SimpleTypeModelBinder(context.Metadata.ModelType, loggerFactory);
                var errorSuppressModelBinder = new ErrorSuppressModelBinder();
                var authorizationModelBinder = new AuthorizationModelBinder(simpleTypeModelBinder, errorSuppressModelBinder);

                return authorizationModelBinder;
            }

            return null;
        }
    }
}
#endif