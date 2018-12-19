﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerAccounts.Api.Client;
using SFA.DAS.EmployerAccounts.Types.Models;

namespace SFA.DAS.Authorization.EmployerRoles
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => EmployerUserRole.Namespace;

        private readonly IEmployerAccountsApiClient _employerAccountsApiClient;

        public AuthorizationHandler(IEmployerAccountsApiClient employerAccountsApiClient)
        {
            _employerAccountsApiClient = employerAccountsApiClient;
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

            return authorizationResult;
        }
    }
}