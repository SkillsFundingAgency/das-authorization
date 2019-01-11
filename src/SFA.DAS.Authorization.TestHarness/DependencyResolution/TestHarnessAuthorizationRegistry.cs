using System.Collections.Generic;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.ProviderPermissions;
using SFA.DAS.Authorization.TestHarness.Handlers;
using StructureMap;

namespace SFA.DAS.Authorization.TestHarness.DependencyResolution
{
    public class TestHarnessAuthorizationRegistry : Registry
    {
        public const string DefaultUser = "test";
        public const long DefaultAccountId = 112;

        public TestHarnessAuthorizationRegistry()
        {
            //Setup test harness context, add values in TestHarnessAuthorizationContextProvider as needed.
            For<IAuthorizationContextProvider>().Use<TestHarnessAuthorizationContextProvider>();

            //Include the registry(ies) that you are testing
            IncludeRegistry<EmployerFeaturesAuthorizationRegistry>();
            IncludeRegistry<EmployerUserRolesAuthorizationRegistry>();
            IncludeRegistry<ProviderPermissionsAuthorizationRegistry>();

            //Allow Feature.ProviderRelationships for default user
            For<EmployerFeaturesConfiguration>().Use(new EmployerFeaturesConfiguration() {
                FeatureToggles = new List<FeatureToggle> {
                    new FeatureToggle(Feature.ProviderRelationships, true,
                        new List<FeatureToggleWhitelistItem>
                            {new FeatureToggleWhitelistItem(DefaultAccountId, new List<string> {DefaultUser})})
                }
            });

            //Optionally add the test handler for testing outside the context of a specific authorization implementation
            //For<IAuthorizationHandler>().Add<TestHarnessAuthorizationHandler>();
        }
    }
}