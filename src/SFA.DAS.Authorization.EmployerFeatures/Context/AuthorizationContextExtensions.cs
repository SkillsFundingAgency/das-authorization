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
        
        internal static (long AccountId, string UserEmail) GetEmployerFeatureValues(this IAuthorizationContext authorizationContext)
        {
            return (authorizationContext.Get<long>(AuthorizationContextKey.AccountId),
                authorizationContext.Get<string>(AuthorizationContextKey.UserEmail));
        }
    }
}