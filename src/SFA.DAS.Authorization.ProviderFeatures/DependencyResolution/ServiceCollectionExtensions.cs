using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.ProviderFeatures.Configuration;
using SFA.DAS.Authorization.ProviderFeatures.Handlers;
using SFA.DAS.Authorization.ProviderFeatures.Models;

namespace SFA.DAS.Authorization.ProviderFeatures.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderFeaturesAuthorization(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, AuthorizationHandler>()
                .AddSingleton<IFeatureTogglesService<ProviderFeatureToggle>, FeatureTogglesService<ProviderFeaturesConfiguration, ProviderFeatureToggle>>();
        }
    }
}