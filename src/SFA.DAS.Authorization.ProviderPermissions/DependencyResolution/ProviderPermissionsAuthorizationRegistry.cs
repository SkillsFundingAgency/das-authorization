using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.ProviderPermissions.Handlers;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Authorization.ProviderPermissions.DependencyResolution
{
    public class ProviderPermissionsAuthorizationRegistry : Registry
    {
        public ProviderPermissionsAuthorizationRegistry()
        {
            IncludeRegistry<ProviderRelationshipsApiClientRegistry>();
            For<IAuthorizationHandler>().Add<AuthorizationHandler>().InterceptWith(new AuthorizationResultLoggerInterceptor());
        }
    }
}