using StructureMap;

namespace SFA.DAS.Authorization
{
    public class AuthorizationRegistry : Registry
    {
        public AuthorizationRegistry()
        {
            For<IAuthorizationContext>().Use<AuthorizationContext>();
            For<IAuthorizationService>().Use<AuthorizationService>();
            For<ILoggerFactoryManager>().Use<LoggerFactoryManager>().Singleton();
        }
    }
}