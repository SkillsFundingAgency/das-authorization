using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.Authorization.ProviderPermissions
{
   #if false
    public static class AuthorizationContextExtensions
    {
        //how does this work when multiple handlers want the same thing. centralise?
        //if centralise, could just add these methods to authcontext!?
        //have an extensions class in each handler - think its ok to have >1 using with same names extensions : doesn't enforce same type for common values though
        public static void AddProviderId(this IAuthorizationContext authorizationContext, long? providerId)
        {
            authorizationContext.Add("ProviderId", providerId);
        }

        public static void AddAccountLegalEntityId(this IAuthorizationContext authorizationContext, long? accountLegalEntityId)
        {
            authorizationContext.Add("AccountLegalEntityId", accountLegalEntityId);
        }
    }
    
    // we could use fluent style along the lines of authContext.ForProviderPermissions().AddProviderId().AddAccountX()
    // and make Add on context internal, so you have to go through the extension methods
    
    // we're going to have 1 extension method per handler, e.g. AddProviderPermissionsContext(long ukprn, long accountLegalEntityId)
    #endif
    
    public class ProviderPermissionsAuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => ProviderOperation.Namespace;
        
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;
        
        public ProviderPermissionsAuthorizationHandler(IProviderRelationshipsApiClient providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }

        public async Task PopulateAuthorizationResultAsync(AuthorizationResult authorizationResult, IEnumerable<string> operations, IAuthorizationContext authorizationContext)
        {
            var countOperations = operations.Count();

            if (countOperations == 0)
                return;
            
            // for mvs, we only support a single operation

            if (countOperations != 1)
                throw new NotImplementedException("Combining operations (to specify AND) is not currently supported");

            var operation = operations.First();
            if (operation.Contains(','))
                throw new NotImplementedException("Combining operations (to specify OR) by comma separating them is not currently supported");

            //how does consumer know what type to use? can we help? introduce type safety. type safe-extensions on ContextProvider?
            var accountLegalEntityId = authorizationContext.Get<long?>(AuthorizationContextKeys.AccountLegalEntityId);
            var providerId = authorizationContext.Get<long?>(AuthorizationContextKeys.ProviderId);
            
            //todo: add indexer to IAuthorizationContext?
        
            //UserRef for auditing/logging?

            var hasPermissionRequest = new PermissionRequest
            {
                Ukprn = providerId.Value,    //todo: check these are the same!
                EmployerAccountLegalEntityId = accountLegalEntityId.Value,
                Operation = ToOperation(operation)
            };

            // should HasPermissions return which permissions are/aren't granted, and we could pass that on, or just a not-granted
            // as is for mvs?
            if (!await _providerRelationshipsApiClient.HasPermission(hasPermissionRequest))
                authorizationResult.AddError(new ProviderPermissionNotGranted());
        }
        
        // todo: unit test to check that values in ProviderOperation match Operation enum names
        private Operation ToOperation(string operation)
        {
            return (Operation)Enum.Parse(typeof(Operation), operation);
        }
    }
}