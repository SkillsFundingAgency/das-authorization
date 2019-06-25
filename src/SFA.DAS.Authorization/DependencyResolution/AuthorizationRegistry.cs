using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Logging;
using SFA.DAS.Authorization.Services;
using StructureMap;

namespace SFA.DAS.Authorization.DependencyResolution
{
    public class AuthorizationRegistry : Registry
    {
        public AuthorizationRegistry()
        {
            For<IAuthorizationContext>().Use<AuthorizationContext>();
            For<IAuthorizationContextProvider>().Use<DefaultAuthorizationContextProvider>();
            For<IAuthorizationContextProvider>().DecorateAllWith<AuthorizationContextCache>();
            For<IAuthorizationHandler>().DecorateAllWith<AuthorizationResultLogger>();
            For<IAuthorizationService>().Use<AuthorizationService>();
            ForConcreteType<object>().Configure.Named(nameof(AuthorizationRegistry));
#if NET462
            IncludeRegistry<LoggerRegistry>();
            IncludeRegistry<MemoryCacheRegistry>();
            IncludeRegistry<OptionsRegistry>();
#endif
        }
    }
}