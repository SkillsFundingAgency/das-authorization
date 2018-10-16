﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.ProviderPermissions
{
    public class ProviderPermissionsAuthorizationHandler : IAuthorizationHandler
    {
        private static readonly IEnumerable<string> ProviderPermissions = typeof(ProviderPermissions).GetFields().Select(f => f.GetRawConstantValue()).Cast<string>().ToList();

        public Task<AuthorizationResult> GetAuthorizationResultAsync(IEnumerable<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();
            var providerPermissions = options.Intersect(ProviderPermissions).ToList();

            if (providerPermissions.Any())
            {
                var accountLegalEntityId = authorizationContext.Get<int>(AuthorizationContextKeys.AccountLegalEntityId);
                var providerId = authorizationContext.Get<Guid>(AuthorizationContextKeys.ProviderId);

                //authorizationResult.AddError(new ProviderPermissionNotGranted());
            }

            return Task.FromResult(authorizationResult);
        }
    }
}