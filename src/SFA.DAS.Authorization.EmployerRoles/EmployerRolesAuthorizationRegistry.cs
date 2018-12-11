using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using StructureMap;

namespace SFA.DAS.Authorization.EmployerRoles
{
    public class EmployerRolesAuthorizationRegistry : Registry
    {
        public EmployerRolesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            For<ILoggerFactory>().Use(c => c.TryGetInstance<ILoggerFactory>() ?? new LoggerFactory().AddNLog()).Singleton();
            For<ILogger>().Use(c => c.GetInstance<ILoggerFactory>().CreateLogger(c.ParentType));
        }
    }
}