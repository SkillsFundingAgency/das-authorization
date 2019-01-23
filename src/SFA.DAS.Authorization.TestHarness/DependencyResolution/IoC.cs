using SFA.DAS.Authorization;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.ProviderPermissions;
using SFA.DAS.Authorization.TestHarness.DependencyResolution;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Authorization.TestHarness.DependencyResolution 
{	
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
                {
                    c.AddRegistry<AutoConfigurationRegistry>();
                    c.AddRegistry<EmployerFeaturesAuthorizationRegistry>();
                    c.AddRegistry<EmployerUserRolesAuthorizationRegistry>();
                    c.AddRegistry<ProviderPermissionsAuthorizationRegistry>();
                    c.AddRegistry<TestAuthorizationRegistry>();
                    c.AddRegistry<DefaultRegistry>();
                }
            );
        }
    }
}