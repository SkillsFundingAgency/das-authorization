using Microsoft.Extensions.Caching.Memory;
using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.ProviderPermissions.Client;
using SFA.DAS.Authorization.ProviderPermissions.Handlers;
using StructureMap;

namespace SFA.DAS.Authorization.ProviderPermissions.DependencyResolution.StructureMap
{
    public class ProviderPermissionsAuthorizationRegistry : Registry
    {
        public ProviderPermissionsAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>().DecorateWith((c, h) =>
                new AuthorizationResultCache(h, c.GetAllInstances<IAuthorizationResultCacheConfigurationProvider>(),
                    c.GetInstance<IMemoryCache>()));
            For<IProviderRelationshipsApiClient>()
                .Use(c => c.GetInstance<IProviderRelationshipsApiClientFactory>().CreateClient()).Singleton();
            For<IProviderRelationshipsApiClientFactory>().Use<ProviderRelationshipsApiClientFactory>().Transient();
        }
    }
}