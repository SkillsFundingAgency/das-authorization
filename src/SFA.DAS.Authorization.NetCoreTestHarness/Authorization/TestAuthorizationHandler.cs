using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Logging;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Authorization
{
    public class TestAuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => TestOption.Prefix;
        
        private readonly ILogger<TestAuthorizationHandler> _logger;

        public TestAuthorizationHandler(ILogger<TestAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();
            
            if (options.Count > 0)
            {
                switch (options.Single())
                {
                    case TestOption.AuthorizedOption:
                        break;
                    case TestOption.UnauthorizedSingleErrorOption:
                        authorizationResult.AddError(new TestAuthorizationError("Error"));
                        break;
                    case TestOption.UnauthorizedMultipleErrorsOptions:
                        authorizationResult.AddError(new TestAuthorizationError("Error1"));
                        authorizationResult.AddError(new TestAuthorizationError("Error2"));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(options));
                }
            }
            
            _logger.LogAuthorizationResult(this, options, authorizationContext, authorizationResult);
            
            return Task.FromResult(authorizationResult);
        }
    }
}