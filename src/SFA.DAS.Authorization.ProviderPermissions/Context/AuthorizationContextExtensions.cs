using SFA.DAS.Authorization.Context;

namespace SFA.DAS.Authorization.ProviderPermissions.Context
{
    public static class AuthorizationContextExtensions
    {
        public static void AddProviderPermissionValues(this IAuthorizationContext authorizationContext, long accountLegalEntityId, long ukprn)
        {
            authorizationContext.Set(AuthorizationContextKey.Ukprn, ukprn);
            authorizationContext.Set(AuthorizationContextKey.AccountLegalEntityId, accountLegalEntityId);
        }
        
        internal static (long Ukprn, long AccountLegalEntityId) GetProviderPermissionValues(this IAuthorizationContext authorizationContext)
        {
            return (authorizationContext.Get<long>(AuthorizationContextKey.Ukprn),
                authorizationContext.Get<long>(AuthorizationContextKey.AccountLegalEntityId));
        }
    }
}