using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace SFA.DAS.Authorization.Cache
{
    public class AuthorizationResultCache : IAuthorizationHandler
    {
        public string Prefix => _authorizationHandler.Prefix;
        
        private readonly IAuthorizationHandler _authorizationHandler;
        private readonly Dictionary<Type, IAuthorizationResultCachingStrategy> _authorizationResultCachingStrategies;
        private readonly IMemoryCache _memoryCache;

        public AuthorizationResultCache(IAuthorizationHandler authorizationHandler, IEnumerable<IAuthorizationResultCachingStrategy> authorizationResultCachingStrategies, IMemoryCache memoryCache)
        {
            _authorizationHandler = authorizationHandler;
            _authorizationResultCachingStrategies = authorizationResultCachingStrategies.ToDictionary(p => p.HandlerType);
            _memoryCache = memoryCache;
        }

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationHandlerType = _authorizationHandler.GetType();
            var authorizationResultCachingStrategy = _authorizationResultCachingStrategies[authorizationHandlerType];
            var key = authorizationResultCachingStrategy.GetCacheKey(options, authorizationContext);
            var authorizationResult = _memoryCache.GetOrCreateAsync(key, e => CreateCacheEntryValue(_authorizationHandler, options, authorizationContext, authorizationResultCachingStrategy, e));

            return authorizationResult;
        }

        private async Task<AuthorizationResult> CreateCacheEntryValue(IAuthorizationHandler authorizationHandler, IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext, IAuthorizationResultCachingStrategy authorizationResultCachingStrategy, ICacheEntry cacheEntry)
        {
            var authorizationResult = await authorizationHandler.GetAuthorizationResult(options, authorizationContext).ConfigureAwait(false);

            authorizationResultCachingStrategy.ConfigureCacheEntry(cacheEntry);

            return authorizationResult;
        }
    }
}