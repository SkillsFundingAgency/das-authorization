using SFA.DAS.Authorization.Features;
using StructureMap;

namespace SFA.DAS.Authorization.ProviderFeatures
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