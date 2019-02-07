using System.Collections.Generic;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.NetCoreTestHarness.Authorization;
using SFA.DAS.Authorization.NetCoreTestHarness.Models;
using StructureMap;

namespace SFA.DAS.Authorization.NetCoreTestHarness.DependencyResolution
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
                        new FeatureToggleWhitelistItem(Account.Id, new List<string>
                        {
                            User.Email
                        })
                    })
                }
            });
            
            For<IAuthorizationContextProvider>().Use<TestAuthorizationContextProvider>();
            For<IAuthorizationHandler>().Add<TestAuthorizationHandler>();
        }
    }
}