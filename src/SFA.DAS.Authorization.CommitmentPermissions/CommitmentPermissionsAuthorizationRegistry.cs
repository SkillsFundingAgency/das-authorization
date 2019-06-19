using SFA.DAS.Authorization.Cache;
using SFA.DAS.Authorization.CommitmentPermissions.Cache;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using StructureMap;

namespace SFA.DAS.Authorization.CommitmentPermissions
{
    public class CommitmentPermissionsAuthorizationRegistry : Registry
    {
        public CommitmentPermissionsAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationResultCache>().Ctor<IAuthorizationHandler>().Is<AuthorizationHandler>();
            For<IAuthorizationResultCacheConfigurationProvider>().Add<AuthorizationResultCacheConfigurationProvider>().Singleton();
            For<ICommitmentPermissionsApiClient>().Use(c => c.GetInstance<ICommitmentPermissionsApiClientFactory>().CreateClient()).Singleton();
            For<ICommitmentPermissionsApiClientFactory>().Use<CommitmentPermissionsApiClientFactory>();
        }
    }
}