using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.ProviderPermissions.Configuration;
using SFA.DAS.Http;

namespace SFA.DAS.Authorization.ProviderPermissions.Client
{
    public class ProviderRelationshipsApiClientFactory : IProviderRelationshipsApiClientFactory
    {
        private readonly ProviderRelationshipsApiClientConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public ProviderRelationshipsApiClientFactory(ProviderRelationshipsApiClientConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        public IProviderRelationshipsApiClient CreateClient()
        {
            var httpClientFactory = new ManagedIdentityHttpClientFactory(_configuration, _loggerFactory);
            var httpClient = httpClientFactory.CreateHttpClient();
            var restHttpClient = new RestHttpClient(httpClient);
            var apiClient = new ProviderRelationshipsApiClient(restHttpClient);

            return apiClient;
        }
    }
}
