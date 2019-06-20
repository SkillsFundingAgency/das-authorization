using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using SFA.DAS.Authorization.Context;

namespace SFA.DAS.Authorization.Caching
{
    public interface IAuthorizationResultCacheConfigurationProvider
    {
        Type HandlerType { get; }
        
        object GetCacheKey(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext);
        void ConfigureCacheEntry(ICacheEntry cacheEntry);
    }
}