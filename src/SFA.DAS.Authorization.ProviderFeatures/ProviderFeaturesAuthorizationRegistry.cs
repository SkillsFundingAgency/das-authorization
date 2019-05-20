using StructureMap;

namespace SFA.DAS.Authorization.ProviderFeatures
{
    public class ProviderFeaturesAuthorizationRegistry : Registry
    {
        public ProviderFeaturesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            For<IFeatureTogglesService>().Use<FeatureTogglesService>().Singleton();
        }
    }
}