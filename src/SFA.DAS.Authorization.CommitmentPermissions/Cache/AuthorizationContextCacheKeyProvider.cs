using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using SFA.DAS.Authorization.Cache;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.Authorization.CommitmentPermissions.Cache
{
    public class AuthorizationContextCacheKeyProvider : IAuthorizationContextCacheKeyProvider
    {
        public Type SupportsHandlerType { get; } = typeof(AuthorizationHandler);

        public object GetKey(IReadOnlyCollection<string> options, IAuthorizationContext context)
        {
            context.TryGet(AuthorizationContextKey.Party, out Party partyType);
            context.TryGet(AuthorizationContextKey.PartyId, out long partyId);
            context.TryGet(AuthorizationContextKey.CohortId, out long cohortId);

            return new CommitmentAuthorizationHashKey(partyType, partyId, cohortId, options);   
        }

        public void ConfigureCacheEntry(IAuthorizationContext authorizationContext, object key,
            ICacheEntry cacheEntry,
            AuthorizationResult resultToCache)
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(60);
        }
    }
}
