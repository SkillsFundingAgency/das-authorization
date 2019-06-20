using SFA.DAS.Authorization.CommitmentPermissions.Context;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.EmployerFeatures.Context;
using SFA.DAS.Authorization.EmployerUserRoles.Context;
using SFA.DAS.Authorization.NetCoreTestHarness.Models;
using SFA.DAS.Authorization.ProviderFeatures.Context;
using SFA.DAS.Authorization.ProviderPermissions.Context;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Authorization
{
    public class TestAuthorizationContextProvider : IAuthorizationContextProvider
    {
        public IAuthorizationContext GetAuthorizationContext()
        {
            var authorizationContext = new AuthorizationContext();
            
            authorizationContext.AddCommitmentPermissionValues(Cohort.Id, PartyInstance.Type, PartyInstance.Id);
            authorizationContext.AddEmployerFeatureValues(Account.Id, User.Email);
            authorizationContext.AddEmployerUserRoleValues(Account.Id, User.Ref);
            authorizationContext.AddProviderFeatureValues(Provider.Ukprn, User.Email);
            authorizationContext.AddProviderPermissionValues(AccountLegalEntity.Id, Provider.Ukprn);
            
            return authorizationContext;
        }
    } 
}