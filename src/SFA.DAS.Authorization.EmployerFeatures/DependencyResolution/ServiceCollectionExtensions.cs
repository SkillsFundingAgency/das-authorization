using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.EmployerFeatures.Configuration;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using AuthorizationHandler = SFA.DAS.Authorization.EmployerFeatures.Handlers.AuthorizationHandler;

namespace SFA.DAS.Authorization.EmployerFeatures.DependencyResolution
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