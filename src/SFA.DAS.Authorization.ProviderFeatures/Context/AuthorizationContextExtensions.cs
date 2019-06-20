using SFA.DAS.Authorization.Context;

namespace SFA.DAS.Authorization.ProviderFeatures.Context
{
    public static class AuthorizationContextExtensions
    {
        public static void AddProviderFeatureValues(this IAuthorizationContext authorizationContext, long ukprn, string userEmail)
        {
            authorizationContext.Set(AuthorizationContextKey.Ukprn, ukprn);
            authorizationContext.Set(AuthorizationContextKey.UserEmail, userEmail);
        }
        
        internal static (long Ukprn, string UserEmail) GetProviderFeatureValues(this IAuthorizationContext authorizationContext)
        {
            return (authorizationContext.Get<long>(AuthorizationContextKey.Ukprn),
                authorizationContext.Get<string>(AuthorizationContextKey.UserEmail));
        }
    }
}