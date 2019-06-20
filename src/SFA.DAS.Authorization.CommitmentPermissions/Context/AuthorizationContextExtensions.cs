using SFA.DAS.Authorization.Context;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.Authorization.CommitmentPermissions.Context
{
    public static class AuthorizationContextExtensions
    {
        public static void AddCommitmentPermissionValues(this IAuthorizationContext authorizationContext, long cohortId, Party party, long partyId)
        {
            authorizationContext.Set(AuthorizationContextKey.CohortId, cohortId);
            authorizationContext.Set(AuthorizationContextKey.Party, party);
            authorizationContext.Set(AuthorizationContextKey.PartyId, partyId);
        }
        
        internal static (long CohortId, Party Party, long PartyId) GetCommitmentPermissionValues(this IAuthorizationContext authorizationContext)
        {
            return (authorizationContext.Get<long>(AuthorizationContextKey.CohortId),
                authorizationContext.Get<Party>(AuthorizationContextKey.Party),
                authorizationContext.Get<long>(AuthorizationContextKey.PartyId));
        }
        
        internal static bool TryGetCommitmentPermissionValues(this IAuthorizationContext authorizationContext, out long cohortId, out Party party, out long partyId)
        {
            return authorizationContext.TryGet(AuthorizationContextKey.CohortId, out cohortId) &
                authorizationContext.TryGet(AuthorizationContextKey.Party, out party) &
                authorizationContext.TryGet(AuthorizationContextKey.PartyId, out partyId);
        }
    }
}