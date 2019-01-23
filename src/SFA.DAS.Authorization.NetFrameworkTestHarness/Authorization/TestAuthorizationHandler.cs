using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.Authorization
{
    public class TestAuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => TestOption.Namespace;
        
        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();
            
            if (options.Any())
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
            
            return Task.FromResult(authorizationResult);
        }
    }
}