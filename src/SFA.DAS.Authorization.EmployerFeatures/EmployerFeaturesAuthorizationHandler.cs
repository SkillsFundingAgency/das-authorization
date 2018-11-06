using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeaturesAuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => EmployerFeatures.Namespace;

        public Task PopulateAuthorizationResultAsync(AuthorizationResult authorizationResult,
            IEnumerable<string> employerFeatures, IAuthorizationContext authorizationContext)
        {
            if (employerFeatures.Any())
            {
                var accountId = authorizationContext.Get<int>(AuthorizationContextKeys.AccountId);
                var userEmail = authorizationContext.Get<string>(AuthorizationContextKeys.UserEmail);
                
                /*authorizationResult.AddError(new EmployerFeatureNotEnabled());
                authorizationResult.AddError(new EmployerFeatureUserNotWhitelisted());
                authorizationResult.AddError(new EmployerFeatureAgreementNotSigned());*/
            }

            return Task.CompletedTask;
        }
    }
}