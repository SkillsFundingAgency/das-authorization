using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Features.Configuration;
using SFA.DAS.Authorization.Features.Handlers;
using SFA.DAS.Authorization.Features.Models;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;

namespace SFA.DAS.Authorization.Features.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFeaturesAuthorization(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, AuthorizationHandler>()
                .AddSingleton<IFeatureTogglesService<FeatureToggle>, FeatureTogglesService<FeaturesConfiguration, FeatureToggle>>();
        }
    }
}