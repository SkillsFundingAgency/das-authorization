using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Authorization.Features
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFeaturesAuthorization(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, AuthorizationHandler>()
                .AddSingleton<IFeatureTogglesService, FeatureTogglesService>();
        }
    }
}