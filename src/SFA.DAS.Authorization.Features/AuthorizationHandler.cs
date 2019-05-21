using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Authorization.Features
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => Feature.Prefix;
        
        private readonly IFeatureTogglesService _featureTogglesService;
        private readonly ILogger _logger;

        public AuthorizationHandler(IFeatureTogglesService featureTogglesService, ILogger<AuthorizationHandler> logger)
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
                    authorizationResult.AddError(new FeatureNotEnabled());
                }
            }

            _logger.LogInformation($"Finished running '{GetType().FullName}' for options '{string.Join(", ", options)}' with result '{authorizationResult.GetDescription()}'");
            
            return Task.FromResult(authorizationResult);
        }
    }
}