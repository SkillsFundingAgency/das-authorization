using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Extensions;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.ProviderPermissions.Context;
using SFA.DAS.Authorization.ProviderPermissions.Errors;
using SFA.DAS.Authorization.ProviderPermissions.Options;
using SFA.DAS.Authorization.Results;
using SFA.DAS.ProviderRelationships.Api.Client;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.Authorization.ProviderPermissions.Handlers
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => ProviderOperation.Prefix;
        
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;

        public AuthorizationHandler(IProviderRelationshipsApiClient providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }

        public async Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Count > 0)
            {
                options.EnsureNoAndOptions();
                options.EnsureNoOrOptions();
                
                var values = authorizationContext.GetProviderPermissionValues();
                var operation = options.Select(o => o.ToEnum<Operation>()).Single();

                var hasPermissionRequest = new HasPermissionRequest
                {
                    Ukprn = values.Ukprn,
                    AccountLegalEntityId = values.AccountLegalEntityId,
                    Operation = operation
                };

                var hasPermission = await _providerRelationshipsApiClient.HasPermission(hasPermissionRequest).ConfigureAwait(false);
                
                if (!hasPermission)
                {
                    authorizationResult.AddError(new ProviderPermissionNotGranted());
                }
            }

            return authorizationResult;
        }
    }
}