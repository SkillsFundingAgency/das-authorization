using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerRoles
{
    public class EmployerRolesAuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => EmployerRoles.Namespace;

        public Task PopulateAuthorizationResultAsync(AuthorizationResult authorizationResult,
            IEnumerable<string> employerRoles, IAuthorizationContext authorizationContext)
        {
            if (employerRoles.Any())
            {
                var accountId = authorizationContext.Get<int>(AuthorizationContextKeys.AccountId);
                var userRef = authorizationContext.Get<string>(AuthorizationContextKeys.UserRef);

                //authorizationResult.AddError(new EmployerRoleNotAuthorized());
            }

            return Task.CompletedTask;
        }
    }
}