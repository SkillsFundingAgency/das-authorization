using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Authorization.CommitmentPermissions.Context;
using SFA.DAS.Authorization.CommitmentPermissions.Errors;
using SFA.DAS.Authorization.CommitmentPermissions.Models;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Extensions;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Logging;
using SFA.DAS.Authorization.Results;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;

namespace SFA.DAS.Authorization.CommitmentPermissions.Handlers
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => CommitmentOperation.Prefix;

        private readonly ICommitmentPermissionsApiClient _commitmentsApiClient;
        private readonly ILogger<AuthorizationHandler> _logger;

        public AuthorizationHandler(ICommitmentPermissionsApiClient commitmentsApiClient, ILogger<AuthorizationHandler> logger)
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
                        var canAccessCohortRequest = new CohortAccessRequest
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
}