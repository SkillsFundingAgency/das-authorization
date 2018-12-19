using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SFA.DAS.Authorization.TestHarness.Handlers
{
    public class TestHarnessAuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => "TestHarnessAuthorizationHandlerNamespace";
        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            Debug.WriteLine($"Options: {ConstructOptionsStringForDebugOutput(options)}");
            switch (options.Single())
            {
                case TestOptions.DasAuthorizeTrue: return Task.FromResult(new AuthorizationResult());
                case TestOptions.DasAuthorizeFalse: return Task.FromResult(new AuthorizationResult(new TestAuthorizationError("Error")));
                case TestOptions.DasAuthorizeFalseWithErrors: return Task.FromResult(new AuthorizationResult(new [] { new TestAuthorizationError("Error1"), new TestAuthorizationError("Error2") }));
                default: return Task.FromResult(new AuthorizationResult(new TestAuthorizationError("DefaultError")));
            }

        }

        private string ConstructOptionsStringForDebugOutput(IReadOnlyCollection<string> options)
        {
            var output = string.Empty;
            var first = true;
            foreach (var option in options)
            {
                if (!first) { output += ","; }
                first = false;

                output += option;
            }

            return output;
        }
    }
}