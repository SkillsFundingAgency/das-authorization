#if NET462
using System.Web.Mvc;

namespace SFA.DAS.Authorization.Mvc
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