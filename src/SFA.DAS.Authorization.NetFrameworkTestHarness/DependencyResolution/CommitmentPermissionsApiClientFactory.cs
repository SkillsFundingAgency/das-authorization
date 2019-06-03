using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Http;
using SFA.DAS.Http.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.DependencyResolution
{
    public class CommitmentPermissionsApiClientFactory : ICommitmentPermissionsApiClientFactory
    {
        private readonly IAzureActiveDirectoryClientConfiguration _config;

        public CommitmentPermissionsApiClientFactory()
        {
            _config = new AzureActiveDirectoryClientConfiguration {
                Tenant = "citizenazuresfabisgov.onmicrosoft.com",
                IdentifierUri =  "https://citizenazuresfabisgov.onmicrosoft.com/das-commitments-api",
                ClientId = "7fbf9aad-e102-433a-be66-bbfc7017ea1f",
                ClientSecret = "8+qxuAcusE2j1nHCiy4/phZ0HRqt5u9iR75bnz2o2Qs=",
                ApiBaseUrl = "https://localhost:5011/"
            };
        }

        public ICommitmentPermissionsApiClient CreateClient()
        {
            var httpClientFactory = new AzureActiveDirectoryHttpClientFactory(_config);
            var httpClient = httpClientFactory.CreateHttpClient();
            var restHttpClient = new RestHttpClient(httpClient);
            return new CommitmentPermissionsApiClient(restHttpClient);
        }
    }
}
