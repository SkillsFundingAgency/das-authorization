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
            services.AddScoped<IAuthorizationHandler, AuthorizationHandler>()
                .AddSingleton<IAuthorizationContextCacheKeyProvider, AuthorizationContextCacheKeyProvider>()
                .AddSingleton<IAuthorizationHandler>(serviceProvider =>
                {
                    var handler = serviceProvider.GetService<AuthorizationHandler>();
                    var cacheService = serviceProvider.GetService<IAuthorizationCacheService>();
                    return new AuthorizationHandlerCache(cacheService, handler);
                });

            services.AddSingleton<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactoryRegistryStub>();
            services.AddSingleton<ICommitmentPermissionsApiClient>(sp => sp.GetService<ICommitmentPermissionsApiClientFactory>().CreateClient());

            services.AddMemoryCache();
            return services;
        }
    }
}