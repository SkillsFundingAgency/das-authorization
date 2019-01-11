using StructureMap;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeaturesAuthorizationRegistry : Registry
    {
        public EmployerFeaturesAuthorizationRegistry()
        {
            //IncludeRegistry<EmployerFeaturesApiClientRegistry>();
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            For<IFeatureTogglesService>().Use<FeatureTogglesService>().Singleton();
            For<ILoggerFactory>().Use(c => c.GetInstance<ILoggerFactoryManager>().GetFactory(c.TryGetInstance<ILoggerFactory>)).Singleton();
            For<ILogger>().Use(c => c.GetInstance<ILoggerFactory>().CreateLogger(c.ParentType));
        }
    }
}