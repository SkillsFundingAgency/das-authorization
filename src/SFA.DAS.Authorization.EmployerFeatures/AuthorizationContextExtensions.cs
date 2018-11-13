using System;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public static class AuthorizationContextExtensions
    {
        public static void AddEmployerFeatureValues(this IAuthorizationContext authorizationContext, long? accountId, string userEmail)
        {
            authorizationContext.Set(AuthorizationContextKey.AccountId, accountId);
            authorizationContext.Set(AuthorizationContextKey.UserEmail, userEmail);
        }
        
        internal static (long AccountId, string UserEmail) GetEmployerFeatureValues(this IAuthorizationContext authorizationContext)
        {
            var accountId = authorizationContext.Get<long?>(AuthorizationContextKey.AccountId);
            var userEmail = authorizationContext.Get<string>(AuthorizationContextKey.UserEmail);

            if (accountId == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.AccountId}' cannot be null");
            }

            if (string.IsNullOrWhiteSpace(userEmail))
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.UserEmail}' cannot be null or white space");
            }
            
            return (accountId.Value, userEmail);
        }
    }
}