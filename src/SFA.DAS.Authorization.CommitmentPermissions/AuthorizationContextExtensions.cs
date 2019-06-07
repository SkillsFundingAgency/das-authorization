using System;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.Authorization.CommitmentPermissions
{
    public static class AuthorizationContextExtensions
    {
        public static void AddCommitmentPermissionValues(this IAuthorizationContext authorizationContext, long? cohortId, Party? party, long? partyId)
        {
            authorizationContext.Set(AuthorizationContextKey.CohortId, cohortId);
            authorizationContext.Set(AuthorizationContextKey.Party, party);
            authorizationContext.Set(AuthorizationContextKey.PartyId, partyId);
        }
        
        internal static (long CohortId, Party Party, long PartyId) GetCommitmentPermissionValues(this IAuthorizationContext authorizationContext)
        {
            var cohortId = authorizationContext.Get<long?>(AuthorizationContextKey.CohortId);
            var party = authorizationContext.Get<Party?>(AuthorizationContextKey.Party);
            var partyId = authorizationContext.Get<long?>(AuthorizationContextKey.PartyId);

            if (cohortId == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.CohortId}' cannot be null");
            }
            
            if (party == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.Party}' cannot be null");
            }

            if (partyId == null)
            {
                throw new InvalidOperationException($"Value for authorization context key '{AuthorizationContextKey.PartyId}' cannot be null");
            }
            
            return (cohortId.Value, party.Value, partyId.Value);
        }
    }
}