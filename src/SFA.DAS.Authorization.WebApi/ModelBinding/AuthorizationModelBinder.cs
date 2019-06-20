using System.Globalization;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.ModelBinding;
using WebApi.StructureMap;

namespace SFA.DAS.Authorization.WebApi.ModelBinding
{
    public class AuthorizationModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (typeof(IAuthorizationContextModel).IsAssignableFrom(bindingContext.ModelMetadata.ContainerType))
            {
                var authorizationContextProvider = actionContext.Request.GetService<IAuthorizationContextProvider>();
                var authorizationContext = authorizationContextProvider.GetAuthorizationContext();

                if (authorizationContext.TryGet(bindingContext.ModelMetadata.PropertyName, out object value))
                {
                    var valueProviderResult = new ValueProviderResult(value, value?.ToString(), CultureInfo.InvariantCulture);

                    bindingContext.Model = value;
                    bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                    return true;
                }
            }

            return false;
        }
    }
}