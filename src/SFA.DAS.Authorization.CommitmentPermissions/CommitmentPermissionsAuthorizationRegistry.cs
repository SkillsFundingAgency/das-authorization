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
            For<IAuthorizationHandler>().Add<AuthorizationHandler>()
                .DecorateWith((ctx, handler) => BuildCache(ctx, handler))
                .Singleton();

            For<IAuthorizationContextCacheKeyProvider>()
                .Add<AuthorizationContextCacheKeyProvider>()
                .Singleton();
            For<ICommitmentPermissionsApiClient>().Use(c => c.GetInstance<ICommitmentPermissionsApiClientFactory>().CreateClient()).Singleton();
            For<ICommitmentPermissionsApiClientFactory>().Use<CommitmentPermissionsApiClientFactoryRegistryStub>();
        }

        private IAuthorizationHandler BuildCache(IContext ctx, IAuthorizationHandler handler)
        {
            var authCacheService = ctx.GetInstance<IAuthorizationCacheService>();
            return new AuthorizationHandlerCache(authCacheService, handler);
        }
    }
}