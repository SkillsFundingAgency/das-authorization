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
        private readonly Dictionary<Type, IAuthorizationHandlerCacheConfig> _cacheKeyProviders;

        public AuthorizationCacheService(IMemoryCache memoryCache, IAuthorizationHandlerCacheConfig[] config)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

            if (config == null) throw new ArgumentNullException(nameof(config));

            _cacheKeyProviders = config
                .SelectMany(cp => cp.SupportsHandlerTypes
                                    .Select(sht => new {
                                        KeyProvider = cp,
                                        HandlerType = sht
                                    }))
                .ToDictionary(cp => cp.HandlerType, kp => kp.KeyProvider);
        }

        public Task<AuthorizationResult> GetOrAdd(
            IAuthorizationHandler handler, 
            IReadOnlyCollection<string> options,
            IAuthorizationContext authorizationContext)
        {
            var cacheDetails = GetCacheDetailsForAuthorizationHandlerType(handler, options, authorizationContext);
                    
            return _memoryCache.GetOrCreateAsync(cacheDetails.Key, ce => BuildItemToCache(ce, handler, authorizationContext, options, cacheDetails.Config, cacheDetails.Key));
        }

        private (IAuthorizationHandlerCacheConfig Config, object Key) GetCacheDetailsForAuthorizationHandlerType(
            IAuthorizationHandler handler, 
            IReadOnlyCollection<string> options,
            IAuthorizationContext authorizationContext)
        {
            var handlerType = handler.GetType();

            if (!_cacheKeyProviders.TryGetValue(handlerType, out var configure))
            {
                throw new InvalidOperationException(
                    $"The authorization handler type {handlerType.Name} does not have an implementation of {nameof(IAuthorizationHandlerCacheConfig)} that supports it.");
            }

            var key = configure.GetAuthorizationKey(options, authorizationContext);

            return (Config: configure, Key: key);
        }

        private async Task<AuthorizationResult> BuildItemToCache(
            ICacheEntry cacheEntry, 
            IAuthorizationHandler handler, 
            IAuthorizationContext context, 
            IReadOnlyCollection<string> options, 
            IAuthorizationHandlerCacheConfig config,
            object key)
        {
            var authorizationResult = await handler.GetAuthorizationResult(options, context);

            config.ConfigureCacheItem(cacheEntry, context, key, authorizationResult);

            return authorizationResult;
        }
    }
}