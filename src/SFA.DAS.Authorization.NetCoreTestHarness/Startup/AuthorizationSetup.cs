using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution;
using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.Authorization.EmployerFeatures.DependencyResolution;
using SFA.DAS.Authorization.Features.DependencyResolution;
using SFA.DAS.Authorization.NetCoreTestHarness.Authorization;
using SFA.DAS.Authorization.ProviderFeatures.DependencyResolution;

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
                .AddAuthorizationHandler<TestAuthorizationHandler>();
        }
    }
}