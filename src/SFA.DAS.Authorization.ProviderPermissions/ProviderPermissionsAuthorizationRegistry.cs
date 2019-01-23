using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Authorization.ProviderPermissions
{
    public class ProviderPermissionsAuthorizationRegistry : Registry
    {
        public ProviderPermissionsAuthorizationRegistry()
        {
            IncludeRegistry<AuthorizationRegistry>();
            IncludeRegistry<ProviderRelationshipsApiClientRegistry>();
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
        }
    }
}