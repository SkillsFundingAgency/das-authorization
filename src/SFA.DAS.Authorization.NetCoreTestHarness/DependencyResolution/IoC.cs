using SFA.DAS.Authorization.EmployerUserRoles.DependencyResolution;
using SFA.DAS.Authorization.ProviderPermissions.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Authorization.NetCoreTestHarness.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<EmployerUserRolesAuthorizationRegistry>();
            registry.IncludeRegistry<ProviderPermissionsAuthorizationRegistry>();
        }
    }
}