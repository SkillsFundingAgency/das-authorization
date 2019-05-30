using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace SFA.DAS.Authorization.Cache
{
    /// <summary>
    ///     Provides handler specific details to the <see cref="IAuthorizationCacheService"/>.
    /// </summary>
    public interface IAuthorizationHandlerCacheConfig
    {
        /// <summary>
        ///     Which <see cref="IAuthorizationHandler"/> types can this instance support?
        /// </summary>
        Type[] SupportsHandlerTypes { get; }

        /// <summary>
        ///     Return the key that should be used for storing a specific authorization key.
        /// </summary>
        object GetAuthorizationKey(IReadOnlyCollection<string> options, IAuthorizationContext context);

        /// <summary>
        ///     Configure the specified cache entry
        /// </summary>
        void ConfigureCacheItem(ICacheEntry cacheEntry, IAuthorizationContext authorizationContext, object key, AuthorizationResult resultToCache);
    }
}