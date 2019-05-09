using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployerFeaturesAuthorization(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, AuthorizationHandler>()
                .AddSingleton<IFeatureTogglesService, FeatureTogglesService>();
        }
    }
}