using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Authorization.CommitmentPermissions
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => CommitmentOperation.Prefix;
        
        private readonly ILogger<AuthorizationHandler> _logger;

        public AuthorizationHandler(ILogger<AuthorizationHandler> logger)
        {
            _logger = logger;
        }

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Count > 0)
            {
                options.EnsureNoAndOptions();
                options.EnsureNoOrOptions();
                
                var values = authorizationContext.GetCommitmentPermissionValues();
            }
            
            _logger.LogAuthorizationResult(this, options, authorizationContext, authorizationResult);
            
            return Task.FromResult(authorizationResult);
        }
    }
}