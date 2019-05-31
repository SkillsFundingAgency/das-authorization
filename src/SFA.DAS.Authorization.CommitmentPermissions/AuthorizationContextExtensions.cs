using System;

namespace SFA.DAS.Authorization.CommitmentPermissions
{
    public static class AuthorizationContextExtensions
    {
        public static void AddCommitmentPermissionValues(this IAuthorizationContext authorizationContext, long? cohortId, PartyType? partyType, string partyId)
        {
            authorizationContext.Set(AuthorizationContextKey.CohortId, cohortId);
            authorizationContext.Set(AuthorizationContextKey.PartyType, partyType);
            authorizationContext.Set(AuthorizationContextKey.PartyId, partyId);
        }
        
        internal static (long CohortId, PartyType PartyType, string PartyId) GetCommitmentPermissionValues(this IAuthorizationContext authorizationContext)
        {
            var cohortId = authorizationContext.Get<long?>(AuthorizationContextKey.CohortId);
            var partyType = authorizationContext.Get<PartyType?>(AuthorizationContextKey.PartyType);
            var partyId = authorizationContext.Get<string>(AuthorizationContextKey.PartyId);

            if (cohortId == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.CohortId}' cannot be null");
            }
            
            if (partyType == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.PartyType}' cannot be null");
            }

            if (partyId == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.PartyId}' cannot be null");
            }
            
            return (cohortId.Value, partyType.Value, partyId);
        }
    }
}