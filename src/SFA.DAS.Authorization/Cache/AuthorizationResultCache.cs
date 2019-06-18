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
        private readonly Dictionary<Type, IAuthorizationContextCacheKeyProvider> _authorizationResultCacheKeyProviders;
        private readonly IMemoryCache _memoryCache;

        public AuthorizationResultCache(IAuthorizationHandler authorizationHandler, IEnumerable<IAuthorizationContextCacheKeyProvider> authorizationResultCacheKeyProviders, IMemoryCache memoryCache)
        {
            _authorizationHandler = authorizationHandler;
            _memoryCache = memoryCache;
            _authorizationResultCacheKeyProviders = authorizationResultCacheKeyProviders.ToDictionary(p => p.SupportsHandlerType);
        }

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationHandlerType = _authorizationHandler.GetType();
            var authorizationContextCacheKeyProvider = _authorizationResultCacheKeyProviders[authorizationHandlerType];
            var key = authorizationContextCacheKeyProvider.GetKey(options, authorizationContext);
            var authorizationResult = _memoryCache.GetOrCreateAsync(key, e => CreateCacheEntryValue(_authorizationHandler, options, authorizationContext, authorizationContextCacheKeyProvider, key, e));

            return authorizationResult;
        }

        private async Task<AuthorizationResult> CreateCacheEntryValue(IAuthorizationHandler authorizationHandler, IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext, IAuthorizationContextCacheKeyProvider authorizationResultCacheKeyProvider, object key, ICacheEntry cacheEntry)
        {
            var authorizationResult = await authorizationHandler.GetAuthorizationResult(options, authorizationContext).ConfigureAwait(false);

            authorizationResultCacheKeyProvider.ConfigureCacheEntry(authorizationContext, key, cacheEntry, authorizationResult);

            return authorizationResult;
        }
    }
}