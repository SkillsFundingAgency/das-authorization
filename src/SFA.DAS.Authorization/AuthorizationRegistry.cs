using StructureMap;

namespace SFA.DAS.Authorization
{
    public class AuthorizationRegistry : Registry
    {
        public AuthorizationRegistry()
        {
#if NET462
            IncludeRegistry<LoggerRegistry>();
            IncludeRegistry<MemoryCacheRegistry>();
            IncludeRegistry<OptionsRegistry>();
#endif
            For<IAuthorizationContext>().Use<AuthorizationContext>();
            For<IAuthorizationContextProvider>().Use<DefaultAuthorizationContextProvider>();
            For<IAuthorizationContextProvider>().DecorateAllWith<AuthorizationContextCache>();
            For<IAuthorizationService>().Use<AuthorizationService>();
        }
    }
}