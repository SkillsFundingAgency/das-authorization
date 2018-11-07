using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeaturesAuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => EmployerFeature.Namespace;

        public Task<AuthorizationResult> GetAuthorizationResultAsync(IEnumerable<string> employerFeatures, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (!employerFeatures.Any())
                return Task.FromResult(authorizationResult);

            var accountId = authorizationContext.Get<int>(AuthorizationContextKey.AccountId);
            var userEmail = authorizationContext.Get<string>(AuthorizationContextKey.UserEmail);
            
            /*authorizationResult.AddError(new EmployerFeatureNotEnabled());
            authorizationResult.AddError(new EmployerFeatureUserNotWhitelisted());
            authorizationResult.AddError(new EmployerFeatureAgreementNotSigned());*/

            return Task.FromResult(authorizationResult);
        }
    }
}