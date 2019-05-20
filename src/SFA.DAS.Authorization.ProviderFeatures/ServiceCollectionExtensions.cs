using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Authorization.ProviderFeatures
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderFeaturesAuthorization(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, AuthorizationHandler>()
                .AddSingleton<IFeatureTogglesService, FeatureTogglesService>();
        }
    }
}