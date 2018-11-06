using System;
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

        public void Authorize(params string[] options)
        {
            AuthorizeAsync(options).GetAwaiter().GetResult();
        }

        public async Task AuthorizeAsync(params string[] options)
        {
            var isAuthorized = await IsAuthorizedAsync(options).ConfigureAwait(false);

            if (!isAuthorized)
            {
                throw new UnauthorizedAccessException();
            }
        }

        public AuthorizationResult GetAuthorizationResult(params string[] options)
        {
            return GetAuthorizationResultAsync(options).GetAwaiter().GetResult();
        }

        public async Task<AuthorizationResult> GetAuthorizationResultAsync(params string[] options)
        {
            var authorizationContext = _authorizationContextProvider.GetAuthorizationContext();
            
            var authorizationResults = await Task.WhenAll(
                _handlers.Select(h => GetHandlerAuthorizationResultAsync(h, options, authorizationContext))).ConfigureAwait(false);
            return new AuthorizationResult(authorizationResults.SelectMany(r => r.Errors));
        }
        
        private Task<AuthorizationResult> GetHandlerAuthorizationResultAsync(IAuthorizationHandler handler, IEnumerable<string> allOptions, IAuthorizationContext authorizationContext)
        {
            return handler.GetAuthorizationResultAsync(GetHandlerOptions(handler, allOptions), authorizationContext);
        }
        
        private IEnumerable<string> GetHandlerOptions(IAuthorizationHandler handler, IEnumerable<string> allOptions)
        {
            var nakedOptionStartPos = handler.Namespace.Length + 1;
            return allOptions.Where(o => o.StartsWith(handler.Namespace))
                .Select(ho => ho.Substring(nakedOptionStartPos));
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