using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using SFA.DAS.Authorization.Cache;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.Authorization.CommitmentPermissions.Cache
{
    public class AuthorizationResultCachingStrategy : IAuthorizationResultCachingStrategy
    {
        public Type HandlerType => typeof(AuthorizationHandler);

        public object GetCacheKey(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            return new CacheKey(options, authorizationContext);
        }

        public void ConfigureCacheEntry(ICacheEntry cacheEntry)
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(60);
        }
    }
}