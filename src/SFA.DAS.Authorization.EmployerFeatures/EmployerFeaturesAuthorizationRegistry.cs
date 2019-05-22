using SFA.DAS.Authorization.Features;
using StructureMap;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeaturesAuthorizationRegistry : Registry
    {
        public EmployerFeaturesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            For<IFeatureTogglesService<EmployerFeatureToggle>>().Use<FeatureTogglesService<EmployerFeaturesConfiguration, EmployerFeatureToggle>>().Singleton();
        }
    }
}