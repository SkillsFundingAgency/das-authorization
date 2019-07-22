using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.Authorization.EmployerUserRoles.Handlers;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.EmployerAccounts.Api.Client;
using StructureMap;

namespace SFA.DAS.Authorization.EmployerUserRoles.DependencyResolution
{
    public class EmployerUserRolesAuthorizationRegistry : Registry
    {
        public EmployerUserRolesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>().InterceptWith(new AuthorizationResultLoggerInterceptor());
            IncludeRegistry<EmployerAccountsApiClientRegistry>();
        }
    }
}