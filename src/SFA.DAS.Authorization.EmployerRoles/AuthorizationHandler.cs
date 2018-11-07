using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerRoles
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => EmployerRole.Namespace;

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Any())
            {
                var values = authorizationContext.GetEmployerRoleValues();

                //authorizationResult.AddError(new EmployerRoleNotAuthorized());
            }

            return Task.FromResult(authorizationResult);
        }
    }
}