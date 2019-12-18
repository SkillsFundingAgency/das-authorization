using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Logging;
using SFA.DAS.Authorization.Services;
using StructureMap;

namespace SFA.DAS.Authorization.DependencyResolution.StructureMap
{
    public class AuthorizationRegistry : Registry
    {
        public AuthorizationRegistry()
        {
            For<IAuthorizationContext>().Use(c => c.GetInstance<IAuthorizationContextProvider>().GetAuthorizationContext());
            For<IAuthorizationContextProvider>().Use<DefaultAuthorizationContextProvider>();
            For<IAuthorizationContextProvider>().DecorateAllWith<AuthorizationContextCache>();
            For<IAuthorizationHandler>().DecorateAllWith<AuthorizationResultLogger>();

            var authorizationService = For<IAuthorizationService>().Use<AuthorizationService>();            
            For<IDefaultAuthorizationHandler>().Use<DefaultAuthorizationHandler>();
            For<IAuthorizationService>().Use<AuthorizationServiceWithDefaultHandler>().Ctor<IAuthorizationService>().Is(authorizationService);


            ForConcreteType<object>().Configure.Named(nameof(AuthorizationRegistry));
#if NET462
            IncludeRegistry<LoggerRegistry>();
            IncludeRegistry<MemoryCacheRegistry>();
            IncludeRegistry<OptionsRegistry>();
#endif
        }
    }
}