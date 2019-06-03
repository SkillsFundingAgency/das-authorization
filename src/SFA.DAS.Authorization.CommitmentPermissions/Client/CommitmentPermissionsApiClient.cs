using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.Http;

namespace SFA.DAS.Authorization.CommitmentPermissions.Client
{
    public class CommitmentPermissionsApiClient : ICommitmentPermissionsApiClient
    {
        private readonly IRestHttpClient _restClient;

        public CommitmentPermissionsApiClient(IRestHttpClient restClient)
        {
            _restClient = restClient;
        }
        public Task<bool> CanAccessCohort(CohortAccessRequest request, CancellationToken cancellationToken = default)
        {
            return _restClient.Get<bool>("api/authorization/access-cohort", request, cancellationToken);
        }
    }
}
