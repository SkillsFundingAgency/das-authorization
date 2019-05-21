using System.Collections.Generic;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.Features;
using SFA.DAS.Authorization.NetCoreTestHarness.Authorization;
using SFA.DAS.Authorization.NetCoreTestHarness.Models;
using SFA.DAS.Authorization.ProviderFeatures;
using StructureMap;

namespace SFA.DAS.Authorization.NetCoreTestHarness.DependencyResolution
{
    public class TestAuthorizationRegistry : Registry
    {
        public TestAuthorizationRegistry()
        {
            For<FeaturesConfiguration>().Use(new FeaturesConfiguration
            {
                FeatureToggles = new List<Features.FeatureToggle> 
                {
                    new Features.FeatureToggle("ProviderRelationships", true)
                }
            });
            
            For<EmployerFeaturesConfiguration>().Use(new EmployerFeaturesConfiguration
            {
                FeatureToggles = new List<EmployerFeatures.FeatureToggle> 
                {
                    new EmployerFeatures.FeatureToggle("ProviderRelationships", true, new List<EmployerFeatures.FeatureToggleWhitelistItem>
                    {
                        new EmployerFeatures.FeatureToggleWhitelistItem(Account.Id, new List<string>
                        {
                            User.Email
                        })
                    })
                }
            });
            
            For<ProviderFeaturesConfiguration>().Use(new ProviderFeaturesConfiguration
            {
                FeatureToggles = new List<ProviderFeatures.FeatureToggle> 
                {
                    new ProviderFeatures.FeatureToggle("ProviderRelationships", true, new List<ProviderFeatures.FeatureToggleWhitelistItem>
                    {
                        new ProviderFeatures.FeatureToggleWhitelistItem(Provider.Ukprn, new List<string>
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