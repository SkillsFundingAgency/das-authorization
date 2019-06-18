using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.CommitmentPermissions;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.Features;
using SFA.DAS.Authorization.NetCoreTestHarness.Authorization;
using SFA.DAS.Authorization.ProviderFeatures;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Startup
{
    public static class AuthorizationSetup
    {
        public static IServiceCollection AddDasAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorization<TestAuthorizationContextProvider>()
                .AddCommitmentPermissionsAuthorization()
                .AddEmployerFeaturesAuthorization()
                .AddFeaturesAuthorization()
                .AddProviderFeaturesAuthorization()
                .AddScoped<IAuthorizationHandler, TestAuthorizationHandler>()
                .AddSingleton<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactory>();
        }
    }
}