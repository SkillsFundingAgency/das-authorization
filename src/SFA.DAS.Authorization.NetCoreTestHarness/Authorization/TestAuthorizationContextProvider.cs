﻿using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.NetCoreTestHarness.Models;
using SFA.DAS.Authorization.ProviderPermissions;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Authorization
{
    public class TestAuthorizationContextProvider : IAuthorizationContextProvider
    {
        public IAuthorizationContext GetAuthorizationContext()
        {
            var authorizationContext = new AuthorizationContext();
            
            authorizationContext.AddEmployerFeatureValues(Account.Id, User.Email);
            authorizationContext.AddEmployerUserRoleValues(Account.Id, User.Ref);
            authorizationContext.AddProviderPermissionValues(AccountLegalEntity.Id, Provider.Ukprn);
            
            return authorizationContext;
        }
    } 
}