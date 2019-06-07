using SFA.DAS.Authorization.Cache;
using SFA.DAS.Authorization.CommitmentPermissions.Cache;
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
            For<ICommitmentsApiClient>().Use<CommitmentsApiClient>().Singleton();

            For<IAuthorizationContextCacheKeyProvider>()
                .Add<AuthorizationContextCacheKeyProvider>()
                .Singleton();
        }

        private IAuthorizationHandler BuildCache(IContext ctx, IAuthorizationHandler handler)
        {
            var authCacheService = ctx.GetInstance<IAuthorizationCacheService>();
            return new AuthorizationHandlerCache(authCacheService, handler);
        }
    }
}