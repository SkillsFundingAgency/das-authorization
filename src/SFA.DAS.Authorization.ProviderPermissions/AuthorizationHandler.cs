using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRelationships.Api.Client;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.Authorization.ProviderPermissions
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => ProviderOperation.Prefix;
        
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;
        private readonly ILogger _logger;

        public AuthorizationHandler(IProviderRelationshipsApiClient providerRelationshipsApiClient,
            ILogger logger)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
            _logger = logger;
        }

        public async Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Any())
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
            
            _logger.LogInformation($"Finished running '{GetType().FullName}' for options '{string.Join(", ", options)}' and context '{authorizationContext.GetDescription()}' with result '{authorizationResult.GetDescription()}'");

            return authorizationResult;
        }
    }
}