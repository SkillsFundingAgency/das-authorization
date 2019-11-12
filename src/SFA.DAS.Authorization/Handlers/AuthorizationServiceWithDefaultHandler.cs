using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Results;
using SFA.DAS.Authorization.Services;

namespace SFA.DAS.Authorization.Handlers
{
    public class AuthorizationServiceWithDefaultHandler : IAuthorizationService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IDefaultAuthorizationHandler _defaultAuthorizationHandler;
        private readonly IAuthorizationContextProvider _authorizationContextProvider;

        public AuthorizationServiceWithDefaultHandler(
            IAuthorizationService authorizationService,
            IAuthorizationContextProvider authorizationContextProvider,
            IDefaultAuthorizationHandler defaultAuthorizationHandler)
        {
            _defaultAuthorizationHandler = defaultAuthorizationHandler;
            _authorizationService = authorizationService;
            _authorizationContextProvider = authorizationContextProvider;
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

            var handlerResult = await _defaultAuthorizationHandler.GetAuthorizationResultDefault(options, _authorizationContextProvider.GetAuthorizationContext());
            authorizationResult.Errors.ToList().AddRange(handlerResult.Errors);           

            

            foreach (var err in handlerResult.Errors)
            {
                authorizationResult.AddError(err);
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
