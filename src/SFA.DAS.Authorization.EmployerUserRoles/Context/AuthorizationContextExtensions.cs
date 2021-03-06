using System;
using SFA.DAS.Authorization.Context;

namespace SFA.DAS.Authorization.EmployerUserRoles.Context
{
    public static class AuthorizationContextExtensions
    {
        public static void AddEmployerUserRoleValues(this IAuthorizationContext authorizationContext, long accountId, Guid userRef)
        {
            authorizationContext.Set(AuthorizationContextKey.AccountId, accountId);
            authorizationContext.Set(AuthorizationContextKey.UserRef, userRef);
        }
        
        internal static (long AccountId, Guid UserRef) GetEmployerUserRoleValues(this IAuthorizationContext authorizationContext)
        {
            return (authorizationContext.Get<long>(AuthorizationContextKey.AccountId),
                authorizationContext.Get<Guid>(AuthorizationContextKey.UserRef));
        }
    }
}