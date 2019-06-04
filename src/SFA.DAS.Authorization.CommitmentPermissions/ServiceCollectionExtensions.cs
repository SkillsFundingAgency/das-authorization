using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.CommitmentPermissions.Client;

namespace SFA.DAS.Authorization.CommitmentPermissions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommitmentPermissionsAuthorization(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();
            services.AddSingleton<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactoryRegistryStub>();
            services.AddSingleton<ICommitmentPermissionsApiClient>(sp=> sp.GetService<ICommitmentPermissionsApiClientFactory>().CreateClient());

            return services;
        }
    }
}