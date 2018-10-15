using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerRoles
{
    public class EmployerRolesAuthorizationHandler : IAuthorizationHandler
    {
        private static readonly IEnumerable<string> EmployerRoles = typeof(EmployerRoles).GetFields().Select(f => f.GetRawConstantValue()).Cast<string>().ToList();

        public Task<AuthorizationResult> GetAuthorizationResultAsync(IEnumerable<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();
            var employerRoles = options.Intersect(EmployerRoles).ToList();

            if (employerRoles.Any())
            {
                var accountId = authorizationContext.Get<int>(AuthorizationContextKeys.AccountId);
                var userRef = authorizationContext.Get<string>(AuthorizationContextKeys.UserRef);

                //authorizationResult.AddError(new EmployerRoleNotAuthorized());
            }

            return Task.FromResult(authorizationResult);
        }
    }
}