using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;

namespace SFA.DAS.Authorization.CommitmentPermissions.Client
{
    public interface ICommitmentPermissionsApiClient
    {
        Task<bool> CanAccessCohort(CohortAccessRequest request, CancellationToken cancellationToken = default);
        Task<bool> CanAccessApprenticeship(ApprenticeshipAccessRequest request, CancellationToken cancellationToken = default);
    }
}
