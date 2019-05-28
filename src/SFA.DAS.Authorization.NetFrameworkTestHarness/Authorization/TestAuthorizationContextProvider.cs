using SFA.DAS.Authorization.CommitmentPermissions;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.NetFrameworkTestHarness.Models;
using SFA.DAS.Authorization.ProviderFeatures;
using SFA.DAS.Authorization.ProviderPermissions;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.Authorization
{
    public class TestAuthorizationContextProvider : IAuthorizationContextProvider
    {
        public IAuthorizationContext GetAuthorizationContext()
        {
            var authorizationContext = new AuthorizationContext();
            
            authorizationContext.AddCommitmentPermissionValues(Cohort.Id, Party.Type, Party.Id);
            authorizationContext.AddEmployerFeatureValues(Account.Id, User.Email);
            authorizationContext.AddEmployerUserRoleValues(Account.Id, User.Ref);
            authorizationContext.AddProviderFeatureValues(Provider.Ukprn, User.Email);
            authorizationContext.AddProviderPermissionValues(AccountLegalEntity.Id, Provider.Ukprn);
            
            return authorizationContext;
        }
    } 
}