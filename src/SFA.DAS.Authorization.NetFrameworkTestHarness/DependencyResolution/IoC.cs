using SFA.DAS.Authorization.CommitmentPermissions;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution;
using SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerFeatures.DependencyResolution;
using SFA.DAS.Authorization.EmployerFeatures.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.EmployerUserRoles.DependencyResolution;
using SFA.DAS.Authorization.EmployerUserRoles.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.Features;
using SFA.DAS.Authorization.Features.DependencyResolution;
using SFA.DAS.Authorization.Features.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.ProviderFeatures;
using SFA.DAS.Authorization.ProviderFeatures.DependencyResolution;
using SFA.DAS.Authorization.ProviderFeatures.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.ProviderPermissions;
using SFA.DAS.Authorization.ProviderPermissions.DependencyResolution;
using SFA.DAS.Authorization.ProviderPermissions.DependencyResolution.StructureMap;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.DependencyResolution 
{	
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<AuthorizationRegistry>();
                c.AddRegistry<CommitmentPermissionsAuthorizationRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<EmployerFeaturesAuthorizationRegistry>();
                c.AddRegistry<EmployerUserRolesAuthorizationRegistry>();
                c.AddRegistry<FeaturesAuthorizationRegistry>();
                c.AddRegistry<ProviderFeaturesAuthorizationRegistry>();
                c.AddRegistry<ProviderPermissionsAuthorizationRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}