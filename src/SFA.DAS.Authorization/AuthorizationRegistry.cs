using Microsoft.Extensions.Logging;
using StructureMap;

namespace SFA.DAS.Authorization
{
    public class AuthorizationRegistry : Registry
    {
        public AuthorizationRegistry()
        {
            For<IAuthorizationContext>().Use<AuthorizationContext>();
            For<IAuthorizationContextProvider>().Use<DefaultAuthorizationContextProvider>();
            For<IAuthorizationContextProvider>().DecorateAllWith<AuthorizationContextCache>();
            For<IAuthorizationService>().Use<AuthorizationService>();
            For<ILoggerFactoryManager>().Use(c => new LoggerFactoryManager(c.TryGetInstance<ILoggerFactory>())).Singleton();
            For(typeof(ILogger<>)).Use(typeof(Logger<>)).Ctor<ILoggerFactory>().Is(c => c.GetInstance<ILoggerFactoryManager>().GetLoggerFactory());
        }
    }
}