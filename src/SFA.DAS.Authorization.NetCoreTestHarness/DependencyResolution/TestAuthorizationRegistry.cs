﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SFA.DAS.Authorization.CommitmentPermissions;
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
                FeatureToggles = new List<FeatureToggle> 
                {
                    new FeatureToggle
                    {
                        Feature = "ProviderRelationships",
                        IsEnabled = true
                    }
                }
            });
            
            For<EmployerFeaturesConfiguration>().Use(new EmployerFeaturesConfiguration
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
            });
            
            For<ProviderFeaturesConfiguration>().Use(new ProviderFeaturesConfiguration
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
            });
            
            For<IAuthorizationContextProvider>().Use<TestAuthorizationContextProvider>();
            For<IAuthorizationHandler>().Add<TestAuthorizationHandler>();

            For<IAuthorizationContextProvider>().Use<TestAuthorizationContextProvider>();
            For<IAuthorizationHandler>().Add<TestAuthorizationHandler>();
            For<ICommitmentsApiClient>().Use<CommitmentsApiClient>().Singleton();
            For<IMemoryCache>().Use<MemoryCache>().Singleton();
            var memoryCacheOptions = new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions());
            For<IOptions<MemoryCacheOptions>>().Use(memoryCacheOptions).Singleton();
        }
    }

    public class CommitmentsApiClient : ICommitmentsApiClient
    {
        public Task<bool> CanAccessCohort(CanAccessCohortRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}