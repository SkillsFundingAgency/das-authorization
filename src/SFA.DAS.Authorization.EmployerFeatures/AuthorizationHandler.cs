using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => EmployerFeature.Namespace;
        
        private readonly IFeatureTogglesService _featureTogglesService;

        public AuthorizationHandler(IFeatureTogglesService featureTogglesService)
        {
            _featureTogglesService = featureTogglesService;
        }

        public async Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Any())
            {
                options.EnsureNoAndOptions();
                options.EnsureNoOrOptions();

                var values = authorizationContext.GetEmployerFeatureValues();
                var feature = options.Select(o => o.ToEnum<Feature>()).Single();
                var featureToggle = await _featureTogglesService.GetFeatureToggle(feature).ConfigureAwait(false);

                if (!featureToggle.IsEnabled)
                {
                    authorizationResult.AddError(new EmployerFeatureNotEnabled());
                }
                else if (!featureToggle.IsUserEmailWhitelisted(values.UserEmail))
                {
                    authorizationResult.AddError(new EmployerFeatureUserNotWhitelisted());
                }
            }

            return authorizationResult;
        }
    }
}