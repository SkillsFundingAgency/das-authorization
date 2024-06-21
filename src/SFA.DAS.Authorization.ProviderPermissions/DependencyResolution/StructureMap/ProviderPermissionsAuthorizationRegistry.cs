using SFA.DAS.Authorization.Caching;
using SFA.DAS.Authorization.DependencyResolution.StructureMap;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.ProviderPermissions.Context;
using SFA.DAS.Authorization.ProviderPermissions.Handlers;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.StructureMap;
using StructureMap;

namespace SFA.DAS.Authorization.ProviderPermissions.DependencyResolution.StructureMap
{
    public class ProviderPermissionsAuthorizationRegistry : Registry
    {
        public ProviderPermissionsAuthorizationRegistry()
        {
            For<IAuthorizationResultCacheConfigurationProvider>().Add<AuthorizationResultCacheConfigurationProvider>();
            For<IAuthorizationHandler>().Add<AuthorizationHandler>().InterceptWith(new AuthorizationResultLoggerInterceptor());
            IncludeRegistry<ProviderRelationshipsApiClientRegistry>();
        }
    }
}