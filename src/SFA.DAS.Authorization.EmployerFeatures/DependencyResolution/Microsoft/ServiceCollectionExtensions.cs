﻿using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.DependencyResolution.Microsoft;
using SFA.DAS.Authorization.EmployerFeatures.Configuration;
using SFA.DAS.Authorization.EmployerFeatures.Models;
using SFA.DAS.Authorization.Features.Services;
using AuthorizationHandler = SFA.DAS.Authorization.EmployerFeatures.Handlers.AuthorizationHandler;

namespace SFA.DAS.Authorization.EmployerFeatures.DependencyResolution.Microsoft
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployerFeaturesAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorizationHandler<AuthorizationHandler>()
                .AddSingleton<IFeatureTogglesService<EmployerFeatureToggle>, FeatureTogglesService<EmployerFeaturesConfiguration, EmployerFeatureToggle>>();
        }
    }
}