using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SFA.DAS.Authorization.CommitmentPermissions;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.Features;
using SFA.DAS.Authorization.NetFrameworkTestHarness.Authorization;
using SFA.DAS.Authorization.NetFrameworkTestHarness.Models;
using SFA.DAS.Authorization.ProviderFeatures;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.Http.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using StructureMap;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.DependencyResolution
{
    public class TestAuthorizationRegistry : Registry
    {
        public TestAuthorizationRegistry()
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

            For<IAuthorizationContextProvider>().Use<TestAuthorizationContextProvider>();
            For<IAuthorizationHandler>().Add<TestAuthorizationHandler>();
            For<IMemoryCache>().Use<MemoryCache>().Singleton();
            var memoryCacheOptions = new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions());
            For<IOptions<MemoryCacheOptions>>().Use(memoryCacheOptions).Singleton();

            For<IAzureActiveDirectoryClientConfiguration>().Use(c =>
                c.GetInstance<IAutoConfigurationService>().Get<ConfigHolder>("SFA.DAS.ProviderCommitments")
                    .CommitmentsClientApi).Singleton();
            For<ICommitmentPermissionsApiClientFactory>().Use<CommitmentPermissionsApiClientFactory>();

        }

        internal interface IConfigHolder
        {
            AzureActiveDirectoryClientConfiguration CommitmentsClientApi { get; set; }
        }

        internal class ConfigHolder : IConfigHolder
        {
            public AzureActiveDirectoryClientConfiguration CommitmentsClientApi { get; set; }
        }
    }
}