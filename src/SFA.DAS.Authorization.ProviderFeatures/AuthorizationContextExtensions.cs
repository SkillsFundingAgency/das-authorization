using System;

namespace SFA.DAS.Authorization.ProviderFeatures
{
    public static class AuthorizationContextExtensions
    {
        public static void AddProviderFeatureValues(this IAuthorizationContext authorizationContext, long? ukprn, string userEmail)
        {
            authorizationContext.Set(AuthorizationContextKey.Ukprn, ukprn);
            authorizationContext.Set(AuthorizationContextKey.UserEmail, userEmail);
        }
        
        internal static (long Ukprn, string UserEmail) GetProviderFeatureValues(this IAuthorizationContext authorizationContext)
        {
            var ukprn = authorizationContext.Get<long?>(AuthorizationContextKey.Ukprn);
            var userEmail = authorizationContext.Get<string>(AuthorizationContextKey.UserEmail);

            if (ukprn == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.Ukprn}' cannot be null");
            }

            if (string.IsNullOrWhiteSpace(userEmail))
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.UserEmail}' cannot be null or white space");
            }
            
            return (ukprn.Value, userEmail);
        }
    }
}