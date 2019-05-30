using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Cache;
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

            // For web apps also need services.AddMemoryCache(); in start up
            For<IAuthorizationCacheService>().Use<AuthorizationCacheService>().Singleton();
        }
    }
}