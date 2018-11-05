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
    #endif

    public class MissingRequiredContextException : Exception
    {
        public MissingRequiredContextException(params (string keyName, object value)[] keysWithValues)
            : base(CreateMessage(keysWithValues))
        {
        }

        private static string CreateMessage(IEnumerable<(string keyName, object value)> keysWithValues)
        {
            return $"Missing required context: {string.Join(", ", keysWithValues.Where(kv => kv.value == null).Select(kv => kv.keyName))}";
        }
    }
    
    public class ProviderPermissionsAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;
        
        public ProviderPermissionsAuthorizationHandler(IProviderRelationshipsApiClient providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }
        
        private static readonly IEnumerable<string> ProviderPermissions = typeof(ProviderOperation).GetFields().Select(f => f.GetRawConstantValue()).Cast<string>().ToList();

        public async Task<AuthorizationResult> GetAuthorizationResultAsync(IEnumerable<string> options, IAuthorizationContext authorizationContext)
        {
            //todo: move this boilerplate into AuthorizationService? could also remove namespace from strings and select using namespace also, so no reflection and intersection
            var authorizationResult = new AuthorizationResult();
            var providerPermissions = options.Intersect(ProviderPermissions);

            var countProviderPermissions = providerPermissions.Count();

            if (countProviderPermissions == 0)
                return authorizationResult;
            
            // for mvs, we only support a single operation

            if (countProviderPermissions != 1)
                throw new NotImplementedException("Combining operations (to specify AND) is not currently supported");

            var permissionString = providerPermissions.First();
            if (permissionString.Contains(','))
                throw new NotImplementedException("Combining operations (to specify OR) by comma separating them is not currently supported");

            //how does consumer know what type to use? can we help? introduce type safety. type safe-extensions on ContextProvider?
            var accountLegalEntityId = authorizationContext.Get<long?>(AuthorizationContextKeys.AccountLegalEntityId);
            var providerId = authorizationContext.Get<long?>(AuthorizationContextKeys.ProviderId);

            // already handled by Get...
//            if (accountLegalEntityId == null || providerId == null)
//                throw new MissingRequiredContextException((AuthorizationContextKeys.AccountLegalEntityId, accountLegalEntityId), (AuthorizationContextKeys.ProviderId, providerId));

            // todo: unit test to check that values in ProviderOperation match Operation enum names
            
            var operation = ToOperation(permissionString);
            
            //todo: add indexer to IAuthorizationContext?
        
            //UserRef for auditing/logging?

            var hasPermissionRequest = new PermissionRequest
            {
                Ukprn = providerId.Value,    //todo: check these are the same!
                EmployerAccountLegalEntityId = accountLegalEntityId.Value,
                Operation = operation
            };

            // should HasPermissions return which permissions are/aren't granted, and we could pass that on, or just a not-granted
            // as is for mvs?
            if (!await _providerRelationshipsApiClient.HasPermission(hasPermissionRequest))
                authorizationResult.AddError(new ProviderPermissionNotGranted());

            return authorizationResult;
        }

        private Operation ToOperation(string namespacedOperation)
        {
            // is there a better way than looking for '.' every time?
            var operation = namespacedOperation.Substring(namespacedOperation.IndexOf('.')+1);
            return (Operation)Enum.Parse(typeof(Operation), operation);
        }
    }
}