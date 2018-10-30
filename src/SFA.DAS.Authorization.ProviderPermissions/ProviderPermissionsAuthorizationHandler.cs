using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client;

namespace SFA.DAS.Authorization.ProviderPermissions
{
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
            var authorizationResult = new AuthorizationResult();
            var providerPermissions = options.Intersect(ProviderPermissions).ToList();

            if (providerPermissions.Any())
            {
                var accountLegalEntityId = authorizationContext.Get<long>(AuthorizationContextKeys.AccountLegalEntityId);
                var providerId = authorizationContext.Get<long>(AuthorizationContextKeys.ProviderId);

                //todo: add indexer to IAuthorizationContext?
                //todo: initialise authcontext with collection literal
                
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