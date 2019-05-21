using StructureMap;

namespace SFA.DAS.Authorization.Features
{
    public class FeaturesAuthorizationRegistry : Registry
    {
        public FeaturesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            For<IFeatureTogglesService>().Use<FeatureTogglesService>().Singleton();
        }
    }
}