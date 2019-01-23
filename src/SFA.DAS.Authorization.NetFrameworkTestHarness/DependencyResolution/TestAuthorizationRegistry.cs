using System.Collections.Generic;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.NetFrameworkTestHarness.Authorization;
using StructureMap;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.DependencyResolution
{
    public class TestAuthorizationRegistry : Registry
    {
        public TestAuthorizationRegistry()
        {
            For<EmployerFeaturesConfiguration>().Use(new EmployerFeaturesConfiguration
            {
                FeatureToggles = new List<FeatureToggle> 
                {
                    new FeatureToggle(Feature.ProviderRelationships, true, new List<FeatureToggleWhitelistItem>
                    {
                        new FeatureToggleWhitelistItem(AccountIds.Default, new List<string>
                        {
                            Usernames.Default
                        })
                    })
                }
            });
            
            For<IAuthorizationContextProvider>().Use<TestAuthorizationContextProvider>();
            For<IAuthorizationHandler>().Add<TestAuthorizationHandler>();
        }
    }
}