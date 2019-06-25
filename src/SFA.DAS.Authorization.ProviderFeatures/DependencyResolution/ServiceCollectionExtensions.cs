using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.ProviderFeatures.Configuration;
using SFA.DAS.Authorization.ProviderFeatures.Handlers;
using SFA.DAS.Authorization.ProviderFeatures.Models;

namespace SFA.DAS.Authorization.ProviderFeatures.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderFeaturesAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorizationHandler<AuthorizationHandler>()
                .AddSingleton<IFeatureTogglesService<ProviderFeatureToggle>, FeatureTogglesService<ProviderFeaturesConfiguration, ProviderFeatureToggle>>();
        }
    }
}