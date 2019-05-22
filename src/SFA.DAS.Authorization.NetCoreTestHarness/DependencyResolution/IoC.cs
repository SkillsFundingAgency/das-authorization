using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.Features;
using SFA.DAS.Authorization.ProviderFeatures;
using SFA.DAS.Authorization.ProviderPermissions;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Authorization.NetCoreTestHarness.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<AuthorizationRegistry>();
            registry.IncludeRegistry<AutoConfigurationRegistry>();
            registry.IncludeRegistry<EmployerFeaturesAuthorizationRegistry>();
            registry.IncludeRegistry<EmployerUserRolesAuthorizationRegistry>();
            registry.IncludeRegistry<FeaturesAuthorizationRegistry>();
            registry.IncludeRegistry<ProviderFeaturesAuthorizationRegistry>();
            registry.IncludeRegistry<ProviderPermissionsAuthorizationRegistry>();
            registry.IncludeRegistry<TestAuthorizationRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}