#if NET462
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
            if (typeof(IAuthorizationContextMessage).IsAssignableFrom(bindingContext.ModelType))
            {
                var authorizationContext = _authorizationContextProvider().GetAuthorizationContext();

                if (authorizationContext.TryGet(propertyDescriptor.Name, out object value))
                {
                    var key = CreateSubPropertyName(bindingContext.ModelName, propertyDescriptor.Name);
                    var valueProviderResult = new ValueProviderResult(value, value.ToString(), CultureInfo.InvariantCulture);

                    propertyDescriptor.SetValue(bindingContext.Model, value);
                    bindingContext.ModelState.SetModelValue(key, valueProviderResult);
                }

                return;
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}
#endif