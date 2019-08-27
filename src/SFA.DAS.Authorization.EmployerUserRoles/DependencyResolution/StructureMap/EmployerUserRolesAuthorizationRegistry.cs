using SFA.DAS.Authorization.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.EmployerUserRoles.Handlers;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.EmployerAccounts.Api.Client;
using StructureMap;

namespace SFA.DAS.Authorization.EmployerUserRoles.DependencyResolution.StructureMap
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