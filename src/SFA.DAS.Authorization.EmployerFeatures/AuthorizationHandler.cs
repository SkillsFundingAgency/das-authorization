using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Features;

namespace SFA.DAS.Authorization.EmployerFeatures
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => "EmployerFeature.";
        
        private readonly IFeatureTogglesService<EmployerFeatureToggle> _featureTogglesService;
        private readonly ILogger _logger;

        public AuthorizationHandler(IFeatureTogglesService<EmployerFeatureToggle> featureTogglesService, ILogger<AuthorizationHandler> logger)
        {
            _featureTogglesService = featureTogglesService;
            _logger = logger;
        }

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Any())
            {
                options.EnsureNoAndOptions();
                options.EnsureNoOrOptions();
                
                var feature = options.Single();
                var featureToggle = _featureTogglesService.GetFeatureToggle(feature);

                if (!featureToggle.IsEnabled)
                {
                    authorizationResult.AddError(new EmployerFeatureNotEnabled());
                }
                else if (featureToggle.IsWhitelistEnabled)
                {
                    var values = authorizationContext.GetEmployerFeatureValues();
                    
                    if (!featureToggle.IsUserWhitelisted(values.AccountId, values.UserEmail))
                    {
                        authorizationResult.AddError(new EmployerFeatureUserNotWhitelisted());
                    }
                }
            }

            _logger.LogInformation($"Finished running '{GetType().FullName}' for options '{string.Join(", ", options)}' with result '{authorizationResult.GetDescription()}'");
            
            return Task.FromResult(authorizationResult);
        }
    }
}