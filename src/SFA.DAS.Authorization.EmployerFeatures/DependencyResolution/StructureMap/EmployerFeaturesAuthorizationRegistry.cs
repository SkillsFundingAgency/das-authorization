using SFA.DAS.Authorization.EmployerFeatures.Configuration;
using SFA.DAS.Authorization.EmployerFeatures.Models;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using StructureMap;
using AuthorizationHandler = SFA.DAS.Authorization.EmployerFeatures.Handlers.AuthorizationHandler;

namespace SFA.DAS.Authorization.EmployerFeatures.DependencyResolution.StructureMap
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