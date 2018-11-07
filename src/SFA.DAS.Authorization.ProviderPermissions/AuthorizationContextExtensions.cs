namespace SFA.DAS.Authorization.ProviderPermissions
{
    public static class AuthorizationContextExtensions
    {
        public static void AddProviderPermissionsContext(this AuthorizationContext authorizationContext,
            long? ukprn, long? accountLegalEntityId)
        {
            authorizationContext.Add(AuthorizationContextKey.Ukprn, ukprn);
            authorizationContext.Add(AuthorizationContextKey.AccountLegalEntityId, accountLegalEntityId);
        }
    }
}