using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace SFA.DAS.Authorization.Cache
{
    public interface IAuthorizationResultCacheConfigurationProvider
    {
        Type HandlerType { get; }
        
        object GetCacheKey(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext);
        void ConfigureCacheEntry(ICacheEntry cacheEntry);
    }
}