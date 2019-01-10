using System;

namespace SFA.DAS.Authorization.EmployerUserRoles
{
    public static class AuthorizationContextExtensions
    {
        public static void AddEmployerUserRoleValues(this IAuthorizationContext authorizationContext, long? accountId, Guid? userRef)
        {
            authorizationContext.Set(AuthorizationContextKey.AccountId, accountId);
            authorizationContext.Set(AuthorizationContextKey.UserRef, userRef);
        }
        
        internal static (long AccountId, Guid UserRef) GetEmployerUserRoleValues(this IAuthorizationContext authorizationContext)
        {
            var accountId = authorizationContext.Get<long?>(AuthorizationContextKey.AccountId);
            var userRef = authorizationContext.Get<Guid?>(AuthorizationContextKey.UserRef);

            if (accountId == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.AccountId}' cannot be null");
            }

            if (userRef == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.UserRef}' cannot be null");
            }
            
            return (accountId.Value, userRef.Value);
        }
    }
}