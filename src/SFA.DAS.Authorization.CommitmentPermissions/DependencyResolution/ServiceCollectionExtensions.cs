﻿using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.CommitmentPermissions.Caching;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Authorization.CommitmentPermissions.Handlers;
using SFA.DAS.Authorization.DependencyResolution;

namespace SFA.DAS.Authorization.CommitmentPermissions.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommitmentPermissionsAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorizationHandler<AuthorizationHandler>(true)
                .AddSingleton<IAuthorizationResultCacheConfigurationProvider, AuthorizationResultCacheConfigurationProvider>()
                .AddSingleton(p => p.GetService<ICommitmentPermissionsApiClientFactory>().CreateClient())
                .AddScoped<ICommitmentPermissionsApiClientFactory, CommitmentPermissionsApiClientFactory>();
        }
    }
}