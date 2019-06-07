using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace SFA.DAS.Authorization.Cache
{
    public class AuthorizationCacheService : IAuthorizationCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Dictionary<Type, IAuthorizationContextCacheKeyProvider> _cacheKeyProviders;

        public AuthorizationCacheService(IMemoryCache memoryCache, IAuthorizationContextCacheKeyProvider[] keyProvider)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

            if (keyProvider == null) throw new ArgumentNullException(nameof(keyProvider));

            _cacheKeyProviders = keyProvider
                .ToDictionary(cp => cp.SupportsHandlerType, kp => kp);
        }

        public Task<AuthorizationResult> GetOrAdd(
            IAuthorizationHandler handler, 
            IReadOnlyCollection<string> options,
            IAuthorizationContext authorizationContext)
        {
            var cacheDetails = GetCacheDetailsForAuthorizationHandlerType(handler, options, authorizationContext);
                    
            return _memoryCache.GetOrCreateAsync(cacheDetails.Key, ce => BuildItemToCache(ce, handler, authorizationContext, options, cacheDetails.Config, cacheDetails.Key));
        }

        private (IAuthorizationContextCacheKeyProvider Config, object Key) GetCacheDetailsForAuthorizationHandlerType(
            IAuthorizationHandler handler, 
            IReadOnlyCollection<string> options,
            IAuthorizationContext authorizationContext)
        {
            var handlerType = handler.GetType();

            if (!_cacheKeyProviders.TryGetValue(handlerType, out var configure))
            {
                throw new InvalidOperationException(
                    $"The authorization handler type {handlerType.Name} does not have an implementation of {nameof(IAuthorizationContextCacheKeyProvider)} that supports it.");
            }

            var key = configure.GetAuthorizationKey(options, authorizationContext);

            return (Config: configure, Key: key);
        }

        private async Task<AuthorizationResult> BuildItemToCache(
            ICacheEntry cacheEntry, 
            IAuthorizationHandler handler, 
            IAuthorizationContext context, 
            IReadOnlyCollection<string> options, 
            IAuthorizationContextCacheKeyProvider keyProvider,
            object key)
        {
            var authorizationResult = await handler.GetAuthorizationResult(options, context);

            keyProvider.ConfigureCacheItem(cacheEntry, context, key, authorizationResult);

            return authorizationResult;
        }
    }
}