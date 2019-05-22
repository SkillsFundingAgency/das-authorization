using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Features;

namespace SFA.DAS.Authorization.ProviderFeatures
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => "ProviderFeature.";
        
        private readonly IFeatureTogglesService<ProviderFeatureToggle> _featureTogglesService;
        private readonly ILogger _logger;

        public AuthorizationHandler(IFeatureTogglesService<ProviderFeatureToggle> featureTogglesService, ILogger<AuthorizationHandler> logger)
        {
            _featureTogglesService = featureTogglesService;
            _logger = logger;
        }

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();

            if (options.Count > 0)
            {
                options.EnsureNoAndOptions();
                options.EnsureNoOrOptions();
                
                var feature = options.Single();
                var featureToggle = _featureTogglesService.GetFeatureToggle(feature);

                if (!featureToggle.IsEnabled)
                {
                    authorizationResult.AddError(new ProviderFeatureNotEnabled());
                }
                else if (featureToggle.IsWhitelistEnabled)
                {
                    var values = authorizationContext.GetProviderFeatureValues();

                    if (!featureToggle.IsUserWhitelisted(values.Ukprn, values.UserEmail))
                    {
                        authorizationResult.AddError(new ProviderFeatureUserNotWhitelisted());
                    }
                }
            }

            _logger.LogInformation($"Finished running '{GetType().FullName}' for options '{string.Join(", ", options)}' and context '{authorizationContext}' with result '{authorizationResult}'");
            
            return Task.FromResult(authorizationResult);
        }
    }
}