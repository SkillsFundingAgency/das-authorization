using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.Features;
using SFA.DAS.Authorization.NetCoreTestHarness.Models;
using SFA.DAS.Authorization.ProviderFeatures;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Http.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Startup
{
    public static class ConfigurationStartup
    {
        public static IServiceCollection AddDasConfiguration(this IServiceCollection services)
        {
            return services.AddSingleton(new FeaturesConfiguration
                {
                    FeatureToggles = new List<FeatureToggle> 
                    {
                        new FeatureToggle
                        {
                            Feature = "ProviderRelationships",
                            IsEnabled = true
                        }
                    }
                })
                .AddSingleton(new EmployerFeaturesConfiguration
                {
                    FeatureToggles = new List<EmployerFeatureToggle> 
                    {
                        new EmployerFeatureToggle
                        {
                            Feature = "ProviderRelationships",
                            IsEnabled = true,
                            Whitelist = new List<EmployerFeatureToggleWhitelistItem>
                            {
                                new EmployerFeatureToggleWhitelistItem
                                {
                                    AccountId = Account.Id,
                                    UserEmails = new List<string> { User.Email }
                                }
                            }
                        }
                    }
                })
                .AddSingleton(new ProviderFeaturesConfiguration
                {
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
                .AddSingleton<IAzureActiveDirectoryClientConfiguration>(p => p.GetService<IConfiguration>().GetSection("SFA.DAS.ProviderCommitments:CommitmentsClientApi").Get<AzureActiveDirectoryClientConfiguration>());
        }
        
        public static IWebHostBuilder ConfigureDasAppConfiguration(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(c => c
                .AddAzureTableStorage("SFA.DAS.ProviderCommitments"));
        }
    }
}