using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Authorization.ProviderPermissions.Types;
using SFA.DAS.Http;

namespace SFA.DAS.Authorization.ProviderPermissions.Client
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient
    {

        private readonly IRestHttpClient _restClient;

        public ProviderRelationshipsApiClient(IRestHttpClient restClient)
        {
            _restClient = restClient;
        }

        public Task<bool> HasPermission(HasPermissionRequest request, CancellationToken cancellationToken = default)
        {
            return _restClient.Get<bool>("permissions/has", request, cancellationToken);
        }
    }
}