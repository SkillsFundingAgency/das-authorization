using SFA.DAS.Authorization.Context;

namespace SFA.DAS.Authorization.EmployerFeatures.Context
{
    public static class AuthorizationContextExtensions
    {
        public static void AddEmployerFeatureValues(this IAuthorizationContext authorizationContext, long accountId, string userEmail)
        {
            authorizationContext.Set(AuthorizationContextKey.AccountId, accountId);
            authorizationContext.Set(AuthorizationContextKey.UserEmail, userEmail);
        }
        
        internal static (long? AccountId, string UserEmail) GetEmployerFeatureValues(this IAuthorizationContext authorizationContext)
        {
            authorizationContext.TryGet<long?>(AuthorizationContextKey.AccountId, out var accountId);
            authorizationContext.TryGet<string>(AuthorizationContextKey.UserEmail, out var email);
            return (accountId, email);
        }
    }
}