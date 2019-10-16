using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.CommitmentPermissions.Client;
using SFA.DAS.Authorization.CommitmentPermissions.Context;
using SFA.DAS.Authorization.CommitmentPermissions.Errors;
using SFA.DAS.Authorization.CommitmentPermissions.Models;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Extensions;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Options;
using SFA.DAS.Authorization.Results;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;

namespace SFA.DAS.Authorization.CommitmentPermissions.Handlers
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => CommitmentOperation.Prefix;

        private readonly ICommitmentPermissionsApiClient _commitmentsApiClient;

        public AuthorizationHandler(ICommitmentPermissionsApiClient commitmentsApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
        }

        public async Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Count > 0)
            {   
                var operations = options.Select(o => o.ToEnum<Operation>()).ToList();

                if (operations.Contains(Operation.IgnoreEmptyCohort))
                {
                    if (!authorizationContext.TryGet(AuthorizationContextKey.CohortId, out long _))
                    {
                        return authorizationResult;
                    }
                }

                var values = authorizationContext.GetCommitmentPermissionValues();

                foreach (var operation in operations)
                {
                    switch (operation)
                    {
                        case Operation.AccessCohort:
                            var canAccessCohortRequest = new CohortAccessRequest {
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
                    }
                }
                
            }
            
            return authorizationResult;
        }
    }
}