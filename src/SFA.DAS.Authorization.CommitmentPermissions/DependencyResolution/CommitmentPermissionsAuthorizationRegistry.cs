using Microsoft.Extensions.Caching.Memory;
using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.CommitmentPermissions.Caching;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Authorization.CommitmentPermissions.Handlers;
using SFA.DAS.Authorization.Handlers;
using StructureMap;

namespace SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution
{
    public class CommitmentPermissionsAuthorizationRegistry : Registry
    {
        public CommitmentPermissionsAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>().DecorateWith((c, h) => new AuthorizationResultCache(h, c.GetAllInstances<IAuthorizationResultCacheConfigurationProvider>(), c.GetInstance<IMemoryCache>()));
            For<IAuthorizationResultCacheConfigurationProvider>().Add<AuthorizationResultCacheConfigurationProvider>().Singleton();
            For<ICommitmentPermissionsApiClient>().Use(c => c.GetInstance<ICommitmentPermissionsApiClientFactory>().CreateClient()).Singleton();
            For<ICommitmentPermissionsApiClientFactory>().Use<CommitmentPermissionsApiClientFactory>().Transient();
        }
    }
}