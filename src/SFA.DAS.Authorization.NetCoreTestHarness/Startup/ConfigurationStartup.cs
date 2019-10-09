using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.CommitmentPermissions.Configuration;
using SFA.DAS.Authorization.EmployerFeatures.Configuration;
using SFA.DAS.Authorization.EmployerFeatures.Models;
using SFA.DAS.Authorization.Features.Configuration;
using SFA.DAS.Authorization.Features.Models;
using SFA.DAS.Authorization.NetCoreTestHarness.Models;
using SFA.DAS.Authorization.ProviderFeatures.Configuration;
using SFA.DAS.Authorization.ProviderFeatures.Models;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Startup
{
    public static class ConfigurationStartup
    {
        public static IServiceCollection AddDasConfiguration(this IServiceCollection services)
        {
            return services.AddSingleton(new FeaturesConfiguration {
                FeatureToggles = new List<FeatureToggle>
                    {
                        new FeatureToggle
                        {
                            Feature = "ProviderRelationships",
                            IsEnabled = true
                        }
                    }
            })
                .AddSingleton(new EmployerFeaturesConfiguration {
                    FeatureToggles = new List<EmployerFeatureToggle>
                    {
                        new EmployerFeatureToggle
                        {
                            Feature = "ProviderRelationships",
                            IsEnabled = true,
                            EmailWhitelist = new List<string>(){ User.Email },
                            AccountWhitelist = new List<long>(){ Account.Id }
                        }
                    }
                })
                .AddSingleton(new ProviderFeaturesConfiguration {
                    FeatureToggles = new List<ProviderFeatureToggle>
                    {
                        new ProviderFeatureToggle
                        {
                            Feature = "ProviderRelationships",
                            IsEnabled = true,
                            Whitelist = new List<ProviderFeatureToggleWhitelistItem>
                            {
                                new ProviderFeatureToggleWhitelistItem
                                {
                                    Ukprn = Provider.Ukprn,
                                    UserEmails = new List<string> { User.Email }
                                }
                            }
                        }
                    }
                })
                .AddSingleton(p => p.GetService<IConfiguration>().GetSection("SFA.DAS.ProviderCommitments:CommitmentsClientApi").Get<CommitmentPermissionsApiClientConfiguration>());
        }

        public static IWebHostBuilder ConfigureDasAppConfiguration(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(c => c.AddAzureTableStorage("SFA.DAS.ProviderCommitments"));
        }
    }
}