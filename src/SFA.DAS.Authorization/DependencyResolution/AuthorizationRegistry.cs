using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Services;
using StructureMap;

namespace SFA.DAS.Authorization.DependencyResolution
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