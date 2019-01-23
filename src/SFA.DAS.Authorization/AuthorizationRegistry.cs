using Microsoft.Extensions.Logging;
using StructureMap;

namespace SFA.DAS.Authorization
{
    public class AuthorizationRegistry : Registry
    {
        public AuthorizationRegistry()
        {
            For<IAuthorizationContext>().Use<AuthorizationContext>();
            For<IAuthorizationService>().Use<AuthorizationService>();
            For<ILogger>().Use(c => c.GetInstance<ILoggerFactory>().CreateLogger(c.ParentType));
            For<ILoggerFactory>().Use(c => c.GetInstance<ILoggerFactoryManager>().GetFactory(c.TryGetInstance<ILoggerFactory>)).Singleton();
            For<ILoggerFactoryManager>().Use<LoggerFactoryManager>().Singleton();
        }
    }
}