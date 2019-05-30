using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Authorization.CommitmentPermissions
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => CommitmentOperation.Prefix;

        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILogger<AuthorizationHandler> _logger;

        public AuthorizationHandler(ICommitmentsApiClient commitmentsApiClient, ILogger<AuthorizationHandler> logger)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _logger = logger;
        }

        public async Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Count > 0)
            {
                options.EnsureNoAndOptions();
                options.EnsureNoOrOptions();
                
                var values = authorizationContext.GetCommitmentPermissionValues();
                
                var canAccessCohortRequest = new CanAccessCohortRequest
                {
                    CohortId = values.CohortId,
                    PartyType = values.PartyType,
                    PartyId = values.PartyId
                };

                var canAccessCohort = await _commitmentsApiClient.CanAccessCohort(canAccessCohortRequest).ConfigureAwait(false);

                if (!canAccessCohort)
                {
                    authorizationResult.AddError(new CommitmentPermissionNotGranted());
                }
            }
            
            _logger.LogAuthorizationResult(this, options, authorizationContext, authorizationResult);
            
            return authorizationResult;
        }
    }

    public class CanAccessCohortRequest
    {
        public long CohortId { get; set; }
        public string PartyType { get; set; }
        public long PartyId { get; set; }
    }
    
    public interface ICommitmentsApiClient
    {
        Task<bool> CanAccessCohort(CanAccessCohortRequest request, CancellationToken cancellationToken = default);
    }
}