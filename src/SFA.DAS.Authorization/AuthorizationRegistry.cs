using Microsoft.Extensions.Logging;
using StructureMap;

namespace SFA.DAS.Authorization
{
    public class AuthorizationRegistry : Registry
    {
        public AuthorizationRegistry()
        {
            For<IAuthorizationContext>().Use<AuthorizationContext>();
            For<IAuthorizationContextProvider>().DecorateAllWith<AuthorizationContextCache>();
            For<IAuthorizationService>().Use<AuthorizationService>();
            For<ILogger>().Use(c => c.GetInstance<ILoggerFactoryManager>().GetLoggerFactory().CreateLogger(c.ParentType));
            For<ILoggerFactoryManager>().Use(c => new LoggerFactoryManager(c.TryGetInstance<ILoggerFactory>())).Singleton();
            For(typeof(ILogger<>)).Use(typeof(Logger<>));
        }
    }
}