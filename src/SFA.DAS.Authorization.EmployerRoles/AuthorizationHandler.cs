using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Authorization.EmployerRoles
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        private readonly ILogger _logger;

        public AuthorizationHandler(ILogger logger)
        {
            _logger = logger;
        }

        public string Namespace => EmployerRole.Namespace;

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Any())
            {
                var values = authorizationContext.GetEmployerRoleValues();

                //authorizationResult.AddError(new EmployerRoleNotAuthorized());
            }

            _logger.LogInformation($"Finished running '{this.GetType().FullName}' for options '{options.ToCsvString()}' with successful result");

            return Task.FromResult(authorizationResult);
        }
    }
}