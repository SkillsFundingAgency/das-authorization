using SFA.DAS.Http.Configuration;

namespace SFA.DAS.Authorization.CommitmentPermissions.Configuration
{
    public class CommitmentPermissionsApiClientConfiguration : IManagedIdentityClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string IdentifierUri { get; set; }
    }
}