using SFA.DAS.Http;
using SFA.DAS.Http.Configuration;

namespace SFA.DAS.Authorization.CommitmentPermissions.Client
{
    public class  CommitmentPermissionsApiClientFactory : ICommitmentPermissionsApiClientFactory
    {
        private readonly IAzureActiveDirectoryClientConfiguration _config;

        public CommitmentPermissionsApiClientFactory(IAzureActiveDirectoryClientConfiguration config)
        {
            _config = config;
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