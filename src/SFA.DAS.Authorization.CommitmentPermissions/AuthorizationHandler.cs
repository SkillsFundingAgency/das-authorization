using System;
using System.Collections.Generic;
using System.Linq;
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
                var operation = options.Select(o => o.ToEnum<Operation>()).Single();

                switch (operation)
                {
                    case Operation.AccessCohort:
                        var canAccessCohortRequest = new CanAccessCohortRequest
                        {
                            CohortId = values.CohortId,
                            Party = values.Party,
                            PartyId = values.PartyId
                        };

                        var canAccessCohort = await _commitmentsApiClient.CanAccessCohort(canAccessCohortRequest).ConfigureAwait(false);

                        if (!canAccessCohort)
                        {
                            authorizationResult.AddError(new CommitmentPermissionNotGranted());
                        }
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), operation, "The operation is not currently supported");
                }
            }
            
            _logger.LogAuthorizationResult(this, options, authorizationContext, authorizationResult);
            
            return authorizationResult;
        }
    }

    public enum Party
    {
        None = 0,
        Employer = 1,
        Provider = 2,
        TransferSender = 4
    }

    public class CanAccessCohortRequest
    {
        public long CohortId { get; set; }
        public Party Party { get; set; }
        public long PartyId { get; set; }
    }
    
    public interface ICommitmentsApiClient
    {
        Task<bool> CanAccessCohort(CanAccessCohortRequest request, CancellationToken cancellationToken = default);
    }
}