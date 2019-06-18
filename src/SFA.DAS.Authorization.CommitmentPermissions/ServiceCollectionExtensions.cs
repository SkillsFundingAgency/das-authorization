using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Cache;
using SFA.DAS.Authorization.CommitmentPermissions.Cache;
using SFA.DAS.Authorization.CommitmentPermissions.Client;

namespace SFA.DAS.Authorization.CommitmentPermissions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommitmentPermissionsAuthorization(this IServiceCollection services)
        {
            return services.AddMemoryCache()
                .AddScoped<AuthorizationHandler>()
                .AddScoped<IAuthorizationHandler>(p => new AuthorizationResultCache(p.GetService<AuthorizationHandler>(), p.GetServices<IAuthorizationResultCachingStrategy>(), p.GetService<IMemoryCache>()))
                .AddSingleton<IAuthorizationResultCachingStrategy, AuthorizationResultCachingStrategy>()
                .AddSingleton(p => p.GetService<ICommitmentPermissionsApiClientFactory>().CreateClient())
                .AddSingleton<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactoryRegistryStub>();
        }
    }
}