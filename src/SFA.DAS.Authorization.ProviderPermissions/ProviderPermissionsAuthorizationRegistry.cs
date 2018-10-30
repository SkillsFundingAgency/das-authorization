using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Authorization.ProviderPermissions
{
    public class ProviderPermissionsAuthorizationRegistry : Registry
    {
        public ProviderPermissionsAuthorizationRegistry()
        {
            For<IAuthorizationHandler>().Add<ProviderPermissionsAuthorizationHandler>();
            IncludeRegistry<ProviderRelationshipsApiClientRegistry>();
        }
    }
}