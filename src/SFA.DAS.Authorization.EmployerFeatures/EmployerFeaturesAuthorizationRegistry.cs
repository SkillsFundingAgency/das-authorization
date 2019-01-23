using StructureMap;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeaturesAuthorizationRegistry : Registry
    {
        public EmployerFeaturesAuthorizationRegistry()
        {
            IncludeRegistry<AuthorizationRegistry>();
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            For<IFeatureTogglesService>().Use<FeatureTogglesService>().Singleton();
        }
    }
}