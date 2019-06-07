using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Cache;
using SFA.DAS.Authorization.CommitmentPermissions.Cache;

namespace SFA.DAS.Authorization.CommitmentPermissions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommitmentPermissionsAuthorization(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, AuthorizationHandler>()
                .AddSingleton<ICommitmentsApiClient, CommitmentsApiClient>()
                .AddSingleton<IAuthorizationContextCacheKeyProvider, AuthorizationContextCacheKeyProvider>()
                .AddSingleton<IAuthorizationHandler>(serviceProvider =>
                {
                    var handler = serviceProvider.GetService<AuthorizationHandler>();
                    var cacheService = serviceProvider.GetService<IAuthorizationCacheService>();
                    return new AuthorizationHandlerCache(cacheService, handler);
                });

            services.AddMemoryCache();
            return services;
        }
    }
}