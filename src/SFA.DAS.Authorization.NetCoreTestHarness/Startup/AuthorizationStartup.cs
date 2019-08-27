using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.Authorization.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.EmployerFeatures.DependencyResolution;
using SFA.DAS.Authorization.EmployerFeatures.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.Features.DependencyResolution;
using SFA.DAS.Authorization.Features.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.NetCoreTestHarness.Authorization;
using SFA.DAS.Authorization.ProviderFeatures.DependencyResolution;
using SFA.DAS.Authorization.ProviderFeatures.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.ProviderPermissions.DependencyResolution;
using SFA.DAS.Authorization.ProviderPermissions.DependencyResolution.Microsoft;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Startup
{
    public static class AuthorizationStartup
    {
        public static IServiceCollection AddDasAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorization<TestAuthorizationContextProvider>()
                .AddAuthorizationHandler<TestAuthorizationHandler>()
                .AddCommitmentPermissionsAuthorization()
                .AddEmployerFeaturesAuthorization()
                .AddFeaturesAuthorization()
                .AddProviderFeaturesAuthorization()
                .AddProviderPermissionsAuthorization();
        }
    }
}