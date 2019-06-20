using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.ProviderFeatures.Configuration;
using SFA.DAS.Authorization.ProviderFeatures.Handlers;
using SFA.DAS.Authorization.ProviderFeatures.Models;
using StructureMap;

namespace SFA.DAS.Authorization.ProviderFeatures.DependencyResolution
{
    public class ProviderFeaturesAuthorizationRegistry : Registry
    {
        public ProviderFeaturesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            For<IFeatureTogglesService<ProviderFeatureToggle>>().Use<FeatureTogglesService<ProviderFeaturesConfiguration, ProviderFeatureToggle>>().Singleton();
        }
    }
}