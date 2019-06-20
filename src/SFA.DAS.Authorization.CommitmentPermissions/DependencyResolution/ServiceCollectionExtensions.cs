using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.CommitmentPermissions.Caching;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Authorization.CommitmentPermissions.Handlers;
using SFA.DAS.Authorization.Handlers;

namespace SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommitmentPermissionsAuthorization(this IServiceCollection services)
        {
            return services.AddMemoryCache()
                .AddScoped<AuthorizationHandler>()
                .AddScoped<IAuthorizationHandler>(p => new AuthorizationResultCache(p.GetService<AuthorizationHandler>(), p.GetServices<IAuthorizationResultCacheConfigurationProvider>(), p.GetService<IMemoryCache>()))
                .AddSingleton<IAuthorizationResultCacheConfigurationProvider, AuthorizationResultCacheConfigurationProvider>()
                .AddSingleton(p => p.GetService<ICommitmentPermissionsApiClientFactory>().CreateClient())
                .AddScoped<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactory>();
        }
    }
}