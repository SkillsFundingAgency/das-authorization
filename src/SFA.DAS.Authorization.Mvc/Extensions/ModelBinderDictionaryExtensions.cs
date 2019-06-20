#if NET462
using System.Web.Mvc;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Mvc.ModelBinding;

namespace SFA.DAS.Authorization.Mvc.Extensions
{
    public static class ModelBinderDictionaryExtensions
    {
        public static void UseAuthorizationModelBinder(this ModelBinderDictionary binders)
        {
            binders.DefaultBinder = new AuthorizationModelBinder(() => DependencyResolver.Current.GetService<IAuthorizationContextProvider>());
        }
    }
}
#endif