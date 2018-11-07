using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => EmployerFeature.Namespace;

        public Task<AuthorizationResult> GetAuthorizationResultAsync(IEnumerable<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Any())
            {
                var values = authorizationContext.GetEmployerFeatureValues();
                
                /*authorizationResult.AddError(new EmployerFeatureNotEnabled());
                authorizationResult.AddError(new EmployerFeatureUserNotWhitelisted());
                authorizationResult.AddError(new EmployerFeatureAgreementNotSigned());*/
            }

            return Task.FromResult(authorizationResult);
        }
    }
}