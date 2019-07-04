using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.Authorization.Features.Configuration;
using SFA.DAS.Authorization.Features.Handlers;
using SFA.DAS.Authorization.Features.Models;
using SFA.DAS.Authorization.Features.Services;

namespace SFA.DAS.Authorization.Features.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFeaturesAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorizationHandler<AuthorizationHandler>()
                .AddSingleton<IFeatureTogglesService<FeatureToggle>, FeatureTogglesService<FeaturesConfiguration, FeatureToggle>>();
        }
    }
}