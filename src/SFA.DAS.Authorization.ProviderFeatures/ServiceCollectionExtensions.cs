using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Features;

namespace SFA.DAS.Authorization.ProviderFeatures
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