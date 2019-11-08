using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.Services
{
    public class AuthorizationServiceWithDefaultHandler : IAuthorizationService
    {
        private readonly IAuthorizationContextProvider _authorizationContextProvider;
        private readonly IDefaultAuthorizationHandler _defaultAuthorizationHandler;
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationServiceWithDefaultHandler(IAuthorizationContextProvider authorizationContextProvider,
            IDefaultAuthorizationHandler defaultAuthorizationHandler,
            IAuthorizationService authorizationService)
        {
            _authorizationContextProvider = authorizationContextProvider;
            _defaultAuthorizationHandler = defaultAuthorizationHandler;
            _authorizationService = authorizationService;
        }


        public void Authorize(params string[] options)
        {
            _authorizationService.Authorize(options);
        }

        public async Task AuthorizeAsync(params string[] options)
        {
            await _authorizationService.AuthorizeAsync(options);
        }

        public AuthorizationResult GetAuthorizationResult(params string[] options)
        {
            return _authorizationService.GetAuthorizationResult(options);
        }

        public async Task<AuthorizationResult> GetAuthorizationResultAsync(params string[] options)
        {
            var authorizationResult = await _authorizationService.GetAuthorizationResultAsync();

            var defaultAuthorizationResult = await _defaultAuthorizationHandler.GetAuthorizationResult(options, _authorizationContextProvider.GetAuthorizationContext());

            if (defaultAuthorizationResult != null)
            {
                authorizationResult.Errors.ToList().AddRange(defaultAuthorizationResult.Errors);              
            }            
            
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
