using Microsoft.Extensions.Logging;
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
            For<ILoggerFactory>().Use(c => c.GetInstance<ILoggerFactoryManager>().GetFactory(c.TryGetInstance<ILoggerFactory>)).Singleton();
            For<ILogger>().Use(c => c.GetInstance<ILoggerFactory>().CreateLogger(c.ParentType));
        }
    }
}