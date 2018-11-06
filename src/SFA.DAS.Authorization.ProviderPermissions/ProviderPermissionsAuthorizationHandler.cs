using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.Authorization.ProviderPermissions
{
    /// <remarks>
    /// For mvs, we only support a single operation
    /// </remarks>
    public class ProviderPermissionsAuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => ProviderOperation.Namespace;
        
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;
        
        public ProviderPermissionsAuthorizationHandler(IProviderRelationshipsApiClient providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }

        public async Task<AuthorizationResult> GetAuthorizationResultAsync(IEnumerable<string> operations, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();
            
            var countOperations = operations.Count();

            if (countOperations == 0)
                return authorizationResult;
            
            if (countOperations != 1)
                throw new NotImplementedException("Combining operations (to specify AND) is not currently supported");

            var operation = operations.First();
            if (operation.Contains(','))
                throw new NotImplementedException("Combining operations (to specify OR) by comma separating them is not currently supported");

            var accountLegalEntityId = authorizationContext.Get<long?>(AuthorizationContextKeys.AccountLegalEntityId);
            var ukprn = authorizationContext.Get<long?>(AuthorizationContextKeys.Ukprn);

            var hasPermissionRequest = new PermissionRequest
            {
                Ukprn = ukprn.Value,    //todo: check these are the same!
                EmployerAccountLegalEntityId = accountLegalEntityId.Value,
                Operation = ToOperation(operation)
            };

            // should HasPermissions return which permissions are/aren't granted, and we could pass that on, or just a not-granted
            // as is for mvs?
            if (!await _providerRelationshipsApiClient.HasPermission(hasPermissionRequest))
                authorizationResult.AddError(new ProviderPermissionNotGranted());

            return authorizationResult;
        }
        
        private Operation ToOperation(string operation)
        {
            return (Operation)Enum.Parse(typeof(Operation), operation);
        }
    }
}