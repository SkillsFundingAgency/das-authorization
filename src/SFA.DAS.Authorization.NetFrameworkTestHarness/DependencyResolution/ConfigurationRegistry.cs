using System.Collections.Generic;
using SFA.DAS.Authorization.CommitmentPermissions.Configuration;
using SFA.DAS.Authorization.EmployerFeatures.Configuration;
using SFA.DAS.Authorization.EmployerFeatures.Models;
using SFA.DAS.Authorization.Features.Configuration;
using SFA.DAS.Authorization.Features.Models;
using SFA.DAS.Authorization.NetFrameworkTestHarness.Configuration;
using SFA.DAS.Authorization.NetFrameworkTestHarness.Models;
using SFA.DAS.Authorization.ProviderFeatures.Configuration;
using SFA.DAS.Authorization.ProviderFeatures.Models;
using SFA.DAS.AutoConfiguration;
using StructureMap;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<FeaturesConfiguration>().Use(new FeaturesConfiguration {
                FeatureToggles = new List<FeatureToggle> {
                    new FeatureToggle {
                        Feature = "ProviderRelationships",
                        IsEnabled = true
                    }
                }
            });

            For<EmployerFeaturesConfiguration>().Use(new EmployerFeaturesConfiguration {
                FeatureToggles = new List<EmployerFeatureToggle> {
                    new EmployerFeatureToggle {
                        Feature = "ProviderRelationships",
                        IsEnabled = true,
                        Whitelist = new List<EmployerFeatureToggleWhitelistItem> {
                            new EmployerFeatureToggleWhitelistItem {
                                AccountId = Account.Id,
                                UserEmails = new List<string> {User.Email}
                            }
                        }
                    }
                }
            });

            For<ProviderFeaturesConfiguration>().Use(new ProviderFeaturesConfiguration {
                FeatureToggles = new List<ProviderFeatureToggle> {
                    new ProviderFeatureToggle {
                        Feature = "ProviderRelationships",
                        IsEnabled = true,
                        Whitelist = new List<ProviderFeatureToggleWhitelistItem> {
                            new ProviderFeatureToggleWhitelistItem {
                                Ukprn = Provider.Ukprn,
                                UserEmails = new List<string> {User.Email}
                            }
                        }
                    }
                }
            });

            For<CommitmentPermissionsApiClientConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<ProviderCommitmentsConfiguration>("SFA.DAS.ProviderCommitments").CommitmentsClientApi).Singleton();
        }
    }
}