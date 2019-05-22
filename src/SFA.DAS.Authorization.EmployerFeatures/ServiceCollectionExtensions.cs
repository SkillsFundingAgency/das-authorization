using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Features;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployerFeaturesAuthorization(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, AuthorizationHandler>()
                .AddSingleton<IFeatureTogglesService<EmployerFeatureToggle>, FeatureTogglesService<EmployerFeaturesConfiguration, EmployerFeatureToggle>>();
        }
    }
}