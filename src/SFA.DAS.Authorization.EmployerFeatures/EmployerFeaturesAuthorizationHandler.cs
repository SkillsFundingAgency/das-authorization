using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class EmployerFeaturesAuthorizationHandler : IAuthorizationHandler
    {
        private static readonly IEnumerable<string> EmployerFeatures = typeof(EmployerFeatures).GetFields().Select(f => f.GetRawConstantValue()).Cast<string>().ToList();
        
        public Task<AuthorizationResult> GetAuthorizationResultAsync(IEnumerable<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();
            var providerPermissions = options.Intersect(EmployerFeatures).ToList();

            if (providerPermissions.Any())
            {
                var accountId = authorizationContext.Get<int>(ContextKeys.AccountId);
                var userEmail = authorizationContext.Get<string>(ContextKeys.UserEmail);
                
                /*authorizationResult.AddError(new EmployerFeatureNotEnabled());
                authorizationResult.AddError(new EmployerFeatureUserNotWhitelisted());
                authorizationResult.AddError(new EmployerFeatureAgreementNotSigned());*/
            }

            return Task.FromResult(authorizationResult);
        }
    }
}