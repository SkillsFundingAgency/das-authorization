using SFA.DAS.Authorization.Features.Configuration;
using SFA.DAS.Authorization.Features.Handlers;
using SFA.DAS.Authorization.Features.Models;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using StructureMap;

namespace SFA.DAS.Authorization.Features.DependencyResolution.StructureMap
{
    public class FeaturesAuthorizationRegistry : Registry
    {
        public FeaturesAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            For<IFeatureTogglesService<FeatureToggle>>().Use<FeatureTogglesService<FeaturesConfiguration, FeatureToggle>>().Singleton();
        }
    }
}