#if NET462
using System.Globalization;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using WebApi.StructureMap;

namespace SFA.DAS.Authorization.WebApi
{
    public class AuthorizationModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (typeof(IAuthorizationContextMessage).IsAssignableFrom(bindingContext.ModelMetadata.ContainerType))
            {
                var authorizationContext = actionContext.Request.GetService<IAuthorizationContextProvider>().GetAuthorizationContext();

                if (authorizationContext.TryGet(bindingContext.ModelMetadata.PropertyName, out object value))
                {
                    var key = bindingContext.ModelName;
                    var valueProviderResult = new ValueProviderResult(value, value.ToString(), CultureInfo.InvariantCulture);

                    bindingContext.Model = value;
                    bindingContext.ModelState.SetModelValue(key, valueProviderResult);

                    return true;
                }
            }

            return false;
        }
    }
}
#endif