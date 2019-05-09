using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployerFeaturesAuthorization<T>(this IServiceCollection services) where T : class, IAuthorizationContextProvider
        {
            return services.AddAuthorization<T>()
                .AddScoped<IAuthorizationHandler, AuthorizationHandler>()
                .AddSingleton<IFeatureTogglesService, FeatureTogglesService>();
        }
    }
}