#if NETCOREAPP2_0
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SFA.DAS.Authorization.Mvc
{
    public class AuthorizationModelBinder : IModelBinder
    {
        private readonly IAuthorizationContextProvider _authorizationContextProvider;

        public AuthorizationModelBinder(IAuthorizationContextProvider authorizationContextProvider)
        {
            _authorizationContextProvider = authorizationContextProvider;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var authorizationContext = _authorizationContextProvider.GetAuthorizationContext();

            if (authorizationContext.TryGet(bindingContext.ModelMetadata.PropertyName, out object value))
            {
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value, value.ToString());
                bindingContext.Result = ModelBindingResult.Success(value);
            }
            
            return Task.CompletedTask;
        }
    }
}
#elif NET462
using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;

namespace SFA.DAS.Authorization.Mvc
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
                var authorizationContext = _authorizationContextProvider().GetAuthorizationContext();

                if (authorizationContext.TryGet(propertyDescriptor.Name, out object value))
                {
                    var key = CreateSubPropertyName(bindingContext.ModelName, propertyDescriptor.Name);
                    var valueProviderResult = new ValueProviderResult(value, value.ToString(), CultureInfo.InvariantCulture);

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