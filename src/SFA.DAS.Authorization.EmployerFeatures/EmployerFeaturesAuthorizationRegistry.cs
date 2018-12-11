using StructureMap;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeaturesAuthorizationRegistry : Registry
    {
        public EmployerFeaturesAuthorizationRegistry()
        {
            
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            For<IFeatureTogglesService>().Use<FeatureTogglesService>().Singleton();
            For<ILoggerFactory>().Use(c => c.TryGetInstance<ILoggerFactory>() ?? new LoggerFactory().AddNLog()).Singleton();
            For<ILogger>().Use(c => c.GetInstance<ILoggerFactory>().CreateLogger(c.ParentType));
        }
    }
}