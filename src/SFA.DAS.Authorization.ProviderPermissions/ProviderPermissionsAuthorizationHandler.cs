using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client;

namespace SFA.DAS.Authorization.ProviderPermissions
{
   #if false
    public static class AuthorizationContextExtensions
    {
        //how does this work when multiple handlers want the same thing. centralise?
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
    
    public class ProviderPermissionsAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;
        
        public ProviderPermissionsAuthorizationHandler(IProviderRelationshipsApiClient providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }
        
        private static readonly IEnumerable<string> ProviderPermissions = typeof(ProviderPermissions).GetFields().Select(f => f.GetRawConstantValue()).Cast<string>().ToList();

        public Task<AuthorizationResult> GetAuthorizationResultAsync(IEnumerable<string> options, IAuthorizationContext authorizationContext)
        {
            //todo: move this boilerplate into AuthorizationService?
            var authorizationResult = new AuthorizationResult();
            var providerPermissions = options.Intersect(ProviderPermissions);

            if (providerPermissions.Any())
            {
                //how does consumer know what type to use? can we help? introduce type safety. type safe-extensions on ContextProvider?
                var accountLegalEntityId = authorizationContext.Get<long?>(AuthorizationContextKeys.AccountLegalEntityId);
                var providerId = authorizationContext.Get<long?>(AuthorizationContextKeys.ProviderId);

                //todo: add indexer to IAuthorizationContext?
            
                //UserRef for auditing/logging?

                //todo: talk to paul about accepting IEnumerable<string> for multiple permissions
                //var hasPermissionRequest = new HasPermissionsRequest(accountLegalEntityId, providerId, providerPermissions);

                // should HasPermissions return which permissions are/aren't granted, and we could pass that on, or just a not-granted
                // as is for mvs?
//                if (!await _providerRelationshipsApiClient.HasPermissions(hasPermissionRequest))
//                    authorizationResult.AddError(new ProviderPermissionNotGranted());
            }

            return Task.FromResult(authorizationResult);
        }
    }
}