#if NETCOREAPP2_0
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Mvc.Attributes;

namespace SFA.DAS.Authorization.Mvc.ModelBinding
{
    public class AuthorizationModelBinder : IModelBinder
    {
        private readonly IModelBinder _fallbackModelBinder;
        private readonly IModelBinder _errorSuppresArgumentExceptionBinder;

        public AuthorizationModelBinder(IModelBinder fallbackModelBinder, IModelBinder errorSuppresArgumentExceptionBinder = null)
        {
            _fallbackModelBinder = fallbackModelBinder;
            _errorSuppresArgumentExceptionBinder = errorSuppresArgumentExceptionBinder;
        }


        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var authorizationContextProvider = bindingContext.HttpContext.RequestServices.GetService<IAuthorizationContextProvider>();
            var authorizationContext = authorizationContextProvider.GetAuthorizationContext();

            if (authorizationContext.TryGet(bindingContext.ModelMetadata.PropertyName, out object value))
            {
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value, value?.ToString());
                bindingContext.Result = ModelBindingResult.Success(value);

                return Task.CompletedTask;
            }
            else if (_errorSuppresArgumentExceptionBinder != null)
            {
                bool isErrroSuppressProperty = ((bindingContext.ModelMetadata as DefaultModelMetadata)?.Attributes?.PropertyAttributes?.Where(x => x.GetType() == typeof(ErrorSuppressArgumentExceptionAttribute))?.Count() ?? 0) > 0 ;
                if (isErrroSuppressProperty)
                {
                    _errorSuppresArgumentExceptionBinder.BindModelAsync(bindingContext);
                    return Task.CompletedTask;
                }
            }

            return _fallbackModelBinder.BindModelAsync(bindingContext);
        }
    }
}
#elif NET462
using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.Authorization.Mvc.ModelBinding
{
    public class AuthorizationModelBinder : DefaultModelBinder
    {
        private readonly Func<IAuthorizationContextProvider> _authorizationContextProvider;

        public AuthorizationModelBinder(Func<IAuthorizationContextProvider> authorizationContextProvider)
        {
            _authorizationContextProvider = authorizationContextProvider;
        }

        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            if (typeof(IAuthorizationContextModel).IsAssignableFrom(bindingContext.ModelType))
            {
                var authorizationContextProvider = _authorizationContextProvider();
                var authorizationContext = authorizationContextProvider.GetAuthorizationContext();

                if (authorizationContext.TryGet(propertyDescriptor.Name, out object value))
                {
                    var key = CreateSubPropertyName(bindingContext.ModelName, propertyDescriptor.Name);
                    var valueProviderResult = new ValueProviderResult(value, value?.ToString(), CultureInfo.InvariantCulture);

                    propertyDescriptor.SetValue(bindingContext.Model, value);
                    bindingContext.ModelState.SetModelValue(key, valueProviderResult);

                    return;
                }
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}
#endif