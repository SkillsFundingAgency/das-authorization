using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.ProviderPermissions.Handlers;

namespace SFA.DAS.Authorization.ProviderPermissions.Context
{
    public class AuthorizationResultCacheConfigurationProvider : IAuthorizationResultCacheConfigurationProvider
    {
        public Type HandlerType { get; } = typeof(AuthorizationHandler);

        public object GetCacheKey(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            return new { options, authorizationContext };
        }

        public void ConfigureCacheEntry(ICacheEntry cacheEntry)
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(60);
        }
    }
}
