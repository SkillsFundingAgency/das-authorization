using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.Cache
{
    /// <summary>
    ///     Decorator for <see cref="IAuthorizationHandler"/> that will cache the results to optimize expensive
    ///     authorization calls.
    /// </summary>
    public class AuthorizationHandlerCache : IAuthorizationHandler
    {

        private readonly IAuthorizationHandler _wrapped;
        private readonly IAuthorizationCacheService _cacheService;

        public AuthorizationHandlerCache(IAuthorizationCacheService authorizationCacheService, IAuthorizationHandler authorizationHandler)
        {
            _wrapped = authorizationHandler;
            _cacheService = authorizationCacheService;
        }

        public string Prefix => _wrapped.Prefix;

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            return _cacheService.GetOrAdd(_wrapped, options, authorizationContext);
        }
    }
}