using SFA.DAS.Http;

namespace SFA.DAS.Authorization.CommitmentPermissions.Client
{
    public class  CommitmentPermissionsApiClientFactory : ICommitmentPermissionsApiClientFactory
    {
        private readonly CommitmentPermissionsApiClientConfiguration _configuration;

        public CommitmentPermissionsApiClientFactory(CommitmentPermissionsApiClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ICommitmentPermissionsApiClient CreateClient()
        {
            var httpClientFactory = new AzureActiveDirectoryHttpClientFactory(_configuration);
            var httpClient = httpClientFactory.CreateHttpClient();
            var restHttpClient = new RestHttpClient(httpClient);
            var apiClient = new CommitmentPermissionsApiClient(restHttpClient);
            
            return apiClient;
        }
    }
}