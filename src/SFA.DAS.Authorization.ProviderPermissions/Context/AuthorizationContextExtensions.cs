using System;
using SFA.DAS.Authorization.Context;

namespace SFA.DAS.Authorization.ProviderPermissions.Context
{
    public static class AuthorizationContextExtensions
    {
        public static void AddProviderPermissionValues(this IAuthorizationContext authorizationContext, long? accountLegalEntityId, long? ukprn)
        {
            authorizationContext.Set(AuthorizationContextKey.Ukprn, ukprn);
            authorizationContext.Set(AuthorizationContextKey.AccountLegalEntityId, accountLegalEntityId);
        }
        
        internal static (long Ukprn, long AccountLegalEntityId) GetProviderPermissionValues(this IAuthorizationContext authorizationContext)
        {
            var ukprn = authorizationContext.Get<long?>(AuthorizationContextKey.Ukprn);
            var accountLegalEntityId = authorizationContext.Get<long?>(AuthorizationContextKey.AccountLegalEntityId);

            if (ukprn == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.Ukprn}' cannot be null");
            }

            if (accountLegalEntityId == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.AccountLegalEntityId}' cannot be null");
            }
            
            return (ukprn.Value, accountLegalEntityId.Value);
        }
    }
}