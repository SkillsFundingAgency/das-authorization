using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.CommitmentPermissions.Configuration;
using SFA.DAS.Http;

namespace SFA.DAS.Authorization.CommitmentPermissions.Client
{
    public class  CommitmentPermissionsApiClientFactory : ICommitmentPermissionsApiClientFactory
    {
        private readonly CommitmentPermissionsApiClientConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public CommitmentPermissionsApiClientFactory(CommitmentPermissionsApiClientConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        public ICommitmentPermissionsApiClient CreateClient()
        {
            var httpClientFactory = new ManagedIdentityHttpClientFactory(_configuration, _loggerFactory);
            var httpClient = httpClientFactory.CreateHttpClient();
            var restHttpClient = new RestHttpClient(httpClient);
            var apiClient = new CommitmentPermissionsApiClient(restHttpClient);
            
            return apiClient;
        }
    }
}