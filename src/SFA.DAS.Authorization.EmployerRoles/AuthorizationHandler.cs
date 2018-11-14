using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerRoles
{
    //todo remove this and remap onto the api client stub when it is available
    public interface IEmployerRolesApiClientDummy
    {
        Task<bool> HasRole(RoleRequest roleRequest); //what to call this - has role hides the fact we might be passing multiple roles and accepting either or - HasAnyRole? AnyRoleRequest?
    }

    //todo remove this and remap onto the api client stub when it is available
    public class RoleRequest
    {
        public Guid UserRef { get; set; }
        public long EmployerAccountId { get; set; }
        public List<Role> Roles { get; set; }
    }

    //todo remove this and remap onto the api client stub when it is available
    public enum Role
    {
        Owner = 1,
        Transactor = 2,
        Viewer = 3
    }

    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => EmployerRole.Namespace;

        private readonly IEmployerRolesApiClientDummy _employerRolesApiClient;

        public AuthorizationHandler(IEmployerRolesApiClientDummy employerRolesApiClient)
        {
            _employerRolesApiClient = employerRolesApiClient;
        }

        public async Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Any())
            {
                options.EnsureNoAndOptions();

                var values = authorizationContext.GetEmployerRoleValues();
                var roles = options.SelectMany(o => o.Split(',')).Select(o => o.ToEnum<Role>()).ToList();

                var roleRequest = new RoleRequest
                {
                    UserRef = values.UserRef,
                    EmployerAccountId = values.AccountId,
                    Roles = roles
                };

                var hasRole = await _employerRolesApiClient.HasRole(roleRequest);

                if (!hasRole)
                {
                    authorizationResult.AddError(new EmployerRoleNotAuthorized());
                }
            }

            return authorizationResult;
        }
    }
}