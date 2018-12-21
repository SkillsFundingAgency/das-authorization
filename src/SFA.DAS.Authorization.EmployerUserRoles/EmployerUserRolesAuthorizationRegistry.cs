using SFA.DAS.EmployerAccounts.Api.Client;
using StructureMap;

namespace SFA.DAS.Authorization.EmployerUserRoles
{
    public class EmployerUserRolesAuthorizationRegistry : Registry
    {
        public EmployerUserRolesAuthorizationRegistry()
        {
            IncludeRegistry<EmployerAccountsApiClientRegistry>();
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
        }
    }
}