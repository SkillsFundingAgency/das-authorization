using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using SFA.DAS.Authorization.Cache;

namespace SFA.DAS.Authorization.CommitmentPermissions.Cache
{
    public class AuthorizationContextCacheKeyProvider : IAuthorizationContextCacheKeyProvider
    {
        public Type SupportsHandlerType { get; } = typeof(AuthorizationHandler);

        public object GetAuthorizationKey(IReadOnlyCollection<string> options, IAuthorizationContext context)
        {
            context.TryGet(AuthorizationContextKey.Party, out Party partyType);
            context.TryGet(AuthorizationContextKey.PartyId, out long partyId);
            context.TryGet(AuthorizationContextKey.CohortId, out long cohortId);

            return new CommitmentAuthorizationHashKey(partyType, partyId, cohortId, options);   
        }

        public void ConfigureCacheItem(ICacheEntry cacheEntry, IAuthorizationContext authorizationContext, object key,
            AuthorizationResult resultToCache)
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(60);
        }
    }
}
