using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.EmployerFeatures.Context;
using SFA.DAS.Authorization.EmployerFeatures.Errors;
using SFA.DAS.Authorization.EmployerFeatures.Models;
using SFA.DAS.Authorization.Errors;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Options;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.Authorization.EmployerFeatures.Handlers
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => "EmployerFeature.";
        
        private readonly IFeatureTogglesService<EmployerFeatureToggle> _featureTogglesService;

        public AuthorizationHandler(IFeatureTogglesService<EmployerFeatureToggle> featureTogglesService)
        {
            _featureTogglesService = featureTogglesService;
        }

        private static readonly AuthorizationResult OkayResult = new AuthorizationResult();

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            AuthorizationResult result = null;

            if (options.Count > 0)
            {
                options.EnsureNoOrOptions();

                foreach (var option in options)
                {
                    var featureError = EvaluateSingleFeature(authorizationContext, option);
                    if (featureError != null)
                    {
                        (result = result ?? new AuthorizationResult()).AddError(featureError);
                    }
                }
            }
            
            return Task.FromResult(result ?? OkayResult);
        }

        private AuthorizationError EvaluateSingleFeature(IAuthorizationContext authorizationContext, string feature)
        {
            var featureToggle = _featureTogglesService.GetFeatureToggle(feature);

            if (!featureToggle.IsEnabled)
            {
                return new EmployerFeatureNotEnabled();
            }

            if (featureToggle.IsWhitelistEnabled)
            {
                var values = authorizationContext.GetEmployerFeatureValues();

                if (!featureToggle.IsUserWhitelisted(values.AccountId, values.UserEmail))
                {
                    return new EmployerFeatureUserNotWhitelisted();
                }
            }

            return null;
        }
    }
}