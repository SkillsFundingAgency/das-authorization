using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.ProviderPermissions.Handlers;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.StructureMap;
using StructureMap;

namespace SFA.DAS.Authorization.ProviderPermissions.DependencyResolution.StructureMap
{
    public class ProviderPermissionsAuthorizationRegistry : Registry
    {
        public ProviderPermissionsAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<AuthorizationHandler>();
            IncludeRegistry<ProviderRelationshipsApiClientRegistry>();
        }
    }
}