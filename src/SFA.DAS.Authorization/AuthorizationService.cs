using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationContextProvider _authorizationContextProvider;
        private readonly IEnumerable<IAuthorizationHandler> _handlers;

        public AuthorizationService(IAuthorizationContextProvider authorizationContextProvider, IEnumerable<IAuthorizationHandler> handlers)
        {
            _authorizationContextProvider = authorizationContextProvider;
            _handlers = handlers;
        }

        public AuthorizationResult GetAuthorizationResult(params string[] options)
        {
            return GetAuthorizationResultAsync(options).GetAwaiter().GetResult();
        }

        public async Task<AuthorizationResult> GetAuthorizationResultAsync(params string[] options)
        {
            var authorizationContext = _authorizationContextProvider.GetAuthorizationContext();
            var authorizationResults = await Task.WhenAll(_handlers.Select(h => h.GetAuthorizationResultAsync(options, authorizationContext))).ConfigureAwait(false);
            var authorizationResult = new AuthorizationResult(authorizationResults.SelectMany(r => r.Errors));

            return authorizationResult;
        }

        public bool IsAuthorized(params string[] options)
        {
            return IsAuthorizedAsync(options).GetAwaiter().GetResult();
        }

        public async Task<bool> IsAuthorizedAsync(params string[] options)
        {
            var authorizationResult = await GetAuthorizationResultAsync(options).ConfigureAwait(false);

            return authorizationResult.IsAuthorized;
        }
    }
}