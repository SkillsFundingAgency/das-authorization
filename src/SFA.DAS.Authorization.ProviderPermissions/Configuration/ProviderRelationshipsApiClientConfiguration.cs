using SFA.DAS.Http.Configuration;

namespace SFA.DAS.Authorization.ProviderPermissions.Configuration
{
    public class ProviderRelationshipsApiClientConfiguration : IManagedIdentityClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string IdentifierUri { get; set; }
    }
}