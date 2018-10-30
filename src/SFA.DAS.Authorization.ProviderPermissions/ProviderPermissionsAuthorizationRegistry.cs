using SFA.DAS.ProviderRelationships.Api.Client;
using StructureMap;

namespace SFA.DAS.Authorization.ProviderPermissions
{
    public class ProviderPermissionsAuthorizationRegistry : Registry
    {
        public ProviderPermissionsAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<ProviderPermissionsAuthorizationHandler>();
            //does this belong here?
            For<IProviderRelationshipsApiClient>().Add<ProviderRelationshipsApiClient>();
        }
    }
}