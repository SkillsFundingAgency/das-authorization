using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Authorization.ProviderPermissions
{
    public class ProviderPermissionsAuthorizationRegistry : Registry
    {
        public ProviderPermissionsAuthorizationRegistry()
        {
            IncludeRegistry<ProviderRelationshipsApiClientRegistry>();
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            For<ILoggerFactory>().Use(c => c.GetInstance<ILoggerFactoryManager>().GetFactory(c.TryGetInstance<ILoggerFactory>)).Singleton();
            For<ILogger>().Use(c => c.GetInstance<ILoggerFactory>().CreateLogger(c.ParentType));
        }
    }
}