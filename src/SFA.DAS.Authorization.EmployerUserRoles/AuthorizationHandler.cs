using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerAccounts.Types.Models;

namespace SFA.DAS.Authorization.EmployerUserRoles
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => EmployerUserRole.Namespace;

        private readonly IEmployerAccountsApiClient _employerAccountsApiClient;
        private readonly ILogger _logger;

        public AuthorizationHandler(IEmployerAccountsApiClient employerAccountsApiClient, ILogger logger)
        {
            _employerAccountsApiClient = employerAccountsApiClient;
            _logger = logger;
        }

        public async Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Any())
            {
                options.EnsureNoAndOptions();

                var values = authorizationContext.GetEmployerUserRoleValues();
                var isUserInRole = false;
                
                if (options.Contains(EmployerUserRole.AnyOption))
                {
                    var isUserInAnyRoleRequest = new IsUserInAnyRoleRequest
                    {
                        AccountId = values.AccountId,
                        UserRef = values.UserRef
                    };

                    isUserInRole = await _employerAccountsApiClient.IsUserInAnyRole(isUserInAnyRoleRequest, CancellationToken.None).ConfigureAwait(false);
                }
                else
                {
                    var roles = options.SelectMany(o => o.Split(',')).Select(o => o.ToEnum<UserRole>()).ToHashSet();
                    
                    var isUserInRoleRequest = new IsUserInRoleRequest
                    {
                        AccountId = values.AccountId,
                        UserRef = values.UserRef,
                        Roles = roles
                    };
                    
                    isUserInRole = await _employerAccountsApiClient.IsUserInRole(isUserInRoleRequest, CancellationToken.None).ConfigureAwait(false);
                }

                if (!isUserInRole)
                {
                    authorizationResult.AddError(new EmployerUserRoleNotAuthorized());
                }
            }

            var logResult = authorizationResult.Errors.Any() ? $"results '{authorizationResult.Errors.Select(o => o.GetType()).ToCsvString()}'" : "successful result";
            _logger.LogInformation($"Finished running '{this.GetType().FullName}' for options '{options.ToCsvString()}' with {logResult}");

            return authorizationResult;
        }
    }
}