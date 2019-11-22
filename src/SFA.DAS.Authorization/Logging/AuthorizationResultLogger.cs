using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.Logging
{
    public class AuthorizationResultLogger : IAuthorizationHandler
    {
        public string Prefix => _authorizationHandler.Prefix;
        
        private readonly IAuthorizationHandler _authorizationHandler;
        private readonly ILogger<AuthorizationResultLogger> _logger;

        public AuthorizationResultLogger(IAuthorizationHandler authorizationHandler, ILogger<AuthorizationResultLogger> logger)
        {
            _authorizationHandler = authorizationHandler;
            _logger = logger;
        }

        public async Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = await _authorizationHandler.GetAuthorizationResult(options, authorizationContext).ConfigureAwait(false);
            authorizationContext.TryGet("AccountId", out string AccountId);
            authorizationContext.TryGet("HashedAccountId", out string HashedAccountId);
            var message = $"Finished running handler with prefix '{Prefix}' for options '{string.Join(", ", options)}' and context  AccountId: '{AccountId}' HashedAccountId: '{HashedAccountId}' with result '{authorizationResult}'";

            if (authorizationResult.IsAuthorized)
            {
                _logger.LogInformation(message);
            }
            else
            {
                _logger.LogWarning(message);
            }

            return authorizationResult;
        }
    }
}